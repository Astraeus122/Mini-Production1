using UnityEngine;
using UnityEngine.Events;


public class SteeringWheel : MonoBehaviour
{
    [SerializeField]
    private Transform wheel = null;

    [SerializeField]
    private float wheelRotationLimit = 600f;

    [field: SerializeField]
    public float SteerSpeed { get; set; } = 2f;

    [field: SerializeField]
    public bool Recenter { get; set; } = false;

    [field: SerializeField]
    public bool Controlable { get; set; }

    [SerializeField]
    private UnityEvent<float> onSteeringChanged;


    public float CurrentSteerAngle { get; private set; }


    private void LateUpdate()
    {
        float input = Controlable ? InputInstance.Controls.Boat.Steering.ReadValue<float>() * Time.deltaTime : 0f;

        if (Mathf.Abs(input) > 0.001f)
            CurrentSteerAngle += input * SteerSpeed;
        else if (Recenter)
            CurrentSteerAngle = Mathf.Lerp(CurrentSteerAngle, 0f, Time.deltaTime * SteerSpeed);

        CurrentSteerAngle = Mathf.Clamp(CurrentSteerAngle, -1f, 1f);

        if (wheel)
            wheel.localRotation = Quaternion.Euler(0, 0, CurrentSteerAngle * wheelRotationLimit);

        onSteeringChanged?.Invoke(CurrentSteerAngle);
    }
}
