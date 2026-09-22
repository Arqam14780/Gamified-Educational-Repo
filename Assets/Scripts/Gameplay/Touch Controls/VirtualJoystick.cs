using UnityEngine;
using UnityEngine.EventSystems;

public class VirtualJoystick : MonoBehaviour,
    IPointerDownHandler,
    IDragHandler,
    IPointerUpHandler
{
    [Header("References")]
    [SerializeField] private RectTransform joystickArea;
    [SerializeField] private RectTransform handle;

    [Header("Settings")]
    [SerializeField] private float movementRange = 100f;

    private Vector2 input;

    public Vector2 Input => input;

    private void Awake()
    {
        if (joystickArea == null)
            joystickArea = transform as RectTransform;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        UpdateJoystick(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        UpdateJoystick(eventData);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        input = Vector2.zero;

        if (handle != null)
            handle.anchoredPosition = Vector2.zero;

        SendInput();
    }

    private void UpdateJoystick(PointerEventData eventData)
    {
        if (joystickArea == null)
            return;

        Vector2 localPoint;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            joystickArea,
            eventData.position,
            eventData.pressEventCamera,
            out localPoint
        );

        input = localPoint / movementRange;

        input = Vector2.ClampMagnitude(input, 1f);

        if (handle != null)
        {
            handle.anchoredPosition = input * movementRange;
        }

        SendInput();
    }

    private void SendInput()
    {
        if (MobileInputManager.Instance != null)
        {
            MobileInputManager.Instance.SetMoveInput(input);
        }
    }
}