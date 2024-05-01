using UnityEngine;

public class EnemyAITest : MonoBehaviour
{
    public Transform playerBoat;
    public float detectionRadius = 50f;
    public GameObject cannon;
    private Vector3 spawnPoint;
    public float patrolRadius = 30f;
    public float rotationDampening = 0.8f; // Control rotation speed for larger ships
    public float rotationSpeed = 5f; // Speed at which the ship rotates towards the player
    public bool anchorDropped = false;

    private BoatMovement boatMovement;
    private Shoot shootScript;
    private bool isAIEnabled = true;

    void Start()
    {
        spawnPoint = transform.position;
        boatMovement = GetComponent<BoatMovement>();
        shootScript = cannon.GetComponent<Shoot>();

        if (boatMovement == null)
            Debug.LogError("BoatMovement component not found on " + gameObject.name);
        if (shootScript == null)
            Debug.LogError("Shoot script or cannon GameObject not assigned on " + gameObject.name);
        if (playerBoat == null)
            Debug.LogError("Player boat not assigned in the inspector.");
    }

    void Update()
    {
        if (!isAIEnabled) return;

        float distanceToPlayer = Vector3.Distance(playerBoat.position, transform.position);

        if (distanceToPlayer <= detectionRadius)
        {
            RotateTowards(playerBoat.position);

            if (IsAlignedWithPlayer())
            {
                if (!anchorDropped)
                {
                    boatMovement.ToggleAnchor();
                    anchorDropped = true;
                }
                shootScript.Fire();
            }
            else if (anchorDropped)
            {
                boatMovement.ToggleAnchor();
                anchorDropped = false;
            }
        }
        else
        {
            if (anchorDropped)
            {
                boatMovement.ToggleAnchor();
                anchorDropped = false;
            }
            Patrol();
        }
    }

    void RotateTowards(Vector3 target)
    {
        Vector3 directionToTarget = (target - transform.position).normalized;
        float targetAngle = Mathf.Atan2(directionToTarget.x, directionToTarget.z) * Mathf.Rad2Deg;

        // Adjust targetAngle by subtracting a constant offset
        float rotationOffset = 90; // Adjust this value based on your observation
        targetAngle -= rotationOffset;

        Quaternion targetRotation = Quaternion.Euler(0, targetAngle, 0);
        Debug.Log($"Adjusted Target Angle: {targetAngle}, Current Y Rotation: {transform.rotation.eulerAngles.y}");

        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    bool IsAlignedWithPlayer()
    {
        Vector3 directionToPlayer = playerBoat.position - transform.position;
        Vector3 cannonDirection = cannon.transform.forward; // Adjust this if the cannon's forward isn't the boat's forward.
        float angleToPlayer = Vector3.Angle(cannonDirection, directionToPlayer);
        return angleToPlayer < 10; // Fire if angle to player is less than 10 degrees
    }

    void Patrol()
    {
        Vector3 randomDirection = Random.insideUnitSphere * patrolRadius;
        randomDirection += spawnPoint;
        transform.position = Vector3.MoveTowards(transform.position, randomDirection, boatMovement.speed * Time.deltaTime);
    }
}
