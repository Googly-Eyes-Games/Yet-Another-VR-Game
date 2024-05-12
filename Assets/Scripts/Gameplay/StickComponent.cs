using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StickComponent : MonoBehaviour
{
    public Vector3 Velocity { get; private set; } = Vector3.zero;

    private Vector3 lastTickPosition;

    private void Start()
    {
        lastTickPosition = transform.position;
    }

    void Update()
    {
        Velocity = (transform.position - lastTickPosition) / Time.deltaTime;
        lastTickPosition = transform.position;
        
        Debug.DrawRay(transform.position, Velocity);
    }
}
