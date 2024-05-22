using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class SewerTurn : MonoBehaviour
{
    private bool triggered = false;
    public float turnAngle;
    private void OnTriggerEnter(Collider other)
    {
        if (!triggered)
        {
            if (other.gameObject.name != "Boat")
                return;
            Debug.Log("SewerTurn Triggered:");
            triggered = true;

            GameManager gameManager = FindObjectOfType<GameManager>();
            if (gameManager == null) return;

            var boat = gameManager.boat;
            StartCoroutine(RotateAroundTarget(turnAngle, 2f));
        }
    }

    private IEnumerator RotateAroundTarget(float angle, float duration)
    {
        Vector3 axis = Vector3.up;
        float elapsed = 0f;
        float currentAngle = 0f;

        while (elapsed<duration)
        {
            float stepAngle = Mathf.Lerp(0, angle, elapsed / duration) - currentAngle;
            currentAngle += stepAngle;
            transform.parent.parent.RotateAround(transform.position, axis, stepAngle);

            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.parent.parent.RotateAround(transform.position, axis, angle - currentAngle);
    }
}
