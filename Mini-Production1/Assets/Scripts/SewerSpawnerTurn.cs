using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class SewerSpawnerTurn : MonoBehaviour
{
    [SerializeField]
    private GameObject[] sewerPrefabs;
    public float boatSpeed = 2.5f;

    List<GameObject> spawnedSewer;
    float totalLength = 0f;
    
    [SerializeField]
    private float maxNumberOfSewer = 5;
    
    // used for despawning. kinda jank but does it's job.
    [SerializeField]
    private float maxSewerLength = 50f;

    [SerializeField]
    Vector3 facing;

    GameObject sewerEndPointGO;
    private float rotationWalk = 0f;

    void Awake()
    {
        spawnedSewer = new List<GameObject>();
    }

    public void OnDeath()
    {
        boatSpeed = 0f;
    }

    private void SpawnSewer()
    {
        /*
        // add increasing difficulty with weighted random later :)
        var sewer = Instantiate(sewerPrefabs[Random.Range(0, sewerPrefabs.Length)], transform);
        spawnedSewer.Add(sewer);

        sewer.transform.localRotation = Quaternion.Euler(0,90,0f);
        sewer.transform.position = new Vector3(0, transform.position.y, totalLength);

        // get mesh's length... to update totalLength after spawning a section
        MeshFilter meshFilter = sewer.GetComponent<MeshFilter>();
        if (meshFilter)
        {
            Bounds bounds = meshFilter.mesh.bounds;
            float lengthOfX = bounds.size.x;
            totalLength += lengthOfX;
        }
        
        water edge
        tunnel gradient going up 
        shadow missing 
        reverse depth fade on water.
        
        */
        var sewer = Instantiate(sewerPrefabs[Random.Range(0, sewerPrefabs.Length)], transform);
        spawnedSewer.Add(sewer);
        if (sewerEndPointGO)
            sewer.transform.position = sewerEndPointGO.transform.position;
        
        sewer.transform.localRotation = Quaternion.Euler(0,-90+rotationWalk,0f);
        //sewer.transform.DORotate(new Vector3(0f, -90f+rotationWalk, 0f), 1f, RotateMode.LocalAxisAdd);
        
        // record end point
        var endpointComponent = sewer.GetComponentInChildren<SewerEnd>();

        sewerEndPointGO = endpointComponent.gameObject;
        rotationWalk += endpointComponent.turnAngle;
        
        Debug.Log(sewerEndPointGO);
    }

    void Update()
    {
        /*if (Input.GetKeyUp(KeyCode.L))
        {
            SpawnSewer();
        }*/
        
        
        var deltaPos = boatSpeed * Time.deltaTime;
        totalLength -= deltaPos;

        // move all the sewers
        for (int i = spawnedSewer.Count - 1; i >= 0; i--)
        {
            var sewer = spawnedSewer[i];
            sewer.transform.position = new Vector3(sewer.transform.position.x,
                sewer.transform.position.y,
                sewer.transform.position.z - deltaPos);
            
            // despawn sewer
            if (sewer.transform.position.z < - maxSewerLength)
            {
                spawnedSewer.Remove(sewer);
                Destroy(sewer.gameObject);
            }
        }
        
        if (spawnedSewer.Count < maxNumberOfSewer)
        {
            SpawnSewer();
        }
        /*
        var deltaPos = boatSpeed * Time.deltaTime;
        totalLength -= deltaPos;

        // move all the sewers
        for (int i = spawnedSewer.Count - 1; i >= 0; i--)
        {
            var sewer = spawnedSewer[i];
            sewer.transform.position = new Vector3(sewer.transform.position.x,
                sewer.transform.position.y,
                sewer.transform.position.z - deltaPos);
            
            // despawn sewer
            if (sewer.transform.position.z < - maxSewerLength)
            {
                spawnedSewer.Remove(sewer);
                Destroy(sewer.gameObject);
            }
        }
        
        // spawn new sewer segments 
        if (spawnedSewer.Count < maxNumberOfSewer)
        {
            SpawnSewer();
        }
    */
    }
}
