using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityTimer;

public class TableInput : MonoBehaviour
{
    [SerializeField]
    private Transform inputStartPoint;
    
    [SerializeField]
    private Transform inputEndPoint;

    [SerializeField]
    private float maxVelocityAngleOffset = 45f;
    
    [SerializeField]
    private float mugPositionCorrectionDuration = 0.5f;
    
    [SerializeField]
    private float forwardSpeedMultiplier = 1.2f;

    private readonly HashSet<MugComponent> mugsToMonitor = new();
    
    private void OnTriggerEnter(Collider other)
    {
        // Handle only mugs
        if (!other.CompareTag("Mug"))
            return;

        MugComponent mugComponent = other.GetComponent<MugComponent>();

        XRGrabInteractable grabInteractable = other.GetComponent<XRGrabInteractable>();
        if (grabInteractable.isSelected)
        {
            mugsToMonitor.Add(mugComponent);
            grabInteractable.lastSelectExited.AddListener(OnMugLastSelectExited);
            return;
        }
        
        ThrowMug(mugComponent);
    }

    private void OnTriggerExit(Collider other)
    {
        // Handle only mugs
        if (!other.CompareTag("Mug"))
            return;
        
        MugComponent mugComponent = other.GetComponent<MugComponent>();

        if (!mugsToMonitor.Contains(mugComponent)) 
            return;
        
        XRGrabInteractable grabInteractable = other.GetComponent<XRGrabInteractable>();
        grabInteractable.lastSelectExited.RemoveListener(OnMugLastSelectExited);
            
        mugsToMonitor.Remove(mugComponent);
    }

    void OnMugLastSelectExited(SelectExitEventArgs exitArgs)
    {
        XRGrabInteractable grabInteractable =  (XRGrabInteractable)exitArgs.interactableObject;
        MugComponent mugComponent = grabInteractable.GetComponent<MugComponent>();
        ThrowMug(mugComponent);
    }

    private void ThrowMug(MugComponent mugComponent)
    {
        Rigidbody mugRigidbody = mugComponent.GetComponent<Rigidbody>();
        Vector3 inputLineDir = (inputEndPoint.position - inputStartPoint.position).normalized;

        // Return when angle between velocity and input direction is too high
        if (Vector3.Angle(inputLineDir, mugRigidbody.velocity) > maxVelocityAngleOffset)
            return;
        
        // mugRigidbody.useGravity = false;
        
        CorrectAngularVelocity(mugRigidbody);
        CorrectVelocity(mugRigidbody, inputLineDir);
        CorrectMugPositionAndRotation(mugRigidbody, mugComponent);
    }

    private void CorrectAngularVelocity(Rigidbody mugRigidbody)
    {
        Vector3 mugAngularVelocity = mugRigidbody.angularVelocity;
        mugAngularVelocity.x = 0f;
        mugAngularVelocity.z = 0f;
        mugRigidbody.angularVelocity = mugAngularVelocity;
    }

    private void CorrectVelocity(Rigidbody mugRigidbody, Vector3 inputLineDir)
    {
        mugRigidbody.velocity = Vector3.Dot(inputLineDir, mugRigidbody.velocity) * inputLineDir * forwardSpeedMultiplier;
    }

    private void CorrectMugPositionAndRotation(Rigidbody mugRigidbody, MugComponent mugComponent)
    {
        Vector3 mugPosition = mugRigidbody.position;
        Vector3 mugLocalStartPosition = inputStartPoint.InverseTransformPoint(mugPosition);

        Vector3 mugRotation = mugRigidbody.rotation.eulerAngles;
        mugRotation.x = 0f;
        mugRotation.z = 0f;
        mugRigidbody.rotation = Quaternion.Euler(mugRotation);
        
        Timer.Register(
            mugPositionCorrectionDuration,
            () => { },
            timeSinceStart => TickMugPositionAndCorrection(timeSinceStart, mugRigidbody, mugLocalStartPosition),
            autoDestroyOwner: mugComponent);
    }

    private void TickMugPositionAndCorrection(
        float timeSinceStart, Rigidbody mugRigidbody,
        Vector3 mugLocalStartPosition)
    {
        Vector3 mugPosition = mugRigidbody.position;
        Vector3 mugLocalPosition = inputStartPoint.InverseTransformPoint(mugPosition);
        mugLocalPosition.x = Mathf.Lerp(mugLocalStartPosition.x, 0f, timeSinceStart / mugPositionCorrectionDuration);
        mugLocalPosition.y = Mathf.Lerp(mugLocalStartPosition.y, 0f, timeSinceStart / mugPositionCorrectionDuration);

        mugRigidbody.position = inputStartPoint.TransformPoint(mugLocalPosition);
    }
}
