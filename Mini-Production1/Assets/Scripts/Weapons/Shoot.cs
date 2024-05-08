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
    private float force;

    [SerializeField]
    private float fireRate = 1.0f;
    
    private float nextFireTime = 0.0f;
    private float delay = 5.0f;
    private float angle = 45.0f;

    public void Fire()
    {
        // Check if the current time is greater than or equal to the next allowed fire time.
        if (Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + 1.0f / fireRate; // Update nextFireTime to the current time, plus the inverse of fireRate, to allow the next fire.

            GameObject bullet = Instantiate(projecile, launcher.position, launcher.rotation); // Creates an instance of a projectile.

            Vector3 direction = Quaternion.Euler(angle, 0, 0) * launcher.forward;
            bullet.GetComponent<Rigidbody>().AddForce(direction * force, ForceMode.Impulse); // Applies force to the projectile.

            Destroy(bullet, delay); // Self destroys after delay.
        }
    }
}
