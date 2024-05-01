using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour
{
    public GameObject cannonBall;
    public Transform barrel;
    public float force;

    public void Fire()
    {
        // 
        GameObject bullet = Instantiate(cannonBall, barrel.position, barrel.rotation); // Creates an instance of a cannonball.
        bullet.GetComponent<Rigidbody>().velocity = barrel.forward * force * Time.deltaTime; // Applies velocity to the cannonball.

        Destroy(bullet, 2.0f); // Destroys after 2 seconds.
    }
}
