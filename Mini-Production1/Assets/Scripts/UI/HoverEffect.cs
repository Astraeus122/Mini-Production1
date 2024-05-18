using UnityEngine;
using UnityEngine.EventSystems;

public class HoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Vector3 hoverScale = new Vector3(1.1f, 1.1f, 1.1f);
    public float animationTime = 0.2f;

    private Vector3 originalScale;
    private bool isHovering = false;

    void Start()
    {
        originalScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        isHovering = true;
        StopAllCoroutines();
        StartCoroutine(ScaleOverTime(hoverScale));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        isHovering = false;
        StopAllCoroutines();
        StartCoroutine(ScaleOverTime(originalScale));
    }

    System.Collections.IEnumerator ScaleOverTime(Vector3 targetScale)
    {
        float currentTime = 0.0f;
        Vector3 startScale = transform.localScale;

        while (currentTime < animationTime)
        {
            currentTime += Time.unscaledDeltaTime;  // Using unscaledDeltaTime here
            transform.localScale = Vector3.Lerp(startScale, targetScale, currentTime / animationTime);
            yield return null;
        }

        transform.localScale = targetScale; // Ensure target scale is set perfectly at the end
    }
}
