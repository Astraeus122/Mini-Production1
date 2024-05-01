using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public GameObject cannonBall;
    public Transform barrel;
    
    public float force;
    public float fireRate = 1.0f;
    
    private float nextFireTime = 0.0f;
    private float delay = 2.0f;

    public void Fire()
    {
        // Check if the current time is greater than or equal to the next allowed fire time.
        if (Time.time >= nextFireTime)
        {
            nextFireTime = Time.time + 1.0f / fireRate; // Update nextFireTime to the current time, plus the inverse of fireRate, to allow the next fire.

            GameObject bullet = Instantiate(cannonBall, barrel.position, barrel.rotation); // Creates an instance of a cannonball.
            bullet.GetComponent<Rigidbody>().velocity = barrel.forward * force * Time.deltaTime; // Applies velocity to the cannonball.

            Destroy(bullet, delay); // Self destroys after delay.
        }
    }
}
