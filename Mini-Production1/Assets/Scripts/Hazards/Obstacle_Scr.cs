using System.Collections;
using System.Collections.Generic;
using UnityEngine;




public class Obstacle_Scr : MonoBehaviour
{
    public Vector3 Movement_Dir;
    public float Movement_Speed = 1.0f;
    public float LifeTime = 300.0f;      // in seconds
    public float LifeDuration = 0.0f;


    //Initial spawn var
    public bool FixedSpawn     = false;
    public bool FixedRotation  = true;
    public Vector3 SpawnPos; // only use if Fixed Spawn true
    public Vector3 SpawnRot; // only use if Fixed Rot is true

    public GameObject Spawner;
    public Vector3 TerminationPoint;

    public bool IsDead = false;

    [SerializeField]
    public float Value = 1;

    // Update is called once per frame
    void Update()
    {
        // Needs movement - not sure if manager or self should move it
        transform.position += (Movement_Dir * Movement_Speed) * Time.deltaTime;
        LifeDuration += Time.deltaTime;
        if(TerminationPoint.z - transform.position.z > 0)
        {
            Die();
        }
    }

    public void SetDirection(Vector3 _Dir)
    {
        Movement_Dir = _Dir;
    }

    public void SetSpeed(float _spd)
    {
        Movement_Speed = _spd;
    }

    public void SetLifeTime(float _lft)
    {
        LifeTime = _lft;
    }
    
    public bool Die()
    {
        if (IsDead) return false;
        //play death anim
        if (Spawner != null)
        {
            Spawner.GetComponent<Obst_Spawner_Scr>().ObjectDeath(gameObject);
        }
        Destroy(gameObject);
        return IsDead = true;
    }
}
