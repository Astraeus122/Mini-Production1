using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class BoatMovement : MonoBehaviour
{
    public float Speed = 6.0f;
    public float inertiaDuration = 2.0f; // Time in seconds to stop completely from max speed
    public float tiltAngle = 10.0f; // Maximum tilt angle
    public float maxHitPoints = 5;
    public Leak[] leakSites;
    public float leakSusceptibility = 0.3f;
    public float yawAmount = 2.0f; // The maximum yaw angle
    public float yawSpeed = 2.0f; // How quickly the boat yaws
    private bool isInDrainLiquid = false;
    public float drainDamagePerSecond = 1f;
    [SerializeField]
    Image healthFillUI;
    [SerializeField]
    Transform mountPoint;
    [SerializeField]
    AudioSource HitAudioSource;

    public bool hasAdvancedNavigation = false;
    [SerializeField]
    public Camera miniMapCamera;  // Reference to the mini-map camera
    [SerializeField]
    public GameObject miniMapPanel;  // Reference to the mini-map UI Panel

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

    public string DangerTag = "";
    public string CrateTag = "";
    public string DrainTag = "";

    public UnityEvent OnBoatDied;

    public float SteeringInput { get; set; }

    public float ThrustInput { get; set; }


    [SerializeField] private Transform boatWaterFill = null;
    [SerializeField] private Vector2 boatWaterMinMaxY = new Vector2(0, 1);

    public Vector2 Movement { get { return movement; } }

    public void UpgradeMaxHealth(float additionalHealth)
    {
        maxHitPoints += additionalHealth;
        currentHitPoints = maxHitPoints; // reset current health to max
    }

    public void UpgradeSpeed(float additionalSpeed)
    {
        Speed += additionalSpeed;
    }

    void Start()
    {
        screenShake = Camera.main.GetComponent<ScreenShake>();
        currentHitPoints = maxHitPoints;
        originalRotation = transform.rotation; // Save the original rotation
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

        mountPoint.rotation = Quaternion.identity;

        if (isInDrainLiquid)
        {
            TakeDamage(drainDamagePerSecond * Time.deltaTime);
        }

        ProcessLeaks();
    }

    void FixedUpdate()
    {
        MoveBoat();
    }

    private void ApplyInertia()
    {
        if (movement.sqrMagnitude != 0)
        {
            // Reduce the lateralMovement over time to simulate inertia
            float inertiaEffect = Time.deltaTime;

            movement *= inertiaEffect;
        }
        else
        {
            // Gradually reset rotation to original
            transform.rotation = Quaternion.Slerp(transform.rotation, originalRotation, Time.deltaTime * yawSpeed);
        }
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

        foreach (var leakSite in leakSites)
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

    private void ProcessLeaks()
    {
        foreach (var leak in leakSites)
        {
            TakeDamage(leak.LeakStrength * leakSusceptibility * Time.deltaTime);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(DangerTag))
        {
            if (other.TryGetComponent(out Obstacle_Scr obstacle))
            {
                if (obstacle.Die())
                    TakeDamage(other.GetComponent<Obstacle_Scr>().Value,
                        other.transform
                            .position); //TODO add damage field to the obstacle instance, different ones could do different dmg
            }
        }

        if (other.CompareTag(CrateTag))
        {
            GameManager.Instance.RepairResources += (int)other.GetComponent<Obstacle_Scr>().Value;
            other.gameObject.GetComponent<Obstacle_Scr>().Die();
        }

        if (other.CompareTag(DrainTag))
        {
            // Trigger screen shake
            if (screenShake != null)
            {
                screenShake.TriggerShake();
            }

            isInDrainLiquid = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(DrainTag))
        {
            isInDrainLiquid = false;
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

    public float scrapMagnetRadius = 0f;

    public void IncreaseScrapMagnetRadius(float increment)
    {
        scrapMagnetRadius += increment;
        // Logic to pull in items within `scrapMagnetRadius`
    }

    public Light boatLight; // Attach a Light component in the Unity editor

    public void EnhanceFloodlights(float additionalRange)
    {
        if (boatLight != null)
        {
            boatLight.range += additionalRange;
        }
    }

    public float shieldStrength = 0;
    public bool shieldActive = false;

    public void ActivateShield(float strength)
    {
        shieldStrength = strength;
        shieldActive = true;
        // Visual or gameplay effects to indicate shield is active
    }

    public void HandleDamageWithShield(float damageAmount)
    {
        if (shieldActive && shieldStrength > 0)
        {
            shieldStrength -= damageAmount;
            if (shieldStrength <= 0)
            {
                shieldActive = false;  // Deactivate shield if depleted
                                       // Additional effects for shield deactivation
            }
        }
        else
        {
            TakeDamage(damageAmount);  // Regular damage handling
        }
    }

}