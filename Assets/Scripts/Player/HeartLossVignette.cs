using System;
using DG.Tweening;
using erulathra;
using UnityEngine;

public class HeartLossVignette : MonoBehaviour
{
    [SerializeField]
    private float apertureSize = 0.7f;
    
    [SerializeField]
    private float featheringEffect = 0.5f;

    [SerializeField]
    private Color vignetteColor = Color.black;
    
    [SerializeField]
    private Color vignetteColorBlend = Color.black;

    [SerializeField]
    private float animationDuration = 0.5f;

    private Material material;
    
    private void Awake()
    {
        HeartsSubsystem heartsSubsystem = SceneSubsystemManager.GetSubsystem<HeartsSubsystem>();
        if (heartsSubsystem)
        {
            heartsSubsystem.OnHeartsNumberChanged += HandleHeartsNumberChanged;

            MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
            material = meshRenderer.material;
            material.SetFloat(ShaderLookUp.ApertureSize, 1f);
            material.SetFloat(ShaderLookUp.FeatheringEffect, featheringEffect);
            material.SetColor(ShaderLookUp.VignetteColor, vignetteColor);
            material.SetColor(ShaderLookUp.VignetteColorBlend, vignetteColorBlend);
        }
    }

    private void HandleHeartsNumberChanged(int heartsNumber, int deltaHearts)
    {
        if (deltaHearts > 0)
            return;
            
        Sequence animationSequence = DOTween.Sequence();
        
        var fadeIn = material.DOFloat(apertureSize, ShaderLookUp.ApertureSize, animationDuration * 0.5f);
        var fadeOut = material.DOFloat(1f, ShaderLookUp.ApertureSize, animationDuration * 0.5f);

        animationSequence.Append(fadeIn);
        animationSequence.Append(fadeOut);
    }

    private static class ShaderLookUp
    {
        public static readonly int ApertureSize = Shader.PropertyToID("_ApertureSize");
        public static readonly int FeatheringEffect = Shader.PropertyToID("_FeatheringEffect");
        public static readonly int VignetteColor = Shader.PropertyToID("_VignetteColor");
        public static readonly int VignetteColorBlend = Shader.PropertyToID("_VignetteColorBlend");
    }
}