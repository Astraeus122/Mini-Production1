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
        if (Triggered && Time.time >= nextFireTime)
        {
            FireTurret();
        }
    }

    public void FireTurret()
    {
        if (Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + 1.0f / fireRate;
            GameObject bullet = Instantiate(projecile, launcher.position, launcher.rotation);
            bullet.GetComponent<Rigidbody>().AddForce(launcher.forward * force, ForceMode.Impulse);
            Destroy(bullet, 1.0f + 0.5f * cannonPowerLevel); // Longer lifetime for higher power levels
        }
    }

    public void Cata()
    {
        if (Time.time >= nextFireTime)
        {
            animator.Play("CatapaultAttack");

            Invoke("FireCatapult", 0.5f);
        }
    }

    public void FireCatapult()
    {
        nextFireTime = Time.time + 1.0f / fireRate;
        GameObject bullet = Instantiate(projecile, launcher.position, launcher.rotation);
        Vector3 direction = Quaternion.Euler(angle, 0, 0) * launcher.forward + launcher.up;
        bullet.GetComponent<Rigidbody>().AddForce(direction * force, ForceMode.Impulse);
        Destroy(bullet, 2.0f + 0.5f * cannonPowerLevel); // Longer lifetime for higher power levels

    }
}
