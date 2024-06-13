using System.Collections;
using System.Collections.Generic;
using erulathra;
using UnityEngine;

public class HealthBarHandler : MonoBehaviour
{
    private Material healthBarMaterial;

    private int maxHearts;
    
    void Start()
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        healthBarMaterial = meshRenderer.material;
        
        HeartsSubsystem heartsSubsystem = SceneSubsystemManager.GetSubsystem<HeartsSubsystem>();
        maxHearts = heartsSubsystem.MaxHearts;

        heartsSubsystem.OnHeartsNumberChanged += HandleHeartsNumberChanged;
    }

    private void HandleHeartsNumberChanged(int hearts, int delta)
    {
        float healthPercentage = (float)hearts / maxHearts;
        healthBarMaterial.SetFloat(ShaderLookUp.HealthPercentage, healthPercentage);
    }


    private static class ShaderLookUp
    {
        public static int HealthPercentage = Shader.PropertyToID("_Percentage");
    }
}
