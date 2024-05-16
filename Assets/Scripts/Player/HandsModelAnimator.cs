using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

[RequireComponent(typeof(ActionBasedController))]
public class HandsModelAnimator : MonoBehaviour
{
    private ActionBasedController controller;
    
    private Animator handModelAnimator;
    
    private static readonly int TriggerNameHash = Animator.StringToHash("Trigger");
    private static readonly int GripNameHash = Animator.StringToHash("Grip");

    void OnEnable()
    {
        controller = GetComponent<ActionBasedController>();
        handModelAnimator = controller.model.GetComponent<Animator>();
    }

    private void Update()
    {
        float trigger = controller.activateActionValue.action.ReadValue<float>();
        float grip = controller.selectActionValue.action.ReadValue<float>();
        
        handModelAnimator.SetFloat(TriggerNameHash, trigger);
        handModelAnimator.SetFloat(GripNameHash, grip);
    }
}
