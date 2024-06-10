using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class SceneTransition : MonoBehaviour
{
    [SerializeField]
    private Image image;

    private void Awake()
    {
        Color startColor = image.color;
        startColor.a = 0f;
        image.color = startColor;
    }

    public async Task FadeIn(float transitionTime)
    {
        Color startColor = image.color;
        startColor.a = 0f;

        image.color = startColor;
        image.DOFade(1f, transitionTime);
        
        await Task.Delay(Mathf.RoundToInt(transitionTime * 1000));
    }
    
    public async Task FadeOut(float transitionTime)
    {
        Color startColor = image.color;
        startColor.a = 1f;

        image.color = startColor;
        image.DOFade(0f, transitionTime);
        
        await Task.Delay(Mathf.RoundToInt(transitionTime * 1000));
    }
}
