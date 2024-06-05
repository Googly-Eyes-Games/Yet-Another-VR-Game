using System;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(MugComponent))]
public class MugVFX : MonoBehaviour
{
    [SerializeField]
    private GameObject mugDestroyPrefab;

    [SerializeField]
    private float spawnAnimationDuration = 0.5f;

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
        MugComponent mug = GetComponent<MugComponent>();
        mug.OnDestroy += HandleMugDestroy;
    }

    private void HandleMugDestroy(MugComponent brokenMug)
    {
        Instantiate(mugDestroyPrefab, brokenMug.transform.position, brokenMug.transform.rotation);
    }
}