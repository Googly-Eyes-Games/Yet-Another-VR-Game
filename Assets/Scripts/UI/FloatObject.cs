using System;
using UnityEngine;

public class FloatObject : MonoBehaviour
{
    [SerializeField]
    public float speed;

    [SerializeField]
    public float amplitude;

    private Vector3 startPosition;

    public void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        Vector3 newPosition = transform.position;
        newPosition.y = startPosition.y + Mathf.Sin(Time.time * speed) * amplitude;

        transform.position = newPosition;
    }
}