using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuMugSpawn : MonoBehaviour
{
    [SerializeField]
    private Rigidbody mug;
    private Vector3 startPos;
    private Quaternion startRot;

    private void OnEnable() 
    {
        startPos = mug.position;
        startRot = mug.rotation;  
    }

    public void SpawnMug()
    {
        mug.position = startPos;
        mug.rotation = startRot;
        mug.velocity = Vector3.zero;
        mug.useGravity = false;
    }
}
