using System;
using UnityEngine;

public class MetronomeTest : MonoBehaviour
{
    [SerializeField]
    private MeshRenderer meshRenderer;
    
    private float beatTime;
    
    private void Update()
    {
        beatTime -= Time.deltaTime;
        beatTime = Mathf.Max(0, beatTime);
        
        Color color = Color.Lerp(Color.red, Color.green, beatTime);
        meshRenderer.sharedMaterial.SetColor("_EmissionColor", color);
    }

    public void Beat()
    {
        beatTime = 0.5f;
    }
}
