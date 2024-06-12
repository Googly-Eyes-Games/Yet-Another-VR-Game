using System;
using UnityEngine;
using UnityEngine.Animations.Rigging;


public class ClientLookAtMug : MonoBehaviour
{
    [SerializeField]
    private Transform targetTransform;

    private Vector3 defaultLocalPosition;
    private Transform mugToAim;

    private void Awake()
    {
        defaultLocalPosition = targetTransform.localPosition;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (mugToAim)
            return;
        
        if (other.CompareTag("Mug"))
        {
            mugToAim = other.transform;
        }
    }

    private void OnEnable()
    {
        mugToAim = null;
        targetTransform.localPosition = defaultLocalPosition;
    }

    // Not the best solution but we are short on time
    private void Update()
    {
        if (mugToAim)
        {
            targetTransform.position = mugToAim.position;
        }
    }
}
