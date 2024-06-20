using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMugSpawn : MonoBehaviour
{
    public event Action OnSpawn;
    
    [SerializeField]
    private Rigidbody mug;

    [SerializeField]
    private Vector3 startAngularVelocity = new (0f, 0.5f, 0f);
    

    private void OnEnable() 
    {
        SpawnMug();
    }

    public void SpawnMug()
    {
        mug.position = transform.position;
        mug.rotation = transform.rotation;
        mug.velocity = Vector3.zero;
        mug.angularVelocity = startAngularVelocity;
        mug.useGravity = false;

        MugComponent mugComponent = mug.GetComponent<MugComponent>();
        mugComponent.FillPercentage = 0f;
        
        OnSpawn?.Invoke();
    }
}
