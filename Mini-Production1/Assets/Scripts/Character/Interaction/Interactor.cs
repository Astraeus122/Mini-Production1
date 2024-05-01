using UnityEngine;
using UnityEngine.UI;

public class Interactor : MonoBehaviour
{
    [field: SerializeField]
    public Transform Origin { get; private set; } = null;

    [SerializeField]
    LayerMask interactablesLayer = new LayerMask();

    [SerializeField]
    private GameObject interactPrompt = null;

    [SerializeField]
    private Image interactPromptGraphic = null;

    [SerializeField]
    private Color defaultColor = Color.white;

    [SerializeField]
    private Color interactingColor = Color.white;

    [SerializeField]
    private float interactScaleFactor = 1.1f;

    public Interactable Focus { get; private set; } = null;
    public bool Interacting { get; private set; }


    private void OnDisable()
    {
        if (!Focus) return;

        StopInteract();

        Focus.OnLooseFocus?.Invoke(this);

        Focus = null;

        if (interactPrompt)
            interactPrompt.SetActive(false);
    }

    private void Update()
    {
        var (target, focusInRange) = ClosestInRange();

        if (Focus)
        {
            if (!focusInRange || (!Interacting && Focus != target))
            {
                StopInteract();

                Focus.OnLooseFocus?.Invoke(this);

                Focus = null;
            }
        }
        // no risk for Focus to just be set null (above) and back to itself (below), since the above nullification only can happen if the Focus != target this frame
        // (!focusInRange could allow Focus == target == null, but the overall if ensure Focus not null)
        if (!Focus && target)
        {
            Focus = target;
            Focus.OnFocus?.Invoke(this);
        }

        if (!interactPrompt) return;

        interactPrompt.SetActive(Focus);
        if (Focus)
        {
            interactPrompt.transform.position = Focus.transform.position;
            interactPromptGraphic.color = Interacting ? interactingColor : defaultColor;
            interactPromptGraphic.transform.localScale = Vector3.one * (Interacting ? interactScaleFactor : 1f);
        }
    }

    public bool TryStartInteract()
    {
        if (!Focus) return false;

        Focus.OnStartInteract?.Invoke(this);

        Interacting = true;

        return true;
    }

    public bool StopInteract()
    {
        if (!Focus || !Interacting) return false;

        Focus.OnStopInteract?.Invoke(this);

        Interacting = false;

        return true;
    }

    private (Interactable, bool) ClosestInRange()
    {
        Collider[] triggerInteractables = Physics.OverlapSphere(Origin.position, 0f, interactablesLayer);

        (Interactable interactable, float sqrDistance) closest = (null, Mathf.Infinity);

        bool focusInRange = false;

        foreach (Collider collider in triggerInteractables)
        {
            if (!collider.isTrigger) continue;

            if (!collider.gameObject.TryGetComponent(out Interactable interactable)) continue;

            if (!interactable.isActiveAndEnabled) continue;

            if (interactable == Focus) focusInRange = true;

            float sqrDist = (interactable.transform.position - Origin.position).sqrMagnitude;

            if (sqrDist <= closest.sqrDistance)
            {
                closest.sqrDistance = sqrDist;
                closest.interactable = interactable;
            }
            else continue;
        }
            return (closest.interactable, focusInRange);
    }

    private void OnDrawGizmos()
    {
        if (Focus)
        {
            Gizmos.color = Interacting ? Color.red : Color.yellow;
            Gizmos.DrawCube(Focus.transform.position, Vector3.one * (Interacting ? 0.3f : 0.1f));
        }

        if (Origin)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawCube(Origin.position, Vector3.one * 0.1f);
        }
    }
}