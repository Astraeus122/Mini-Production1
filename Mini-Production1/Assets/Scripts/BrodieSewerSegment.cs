using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrodieSewerSegment : MonoBehaviour
{
    [SerializeField]
    private float turnThreshold = 0f;

    [SerializeField]
    private float turnDuration = 2f;

    [field: SerializeField]
    public float DistToStart { get; private set; } = 0f;

    [field: SerializeField]
    public float DistToEnd { get; private set; } = 0f;

    [field: SerializeField, Tooltip("Used by spawner to add up selected segments, ensuring total angle doesnt exceed 180.")]
    public float WalkAngle { get; private set; } = 0f;


    public bool Armed { get; set; }

    private bool triggered;


    private void Awake()
    {
        triggered = false;
    }

    private void Update()
    {
        if (!Armed || triggered) return;
        

        if (transform.position.z < turnThreshold)
        {
            Debug.Log("SewerTurn Triggered:");
            triggered = true;

            //StartCoroutine(TurnSegement());
        }
    }

    private IEnumerator TurnSegement()
    {
        float elapsed = 0f;

        Quaternion original = transform.rotation;

        while (elapsed < 1)
        {
            transform.rotation = Quaternion.Slerp(original, Quaternion.identity, elapsed);
            elapsed += Time.deltaTime / turnDuration;
            yield return null;
        }
        transform.rotation = Quaternion.identity;
    }
}
