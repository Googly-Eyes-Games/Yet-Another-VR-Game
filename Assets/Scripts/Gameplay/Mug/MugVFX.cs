using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(MugComponent))]
public class MugVFX : MonoBehaviour
{
    [SerializeField]
    private GameObject mugDestroyPrefab;

    [SerializeField]
    private float spawnAnimationDuration = 0.5f;
    
    [SerializeField]
    private VisualEffect foamVFX;
    
    [SerializeField]
    private Vector2 foamFillRange = new Vector2(0.9f, 1.0f);
    
    [SerializeField]
    private LiquidHandler liquidHandler;
    
    [SerializeField]
    private MugComponent mug;

    [SerializeField]
    private Transform meshTransform;

    private void OnEnable()
    {
        AnimateSpawn();
    }

    private void AnimateSpawn()
    {
        meshTransform.localScale = Vector3.zero;
        var spawnTween = meshTransform.DOScale(Vector3.one, spawnAnimationDuration);
        spawnTween.SetEase(Ease.OutBounce);
    }

    private void Awake()
    {
        mug.OnDestroy += HandleMugDestroy;
    }

    private void HandleMugDestroy(MugComponent brokenMug)
    {
        Instantiate(mugDestroyPrefab, brokenMug.transform.position, brokenMug.transform.rotation);
    }

    private void Update()
    {
        float fillPercentage = mug.FillPercentage;
        float targetFoamSize = Mathf.InverseLerp(foamFillRange.x, foamFillRange.y, fillPercentage);
        foamVFX.SetFloat(ShaderLookUp.FoamSize, targetFoamSize);
    }

    private static class ShaderLookUp
    {
        public static readonly int FoamSize = Shader.PropertyToID("FoamSize");
    }
}