using System;
using UnityEngine;
using UnityEngine.InputSystem;


[RequireComponent(typeof(CharacterController), typeof(Interactor))]
public class CharacterDriver : MonoBehaviour
{
    CharacterController character;
    Interactor interactor;

    [SerializeField]
    BoatMovement boatAboard = null;

    private void Awake()
    {
        character = GetComponent<CharacterController>();
        interactor = GetComponent<Interactor>();
    }
    private void OnEnable()
    {
        interactor.enabled = true;

        InputInstance.Controls.Character.Movement.performed += SetMovementInput;
        InputInstance.Controls.Character.Movement.canceled += SetMovementInput;

        InputInstance.Controls.Character.Interact.performed += Interact;
        InputInstance.Controls.Character.Interact.canceled += StopInteract;

        InputInstance.Controls.Character.Dismount.performed += Dismount;


        InputInstance.Controls.Character.CmdFire.performed += CmdFire;
        InputInstance.Controls.Character.CmdFire.canceled += CmdFire;

        InputInstance.Controls.Character.TShift.performed += TShift;
    }

    private void OnDisable()
    {
        interactor.enabled = false;

        InputInstance.Controls.Character.Movement.performed -= SetMovementInput;
        InputInstance.Controls.Character.Movement.canceled -= SetMovementInput;

        InputInstance.Controls.Character.Interact.performed -= Interact;
        InputInstance.Controls.Character.Interact.canceled -= StopInteract;

        InputInstance.Controls.Character.Dismount.performed -= Dismount;


        InputInstance.Controls.Character.CmdFire.performed -= CmdFire;
        InputInstance.Controls.Character.CmdFire.canceled -= CmdFire;

        InputInstance.Controls.Character.TShift.performed -= TShift;
    }

    private void Dismount(InputAction.CallbackContext ctx)
    {
        character.TryDismount();
    }

    private void SetMovementInput(InputAction.CallbackContext ctx)
    {
        character.MoveInput = ctx.ReadValue<Vector2>();
    }

    private void Interact(InputAction.CallbackContext ctx)
    {
        interactor.TryStartInteract();
    }

    private void StopInteract(InputAction.CallbackContext ctx)
    {
        interactor.StopInteract();
    }

    private void CmdFire(InputAction.CallbackContext ctx)
    {
        boatAboard?.IssueCrewCommand(ctx.ReadValueAsButton() ? "Fire" : "Cease Fire");
    }

    private void TShift(InputAction.CallbackContext ctx)
    {
        boatAboard?.ActivateTemporalShift();
    }
}
