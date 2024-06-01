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
        // TODO: Remove magic numbers
        if (keg.NormalizedTippingSpeed < 0.1f)
        {
            tappingAudioSource.Stop();
        }
        else
        {
            tappingAudioSource.Play();
        }

        tappingAudioSource.volume = keg.NormalizedTippingSpeed;
    }
}