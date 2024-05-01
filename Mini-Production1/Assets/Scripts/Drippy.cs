using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drippy : MonoBehaviour
{
    public GameObject splashyPrefab;

    ParticleSystem _particleSystem;
    public List<ParticleCollisionEvent> CollisionEvents;

    void Start()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        CollisionEvents = new List<ParticleCollisionEvent>();
    }

    void OnParticleCollision(GameObject other)
    {
        // create raindrop flip-book
        if (CollisionEvents == null)
            return;
        int num = _particleSystem.GetCollisionEvents(other, CollisionEvents);
        if (num > 0)
        {
            var go = Instantiate(splashyPrefab);
            go.transform.position = CollisionEvents[0].intersection;
            // other.gameObject.transform.position;
        }
    }
}
