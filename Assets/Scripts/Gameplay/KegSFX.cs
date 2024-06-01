using System;
using UnityEngine;

[RequireComponent(typeof(KegComponent))]
public class KegSFX : MonoBehaviour
{
    private KegComponent keg;
    private AudioSource tappingAudioSource;

    private void Awake()
    {
        keg = GetComponent<KegComponent>();
        tappingAudioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        tappingAudioSource.volume = keg.NormalizedTippingSpeed;
    }
}