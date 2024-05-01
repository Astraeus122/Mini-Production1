using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomRotate : MonoBehaviour
{
    public GameObject despawnVFX;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.transform.eulerAngles = new Vector3(Random.Range(-90f,90f), Random.Range(-90f, 90f), Random.Range(-90f, 90f));
    }


    void OnDestroy()
    {
        /*
        if (despawnVFX)
        {
            var go = Instantiate(despawnVFX);
            go.transform.position = gameObject.transform.position;
        }
        */
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
