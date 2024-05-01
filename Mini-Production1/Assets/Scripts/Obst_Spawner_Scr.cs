using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obst_Spawner_Scr : MonoBehaviour
{
    public bool Active = true;
    //spawn zone makers
    public GameObject LowerLeftMarker;
    public GameObject TopRightMarker;

    //Prefab to spawn
    public GameObject[] SpawnObject;
    public int SpawnCapacity = 10;
    public int SpawnCount = 0;
    public float SpawnRate = 1; //per sec
    private float TimeAmount = 0;
    public bool SpawnLimit = true;
    // for spawned object
    public bool PassThoughParam = true;// pass direction,speed,lifetime to clone
    public Vector3 Movement_Dir;
    public float Movement_Speed = 1.0f;
    private float LifeTime = 10.0f;      // in seconds
    public float MinRot = 0;
    public float MaxRot = 0;

    public List<GameObject> SpawnedObjects;
    public Vector3 TerminationPoint = new Vector3(0,0,-50.0f);

    // Update is called once per frame
    void Spawn()
    {
        //Find random x and y in spawn zone
        float X = Random.Range(LowerLeftMarker.transform.position.x, TopRightMarker.transform.position.x);
        float Y = Random.Range(LowerLeftMarker.transform.position.y, TopRightMarker.transform.position.y);
        float Z = Random.Range(LowerLeftMarker.transform.position.z, TopRightMarker.transform.position.z);
        //Create clone
        GameObject clone = null;
        clone = Instantiate(SpawnObject[Random.Range(0, SpawnObject.Length)], new Vector3(X , Y, Z), Quaternion.identity,transform);
        if(clone != null)
        {
            Obstacle_Scr objScript = clone.GetComponent<Obstacle_Scr>(); 
            objScript.Spawner = gameObject;
            objScript.TerminationPoint = LowerLeftMarker.transform.position + TerminationPoint;
            SpawnedObjects.Add(clone);
            if (objScript != null)
            {
                if (PassThoughParam)
                {
                    objScript.SetDirection(Movement_Dir);
                    objScript.SetSpeed(Movement_Speed);
                    objScript.SetLifeTime(LifeTime);
                }

                if (objScript.FixedSpawn)
                {
                    clone.transform.position = objScript.SpawnPos;
                }

                if (objScript.FixedRotation)
                {
                    clone.transform.Rotate(objScript.SpawnRot);
                }
                else
                {
                    float rX = Random.Range(MinRot, MaxRot);
                    float rY = Random.Range(MinRot, MaxRot);
                    float rZ = Random.Range(MinRot, MaxRot);
                    clone.transform.Rotate(new Vector3(rX, rY, rZ));
                }
            }
        }
        TimeAmount = 0;
    }

    void Update()
    {
        if (!Active) { return; }
        // Saftey checks
        if(SpawnObject == null) { return; }
        if(LowerLeftMarker == null || TopRightMarker == null) { return; }

        if (SpawnLimit)
        {
            if (SpawnCount < SpawnCapacity)  // if limit hasn't been reached
            {
                if (TimeAmount >= SpawnRate) // if the spawn time has been reached
                {
                    Spawn();                // Spawn Object
                    SpawnCount++;
                }
            }
        }
        else
        {
            if (TimeAmount >= SpawnRate) // if the spawn time has been reached
            {
                Spawn();                // Spawn Object
            }
        }
        TimeAmount += Time.deltaTime;
    }

    public void ObjectDeath(GameObject _object_)
    {

        SpawnedObjects.Remove(_object_);
        SpawnCount--;
    }


    public void UpdateSpeed(float _speed_)
    {
        Movement_Speed = _speed_;
        foreach (GameObject obst in SpawnedObjects)
        {
            obst.GetComponent<Obstacle_Scr>().SetSpeed(_speed_);
        }
    }

    void OnDrawGizmos()// Just for editor use
    {
        if (LowerLeftMarker != null && TopRightMarker != null)
        {
            // Draws a blue line from this transform to the target
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(LowerLeftMarker.transform.position, new Vector3(TopRightMarker.transform.position.x, TopRightMarker.transform.position.y, LowerLeftMarker.transform.position.z));
            Gizmos.DrawLine(LowerLeftMarker.transform.position, new Vector3(LowerLeftMarker.transform.position.x, LowerLeftMarker.transform.position.y, TopRightMarker.transform.position.z));
            Gizmos.DrawLine(TopRightMarker.transform.position, new Vector3(TopRightMarker.transform.position.x, TopRightMarker.transform.position.y, LowerLeftMarker.transform.position.z));
            Gizmos.DrawLine(TopRightMarker.transform.position, new Vector3(LowerLeftMarker.transform.position.x, LowerLeftMarker.transform.position.y, TopRightMarker.transform.position.z));
            Gizmos.color = Color.red;
            Gizmos.DrawLine(new Vector3(TopRightMarker.transform.position.x, LowerLeftMarker.transform.position.y, LowerLeftMarker.transform.position.z + TerminationPoint.z), LowerLeftMarker.transform.position + TerminationPoint);
        }
    }
}
