using System;
using UnityEngine;

[RequireComponent(typeof(KegComponent))]
public class KegSFX : MonoBehaviour
{
    private KegComponent keg;
    private AudioSource tappingAudioSource;

    private float baseVolume;

    private void Awake()
    {
        keg = GetComponent<KegComponent>();
        tappingAudioSource = GetComponent<AudioSource>();
        
        baseVolume = tappingAudioSource.volume;
    }

    private void Update()
    {
        tappingAudioSource.volume = keg.NormalizedTippingSpeed * baseVolume;
    }
}