using UnityEngine;
using UnityEngine.Events;

public class ButtonInteractionAnimator : MonoBehaviour
{
    [SerializeField]
    private UnityEvent<bool> OnStateChanged;
    
    [SerializeField]
    private Transform buttonTransform;

    [SerializeField]
    private Vector3 defaultPosition;
    
    [SerializeField]
    private Vector3 hoverPosition;
    
    [SerializeField]
    private Vector3 pressedPosition;

    [SerializeField]
    private float animationSpeed;
    
    private bool isPressed;
    private bool isHovered;

    public void SetIsPressed(bool newValue)
    {
        isPressed = newValue;
        OnStateChanged?.Invoke(newValue);
    }
    
    public void SetIsHovered(bool newValue)
    {
        isHovered = newValue;
    }
    
    void Update()
    {
        Vector3 targetPosition = defaultPosition;

        if (isPressed)
        {
            targetPosition = pressedPosition;
        }
        else if (isHovered)
        {
            targetPosition = hoverPosition;
        }

        buttonTransform.localPosition = Vector3.Lerp(buttonTransform.localPosition, targetPosition, Time.deltaTime * animationSpeed);
    }
}
