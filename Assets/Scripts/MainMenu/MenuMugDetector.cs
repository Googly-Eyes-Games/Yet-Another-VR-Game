using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MenuMugDetector : MonoBehaviour
{
    public UnityEvent OnMugFell;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Mug"))
        {
            OnMugFell?.Invoke();
        }
    }
}
