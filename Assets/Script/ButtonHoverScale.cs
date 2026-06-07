using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHoverScale : MonoBehaviour
{
    [SerializeField] private float targetscale = 1.1f;
    [SerializeField] private float smootSpeed = 10f;

    private Vector3 originalScale;
    private Vector3 target;

    void Start()
    {
        originalScale = transform.localScale;
        target = originalScale;
    }

    void Update()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, target, Time.deltaTime*smootSpeed);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        target = originalScale * targetscale;
        Debug.Log("membesar");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        target = originalScale;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Di Klikk!!!");
    }
}
