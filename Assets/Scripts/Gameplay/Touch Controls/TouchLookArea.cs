using UnityEngine;
using UnityEngine.EventSystems;

public class TouchLookArea : MonoBehaviour,
    IPointerDownHandler,
    IDragHandler,
    IPointerUpHandler
{
    [Header("Sensitivity")]
    [SerializeField] private float sensitivity = 0.15f;

    [Header("Invert")]
    [SerializeField] private bool invertX = false;
    [SerializeField] private bool invertY = false;

    private Vector2 lastPosition;
    private bool dragging;

    public void OnPointerDown(PointerEventData eventData)
    {
        dragging = true;
        lastPosition = eventData.position;

        if (MobileInputManager.Instance != null)
            MobileInputManager.Instance.SetLookInput(Vector2.zero);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!dragging)
            return;

        Vector2 delta = eventData.position - lastPosition;

        lastPosition = eventData.position;

        delta *= sensitivity;

        if (invertX)
            delta.x *= -1f;

        if (invertY)
            delta.y *= -1f;

        if (MobileInputManager.Instance != null)
        {
            MobileInputManager.Instance.SetLookInput(delta);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        dragging = false;

        if (MobileInputManager.Instance != null)
        {
            MobileInputManager.Instance.SetLookInput(Vector2.zero);
        }
    }
}