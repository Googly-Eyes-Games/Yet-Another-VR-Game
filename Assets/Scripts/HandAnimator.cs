using System;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class HandAnimator : MonoBehaviour
{
    [SerializeField]
    private InputActionProperty triggerAnimationProperty;
    
    [SerializeField]
    private InputActionProperty gripAnimationProperty;

    private Animator handAnimator;

    private static readonly int Grip = Animator.StringToHash("Grip");
    private static readonly int Trigger = Animator.StringToHash("Trigger");

    private void Awake()
    {
        handAnimator = GetComponent<Animator>();
    }

    void LateUpdate()
    {
        float triggerValue = triggerAnimationProperty.action.ReadValue<float>();
        handAnimator.SetFloat(Trigger, triggerValue);
        
        float gripValue = gripAnimationProperty.action.ReadValue<float>();
        handAnimator.SetFloat(Grip, gripValue);
    }
}
