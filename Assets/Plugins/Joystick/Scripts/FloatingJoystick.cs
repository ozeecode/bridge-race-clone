using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Layouts;
using UnityEngine.InputSystem.OnScreen;

public class FloatingJoystick : OnScreenControl, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [InputControl(layout = "Vector2")]
    [SerializeField]
    private string m_ControlPath;

    private Vector2 m_PointerDownPos;
    private float movementRange;


    [SerializeField] RectTransform Outline;
    [SerializeField] RectTransform Handle;
    [SerializeField] CanvasGroup canvasGroup;
    [SerializeField] bool alwaysShow = false;


    private void Awake()
    {
        movementRange = Outline.sizeDelta.x / 2f;
        canvasGroup.alpha = alwaysShow ? 1 : 0;
    }
    protected override string controlPathInternal
    {
        get => m_ControlPath;
        set => m_ControlPath = value;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData == null)
            throw new System.ArgumentNullException(nameof(eventData));

        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponentInParent<RectTransform>(), eventData.position, eventData.pressEventCamera, out m_PointerDownPos);

        Outline.position = eventData.position;
        Handle.anchoredPosition = Vector2.zero;
        canvasGroup.alpha = 1;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData == null)
            throw new System.ArgumentNullException(nameof(eventData));

        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponentInParent<RectTransform>(), eventData.position, eventData.pressEventCamera, out var position);
        var delta = position - m_PointerDownPos;

        delta = Vector2.ClampMagnitude(delta, movementRange);
        Handle.anchoredPosition = delta;

        var newPos = new Vector2(delta.x / movementRange, delta.y / movementRange);
        SendValueToControl(newPos);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Handle.anchoredPosition = Vector2.zero;
        SendValueToControl(Vector2.zero);
        canvasGroup.alpha = alwaysShow ? 1 : 0;
    }



}
