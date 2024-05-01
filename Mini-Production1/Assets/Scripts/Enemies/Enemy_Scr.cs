using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Scr : MonoBehaviour
{
    public GameObject Target;

    [SerializeField]
    private bool Active = true;

    [SerializeField]
    private GameObject TurretHead;
    [SerializeField]
    private GameObject Projectile;

    [SerializeField]
    [Tooltip("Affects Trajectory")]
    private float Speed = 1;
    [SerializeField]
    [Tooltip("How many seconds between shots")]
    private float FireRate = 10;

    private float CurrentDuration = 0;

    [SerializeField]
    [Tooltip("How far to shoot ahead of player")]
    public float zOffset = 10.0f;

    // Start is called before the first frame update

    // Update is called once per frame
    void Update()
    {
        if(Active)
        {
            if(Target == null)
            {
                Target = GameObject.Find("Boat");
            }
            if(Target == null) { return; }
           transform.LookAt(new Vector3(Target.transform.position.x, transform.position.y, Target.transform.position.z + zOffset));
            if(CurrentDuration >= FireRate)
            {
                Fire();
                CurrentDuration = 0;
            }
            CurrentDuration += Time.deltaTime;
        }
    }

    void Fire()
    {
        if (Projectile != null)
        {
            GameObject clone = Instantiate(Projectile, transform.position, Quaternion.identity);
            float dist = Vector3.Distance(transform.position, Target.transform.position);
            float time = dist / Speed;
            float height = (time * 9.81f);
            //Debug.Log("height: " + height + " time: " + time + " dist: " + dist);
            clone.GetComponent<Rigidbody>().velocity = (Speed * Vector3.Normalize(new Vector3(Target.transform.position.x, transform.position.y + height, Target.transform.position.z + zOffset) - transform.position));
            //clone.GetComponent<Rigidbody>().AddForce(Speed * Vector3.Normalize(new Vector3(Target.transform.position.x, transform.position.y + 5.0f, Target.transform.position.z + zOffset) - transform.position));
            // clone.GetComponent<Obstacle_Scr>().Spawner = gameObject;

        }
        else
        {
            Debug.Log("Forgot to attach projectile");
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (Target != null)
        {
            Gizmos.DrawLine(TurretHead.transform.position, new Vector3(Target.transform.position.x, Target.transform.position.y, Target.transform.position.z + zOffset));
        }
    }
}
