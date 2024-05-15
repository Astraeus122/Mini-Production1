using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class BoatMovement : MonoBehaviour
{
    public float Speed = 6.0f;
    public float inertiaDuration = 2.0f; // Time in seconds to stop completely from max speed
    public float tiltAngle = 10.0f; // Maximum tilt angle
    public float maxHitPoints = 5;
    [field: SerializeField]
    public Leak[] LeakSites { get; private set; }
    [field: SerializeField]
    public Shoot[] Guns { get; private set; }

    [field: SerializeField]
    public List<CrewmateDriver> AiRoster { get; private set; } = new List<CrewmateDriver>();

    public float leakSusceptibility = 0.3f;
    public float yawAmount = 2.0f; // The maximum yaw angle
    public float yawSpeed = 2.0f; // How quickly the boat yaws

    [SerializeField]
    Image healthFillUI;
    [SerializeField]
    private AudioSource HitAudioSource;

    // Upgrade stats
    public bool hasAdvancedNavigation = false;
    [SerializeField]
    private Camera miniMapCamera;  // Reference to the mini-map camera
    [SerializeField]
    private GameObject miniMapPanel;  // Reference to the mini-map UI Panel

    public float scrapMagnetRadius = 0f;  // Initial radius of the scrap magnet effect
    public float scrapMagnetStrength = 0f;  // How strongly items are pulled towards the boat
    [SerializeField]
    private Light boatLight;

    public int maxShieldHits = 0;  // Maximum hits the shield can take
    public int currentShieldHits = 0;  // Current hits before depletion
    public float shieldRegenCooldown = 30f;  // Time in seconds to regenerate shield
    public float shieldStrength = 0;
    public bool shieldActive = false;

    public float collisionDamageReduction = 0f;  // Percentage of damage reduction from collisions

    public float baseTemporalShiftDuration = 3.0f;
    private float temporalShiftIncrement = 0.5f; // Duration increase per upgrade
    public int temporalShiftUpgradeLevel = 0; 
    public float temporalShiftDuration => baseTemporalShiftDuration + temporalShiftUpgradeLevel * temporalShiftIncrement;

    public float temporalShiftCooldown = 30.0f;
    private float temporalShiftTimer = 0.0f; // Ensure it's initially 0 for availability
    private bool isTemporalShiftActive = false;

    public float healthRegenerationRate = 0;  // Health gained per second
    private float healthRegenerationCooldown = 0.1f;  // Cooldown between regenerations
    private float lastRegenerationTime;

    [SerializeField] private GameObject jeremiahPrefab; // Assign this in the Unity Editor
    [SerializeField] private Transform spawnPoint; // Assign or calculate a spawn point for Jeremiah
    private int jeremiahCount = 0;

    [SerializeField]
    private GameObject shieldVisualEffect;
    [SerializeField]
    private AudioSource shieldAudioSource;
    [SerializeField]
    private AudioClip shieldImpactSound;
    [SerializeField]
    private AudioClip shieldCrackSound; 

    public ScreenShake screenShake;
    private float currentHitPoints;
    private Vector2 movement;
    public Quaternion originalRotation;

    public GameObject despawnVFX;

    [SerializeField]
    [Tooltip("Clamp position of ship (min value)")]
    [Range(0, -6f)]
    private float minX;

    [SerializeField]
    [Range(0, 6f)]
    [Tooltip("Clamp position of ship (max value)")]
    private float maxX;

    [SerializeField]
    [Tooltip("Clamp position of ship (min value)")]
    [Range(0, -10f)]
    private float minZ;

    [SerializeField]
    [Tooltip("Clamp position of ship (min value)")]
    [Range(0, 10f)]
    private float maxZ;

    public UnityEvent OnBoatDied;

    public float SteeringInput { get; set; }

    public float ThrustInput { get; set; }


    [SerializeField] private Transform boatWaterFill = null;
    [SerializeField] private Vector2 boatWaterMinMaxY = new Vector2(0, 1);

    public Vector2 Movement { get { return movement; } }

    public event Action<string> OnCrewCommand;

    public void IssueCrewCommand(string cmd)
    {
        OnCrewCommand?.Invoke(cmd);
    }
    public void UpgradeMaxHealth(float additionalHealth)
    {
        maxHitPoints += additionalHealth;
        currentHitPoints = maxHitPoints; // reset current health to max
    }

    public void UpgradeSpeed(float additionalSpeed)
    {
        Speed += additionalSpeed;
    }

    public void ReceiveDamage(float damage, Vector3 impactPoint)
    {
        if (shieldActive)
        {
            HandleDamageWithShield(damage);
        }
        else
        {
            // Apply collision damage reduction if shield is not active
            float reducedDamage = damage * (1 - collisionDamageReduction);
            TakeDamage(reducedDamage, impactPoint);
        }
    }

    void Start()
    {
        screenShake = Camera.main.GetComponent<ScreenShake>();
        currentHitPoints = maxHitPoints;
        originalRotation = transform.rotation; // Save the original rotation
        print(name + " started with " + currentHitPoints + " " +  maxHitPoints);
    }

    private void Update()
    {
        movement.x += SteeringInput * Speed * Time.deltaTime;
        movement.x = Mathf.Clamp(movement.x, -Speed, Speed);

        movement.y += ThrustInput * Speed * Time.deltaTime;
        movement.y = Mathf.Clamp(movement.y, -Speed, Speed);


        float inertiaEffect = Speed / inertiaDuration * Time.deltaTime;

        movement.x = Mathf.Sign(movement.x) * Mathf.Max(Mathf.Abs(movement.x) - inertiaEffect, 0);
        movement.y = Mathf.Sign(movement.y) * Mathf.Max(Mathf.Abs(movement.y) - inertiaEffect, 0);

        TiltBoat(SteeringInput);

        ProcessLeaks();

        if (scrapMagnetRadius > 0)
        {
            Collider[] hits = Physics.OverlapSphere(transform.position, scrapMagnetRadius, LayerMask.GetMask("CrateLayer"));
            foreach (var hit in hits)
            {
                float distance = Vector3.Distance(transform.position, hit.transform.position);
                Vector3 pullDirection = (transform.position - hit.transform.position).normalized;
                float pullStrength = Mathf.Lerp(scrapMagnetStrength, 0, distance / scrapMagnetRadius);

                hit.GetComponent<Rigidbody>().AddForce(pullDirection * pullStrength * Time.deltaTime);
            }
        }

        // Handle Temporal Shift Activation
        if (Input.GetKeyDown(KeyCode.Q) && temporalShiftTimer <= 0)
        {
            ActivateTemporalShift();
        }

        // Manage active Temporal Shift
        if (isTemporalShiftActive)
        {
            temporalShiftTimer -= Time.unscaledDeltaTime;
            if (temporalShiftTimer <= 0)
            {
                DeactivateTemporalShift();
            }
        }
        else if (temporalShiftTimer > 0)  // Cooldown management
        {
            temporalShiftTimer -= Time.deltaTime;
            temporalShiftTimer = Mathf.Max(temporalShiftTimer, 0);
        }

        // Handle passive health regeneration
        if (Time.time >= lastRegenerationTime + healthRegenerationCooldown && healthRegenerationRate > 0)
        {
            Heal(healthRegenerationRate * healthRegenerationCooldown);
            lastRegenerationTime = Time.time;
        }
    }
    public void Heal(float healthAmount)
    {
        TakeDamage(-healthAmount);  // Using negative damage to heal
    }

    void FixedUpdate()
    {
        MoveBoat();
    }

    private void MoveBoat()
    {
        Vector3 position = transform.position;

        position.x += movement.x * Time.deltaTime;
        position.z += movement.y * Time.deltaTime;

        position.x = Mathf.Clamp(position.x, minX, maxX); // Adjust based on your game's boundaries
        position.z = Mathf.Clamp(position.z, minZ, maxZ);

        transform.position = position;
    }

    private void TiltBoat(float input)
    {
        // Calculate the tilt based on input direction
        float tilt = input * -tiltAngle;

        // Calculate yaw based on input
        float yaw = input * yawAmount;

        // Combine the original rotation with tilt and yaw
        Quaternion targetRotation = originalRotation * Quaternion.Euler(0, yaw, tilt);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * yawSpeed);
    }

    public void TakeDamage(float damage, Vector3 impactPoint, float maxDistFromImpact = Mathf.Infinity,
        float releakStrengthThreshold = 0.5f)
    {
        if (!TakeDamage(damage)) return;
        // Trigger screen shake
        if (screenShake != null)
        {
            screenShake.TriggerShake();
        }

        // explosion on damage taken
        var explosion = Instantiate(despawnVFX);
        explosion.transform.position = gameObject.transform.position;

        HitAudioSource.Play();
        // spring leak that is closest to the impact site and less than cutoff distance away
        float sqrDistClosest = Mathf.Infinity;
        Leak closest = null;

        foreach (var leakSite in LeakSites)
        {
            if (leakSite.enabled && leakSite.LeakStrength > releakStrengthThreshold) continue;

            Vector3 delta = leakSite.transform.position - impactPoint;
            if (delta.sqrMagnitude < sqrDistClosest && delta.sqrMagnitude < maxDistFromImpact * maxDistFromImpact)
            {
                closest = leakSite;
                sqrDistClosest = delta.sqrMagnitude;
            }
        }

        if (closest)
        {
            closest.enabled = false;
            closest.enabled = true;
        }
    }

    public bool TakeDamage(float damage)
    {
        print(name +"Took damage");
        if (damage == 0) return false;
        if (damage > 0 && currentHitPoints <= 0) return false;
        if (damage < 0 && currentHitPoints >= maxHitPoints) return false;

        
        // it is take damage, but supports both direcitons / can be used to heal
        currentHitPoints = Mathf.Clamp(currentHitPoints - damage, 0, maxHitPoints);

        float fillAmount = 1 - currentHitPoints / maxHitPoints;

        if (healthFillUI)
            healthFillUI.fillAmount = Mathf.Clamp01(fillAmount);

        if (boatWaterFill)
        {
            boatWaterFill.gameObject.SetActive(fillAmount > 0);
            boatWaterFill.localPosition = new Vector3(boatWaterFill.localPosition.x,
                Mathf.Lerp(boatWaterMinMaxY.x, boatWaterMinMaxY.y, fillAmount), boatWaterFill.localPosition.z);
        }


        if (currentHitPoints <= 0)
        {
            Speed = 0;

            // sink boat
            StartCoroutine(SinkBoatRoutine());

            GameManager.Instance.StopGame();
            OnBoatDied?.Invoke();
        }

        return true;
    }

    private IEnumerator SinkBoatRoutine()
    {
        while (true) 
        {
            transform.position = new Vector3(transform.position.x, transform.position.y - 0.003f, transform.position.z);
            yield return new WaitForEndOfFrame();
        }
    }

    public void AddLeak(Leak leak)
    {
        if (!LeakSites.Contains(leak))
        {
            leak.UpdateLeakRepairDuration(); // Adjust the repair duration
        }
    }

    private void ProcessLeaks()
    {
        foreach (var leak in LeakSites)
        {
            TakeDamage(leak.LeakStrength * leakSusceptibility * Time.deltaTime);
        }
    }

    public void ActivateAdvancedNavigation()
    {
        hasAdvancedNavigation = true;
        if (miniMapCamera != null)
            miniMapCamera.enabled = true;  // Enable the mini-map camera
        if (miniMapPanel != null)
            miniMapPanel.SetActive(true);  // Show the mini-map panel
    }

    public void IncreaseScrapMagnetRadius(float incrementRadius, float incrementStrength)
    {
        scrapMagnetRadius += incrementRadius;
        scrapMagnetStrength += incrementStrength;
    }

    public void EnhanceFloodlights(float additionalIntensity)
    {
        if (boatLight != null)
        {
            boatLight.intensity += additionalIntensity; // Increases the light range
            boatLight.range += additionalIntensity / 2;
        }
        else
        {
            Debug.LogError("No light component found on the boat!");
        }
    }

    public void ActivateShield()
    {
        currentShieldHits = maxShieldHits;
        shieldActive = true;
        if (shieldVisualEffect != null)
            shieldVisualEffect.SetActive(true);
        if (shieldAudioSource != null)
        {
            shieldAudioSource.Play();
        }

        StopCoroutine("RegenerateShield");
        StartCoroutine("RegenerateShield");
    }

    IEnumerator RegenerateShield()
    {
        yield return new WaitForSeconds(shieldRegenCooldown);
        if (currentShieldHits < maxShieldHits)
        {
            currentShieldHits++;
            if (shieldAudioSource != null && currentShieldHits == maxShieldHits)
            {
                shieldAudioSource.Play();
            }
            StartCoroutine("RegenerateShield");
        }
        else
        {
            shieldActive = false;
            if (shieldVisualEffect != null)
                shieldVisualEffect.SetActive(false);
        }
    }

    public void HandleDamageWithShield(float damageAmount)
    {
        if (shieldActive && currentShieldHits > 0)
        {
            currentShieldHits--;
            if (shieldAudioSource != null && shieldImpactSound != null)
            {
                shieldAudioSource.PlayOneShot(shieldImpactSound);  // Play impact sound
            }
            if (currentShieldHits <= 0)
            {
                shieldActive = false;
                if (shieldVisualEffect != null)
                    shieldVisualEffect.SetActive(false);
                if (shieldAudioSource != null && shieldCrackSound != null)
                {
                    shieldAudioSource.PlayOneShot(shieldCrackSound);  // Play crack sound
                }
            }
        }
        else
        {
            TakeDamage(damageAmount);
        }
    }

    public void ActivateShieldVisualEffect(bool isActive)
    {
        if (shieldVisualEffect != null)
        {
            shieldVisualEffect.SetActive(isActive);
        }
    }
    public void ActivateTemporalShift()
    {
        if (temporalShiftUpgradeLevel > 0 && temporalShiftTimer <= 0)
        {
            Time.timeScale = 0.5f;
            Time.fixedDeltaTime = 0.02f * Time.timeScale;
            isTemporalShiftActive = true;
            temporalShiftTimer = temporalShiftDuration; // Start the duration timer
        }
        else
        {
            Debug.Log("Cannot activate Temporal Shift: Level=" + temporalShiftUpgradeLevel + ", Timer=" + temporalShiftTimer);
        }
    }

    private void DeactivateTemporalShift()
    {
        Time.timeScale = 1.0f;
        Time.fixedDeltaTime = 0.02f;
        isTemporalShiftActive = false;
        temporalShiftTimer = temporalShiftCooldown; // Reset cooldown
    }
    public void AddJeremiah()
    {
        if (jeremiahPrefab != null && spawnPoint != null && spawnPoint != null)
        {
            GameObject jeremiahInstance = Instantiate(jeremiahPrefab, spawnPoint.position, Quaternion.identity);
            jeremiahCount++;

            // Set the boat and standby location on the spawned Jeremiah
            CrewmateDriver jeremiahDriver = jeremiahInstance.GetComponent<CrewmateDriver>();
            if (jeremiahDriver != null)
            {
                jeremiahDriver.Initialize(this, spawnPoint);
            }
            else
            {
                Debug.LogError("CrewmateDriver component not found on the spawned Jeremiah prefab!");
            }

            Debug.Log("Jeremiah added, total: " + jeremiahCount);
        }
        else
        {
            Debug.LogError("Jeremiah prefab, spawn point, or standby location not set");
        }
    }
}