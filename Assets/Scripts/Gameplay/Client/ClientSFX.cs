using System;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(Client))]
public class ClientSFX : MonoBehaviour
{
    [Foldout("Clips")]
    [SerializeField]
    private AudioClip spawnSFX;
    
    [Foldout("Clips")]
    [SerializeField]
    private AudioClip unsatisfiedSFX;
    
    [Foldout("Clips")]
    [SerializeField]
    private AudioClip satisfiedSFX;

    [Foldout("General")]
    [SerializeField]
    private AudioMixerGroup SFXMixerGroup;

    private Client client;

    private AudioSource spawnAS;
    private AudioSource unsatisfiedAS;
    private AudioSource satisfiedAS;

    private void Awake()
    {
        client = GetComponent<Client>();
        client.OnClientStateChanged += HandleClientStateChanged;

        spawnAS = CreateAudioSource(spawnSFX);
        unsatisfiedAS = CreateAudioSource(unsatisfiedSFX);
        satisfiedAS = CreateAudioSource(satisfiedSFX);
    }

    private AudioSource CreateAudioSource(AudioClip clip)
    {
        AudioSource newAudioSource = gameObject.AddComponent<AudioSource>();
        newAudioSource.playOnAwake = false;
        newAudioSource.outputAudioMixerGroup = SFXMixerGroup;
        newAudioSource.clip = clip;
        newAudioSource.spatialBlend = 1f;

        return newAudioSource;
    }

    private void OnEnable()
    {
        spawnAS.Play();
    }

    private void HandleClientStateChanged(ClientState newState)
    {
        if (newState == ClientState.Unsatisfied)
        {
            unsatisfiedAS.Play();
        }
        else if (newState == ClientState.DrinkingBeer)
        {
            satisfiedAS.Play();
        }
    }
}
