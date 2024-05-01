using UnityEngine;
using UnityEngine.Events;


public class Mount : MonoBehaviour
{
    [field: SerializeField]
    public Transform MountPoint { get; private set; }

    [field: SerializeField]
    public float MountTime { get; private set; } = 0.3f;


    private CharacterController backingPassenger;
    public CharacterController Passenger
    {
        get
        {
            return backingPassenger;
        }
        set
        {
            if (!value)
            {
                if (backingPassenger == null) return;

                OnDismounted?.Invoke(backingPassenger);
                backingPassenger = null;
            }
            else
            {
                if (backingPassenger == value) return;

                backingPassenger = value;
                OnMounted?.Invoke(backingPassenger);
            }
        }
    }

    public UnityEvent<CharacterController> OnMounted;
    public UnityEvent<CharacterController> OnDismounted;


    public void HandleInteract(Interactor interactor)
    {
        if (interactor.TryGetComponent(out CharacterController character))
        {
            character.Mount(this);
        }
    }
}
