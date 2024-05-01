using UnityEngine;
using UnityEngine.Events;


[RequireComponent(typeof(Collider))]
public class Interactable : MonoBehaviour
{
    public UnityEvent<Interactor> OnFocus;
    public UnityEvent<Interactor> OnLooseFocus;
    public UnityEvent<Interactor> OnStartInteract;
    public UnityEvent<Interactor> OnStopInteract;
}