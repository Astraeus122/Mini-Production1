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

    private float angle = 45.0f;

    private float nextFireTime = 0.0f;

    public void Fire()
    {
        // Check if the current time is greater than or equal to the next allowed fire time.
        if (Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + 1.0f / fireRate; // Update nextFireTime to the current time, plus the inverse of fireRate, to allow the next fire.

            GameObject bullet = Instantiate(projecile, launcher.position, launcher.rotation); // Creates an instance of a projectile.

            if (!turretWeapon)
            {
                Vector3 direction = Quaternion.Euler(0, 0, angle) * launcher.up;
                bullet.GetComponent<Rigidbody>().AddForce(direction * force, ForceMode.Impulse); // Applies force to the projectile.
                Destroy(bullet, 3.0f);
                return; // If not a turret, get fucked
            }

            bullet.GetComponent<Rigidbody>().AddForce(launcher.forward * force, ForceMode.Impulse); // Applies force to the projectile.

            Destroy(bullet, 1.0f); // Self destroys after delay.
        }
    }
}
