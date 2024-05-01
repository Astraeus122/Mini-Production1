using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Scr : MonoBehaviour
{
    [SerializeField]
    public int depth = -1;
    bool inwater = false;
    // Start is called before the first frame update
   

    // Update is called once per frame
    void Update()
    {

        if(transform.position.y <= depth && !inwater)
        {
            gameObject.GetComponent<Rigidbody>().velocity = Vector3.zero;
            gameObject.GetComponent<Rigidbody>().useGravity = false;
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
            gameObject.GetComponent<Obstacle_Scr>().Movement_Dir = new Vector3(0.0f,0.0f,-1.0f);
            gameObject.GetComponent<Obstacle_Scr>().Movement_Speed = GameObject.Find("EnemySpawner").GetComponent<Obst_Spawner_Scr>().Movement_Speed;
        }
        
    }
}
