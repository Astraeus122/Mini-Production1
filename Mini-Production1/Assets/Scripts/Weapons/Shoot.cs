using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    [SerializeField]
    private GameObject projecile;

    [SerializeField]
    private Transform launcher;

    [SerializeField]
    private bool turretWeapon = false;

    [SerializeField]
    private float force;

    [SerializeField]
    private float fireRate = 1.0f;

    private float nextFireTime = 0.0f;
    private float angle = 45.0f;

    public int cannonPowerLevel = 0;
    public int fireRateLevel = 0;
    public float powerIncrement = 10.0f;  // Increment of power per upgrade level
    public float rateIncrement = 0.2f;    // Increment of fire rate per upgrade level

    public bool Triggered { get; set; }

    public Animator animator;

    private bool isCatapultTriggered = false; // Flag to prevent multiple triggers

    public void UpgradeCannon()
    {
        cannonPowerLevel++;
        fireRateLevel++;
        force += powerIncrement;
        fireRate += rateIncrement;
        fireRate = Mathf.Clamp(fireRate, 0.1f, 10f); // Ensure the fire rate does not exceed practical limits
    }

    public void Update()
    {
        if (Triggered)
        {
            if (turretWeapon)
            {
                FireTurret();
            }
            else if (!isCatapultTriggered) // Check the flag before firing the catapult
            {
                FireCatapult();
            }
        }
    }

    private void Turret()
    {
        animator.Play("ShooterAttack");
        nextFireTime = Time.time + 1.0f / fireRate;
        GameObject bullet = Instantiate(projecile, launcher.position, launcher.rotation);
        bullet.GetComponent<Rigidbody>().AddForce(launcher.forward * force, ForceMode.Impulse);
        bullet.GetComponent<Rigidbody>().useGravity = false;
        Destroy(bullet, 1.0f + 0.5f * cannonPowerLevel); // Longer lifetime for higher power levels

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayTurretFire();
        }
    }

    public void FireTurret()
    {
        if (Time.time >= nextFireTime)
        {
            Invoke("Turret", 0.0f);
        }
    }

    private void Catapult()
    {
        nextFireTime = Time.time + 1.0f / fireRate;
        GameObject bullet = Instantiate(projecile, launcher.position, launcher.rotation);
        Vector3 direction = Quaternion.Euler(angle, 0, 0) * launcher.forward + launcher.up;
        bullet.GetComponent<Rigidbody>().AddForce(direction * force, ForceMode.Impulse);
        bullet.GetComponent<Rigidbody>().useGravity = true;
        Destroy(bullet, 2.0f + 0.5f * cannonPowerLevel); // Longer lifetime for higher power levels

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayCatapultFire();
        }

        // Reset the flag after firing
        isCatapultTriggered = false;
    }

    public void FireCatapult()
    {
        if (Time.time >= nextFireTime && !isCatapultTriggered)
        {
            animator.Play("CatapaultAttack");
            isCatapultTriggered = true; // Set the flag to true when the catapult is triggered
            Invoke("Catapult", 0.5f);
        }
    }
}
