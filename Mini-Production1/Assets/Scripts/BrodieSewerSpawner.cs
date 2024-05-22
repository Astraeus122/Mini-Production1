using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrodieSewerSpawner : MonoBehaviour
{
    [SerializeField]
    private BrodieSewerSegment[] sewerPrefabsStraight;

    [SerializeField]
    private BrodieSewerSegment[] sewerPrefabsLeft;

    [SerializeField]
    private BrodieSewerSegment[] sewerPrefabsRight;

    [SerializeField]
    private float initialBoatSpeed = 2.5f;

    [SerializeField, Range(2, 10)]
    private int maxNumberOfSewer = 5;

    List<BrodieSewerSegment> spawnedSewer = new List<BrodieSewerSegment>();

    float currentRotationWalk;
    private float boatSpeed;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        boatSpeed = initialBoatSpeed;

        for (int i = 0; i < 5; i++)
        {
            SpawnSewer();
        }

        spawnedSewer[0].Armed = true;
    }

    private void Update()
    {
        if (gameManager != null)
        {
            AdjustBoatSpeed();
        }

        var root = spawnedSewer[0].transform;
        root.Translate(Time.deltaTime * -Vector3.forward * boatSpeed, Space.World);

        if (spawnedSewer[0].transform.localPosition.z < -spawnedSewer[0].DistToEnd)
        {
            SpawnSewer();
            Rebase();
        }
    }

    private void AdjustBoatSpeed()
    {
        float score = gameManager.Score;
        boatSpeed = initialBoatSpeed * (1 + (score / 2000)); // Adjust the divisor as needed to balance difficulty
    }

    public void OnDeath()
    {
        boatSpeed = 0f;
    }

    private void SpawnSewer()
    {
        // get next location, and follow worl rotation of last

        Transform spawnParent = null;
        BrodieSewerSegment lastSegment = null;

        if (spawnedSewer.Count >= 1)
            spawnParent = (lastSegment = spawnedSewer[spawnedSewer.Count - 1]).transform;
        else
            spawnParent = transform;

        BrodieSewerSegment prefab = SelectNextPipe();

        float dist1 = lastSegment ? lastSegment.DistToEnd : 0f;
        float dist2 = prefab.DistToStart;
        Vector3 pos = spawnParent.position + spawnParent.forward * (dist1 + dist2);

        Quaternion forwardRot = Quaternion.FromToRotation(Vector3.forward, spawnParent.forward);
        Quaternion additionalRot = prefab.transform.rotation;

        Debug.Log((forwardRot * Quaternion.Inverse(additionalRot)).eulerAngles.y);
        BrodieSewerSegment sewer = Instantiate(
            prefab,
            pos,
            forwardRot * additionalRot,
            spawnParent.transform
        );

        spawnedSewer.Add(sewer);
        currentRotationWalk += sewer.WalkAngle;
    }

    BrodieSewerSegment SelectNextPipe()
    {
        int arrIndex;
        if (spawnedSewer.Count == 0)
        {
            // first pipe, make it a easy one like a straight
            return sewerPrefabsStraight[Random.Range(0, sewerPrefabsStraight.Length)];
        }
        else if (currentRotationWalk < -179f)
        {
            // too much left, choose random from straight or right
            arrIndex = Random.Range(0, 2);
            if (arrIndex == 0)
                return sewerPrefabsStraight[Random.Range(0, sewerPrefabsStraight.Length)];
            else
                return sewerPrefabsRight[Random.Range(0, sewerPrefabsRight.Length)];
        }
        else if (currentRotationWalk > 179f)
        {
            //too much right, random from straight or left
            arrIndex = Random.Range(0, 2);
            if (arrIndex == 0)
                return sewerPrefabsStraight[Random.Range(0, sewerPrefabsStraight.Length)];
            else
                return sewerPrefabsLeft[Random.Range(0, sewerPrefabsLeft.Length)];
        }

        // nice and central, choose random from all three
        arrIndex = Random.Range(0, 3);
        if (arrIndex == 0)
            return sewerPrefabsStraight[Random.Range(0, sewerPrefabsStraight.Length)];
        else if (arrIndex == 1)
            return sewerPrefabsLeft[Random.Range(0, sewerPrefabsLeft.Length)];
        else
            return sewerPrefabsRight[Random.Range(0, sewerPrefabsRight.Length)];
    }

    void Rebase()
    {
        spawnedSewer[1].transform.SetParent(transform);
        Destroy(spawnedSewer[0].gameObject);
        spawnedSewer.RemoveAt(0);
        spawnedSewer[0].Armed = true;
    }
}
