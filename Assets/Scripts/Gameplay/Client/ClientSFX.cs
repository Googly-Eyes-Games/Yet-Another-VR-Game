using System;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(Client))]
public class ClientSFX : SFXComponentBase
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
    
    [Foldout("Clips")]
    [SerializeField]
    private AudioClip pickUpMugSFX;

    [Foldout("General")]
    [SerializeField]
    private AudioMixerGroup SFXMixerGroup;

    private Client client;

    private AudioSource spawnAS;
    private AudioSource unsatisfiedAS;
    private AudioSource satisfiedAS;
    private AudioSource pickUpMugAS;

    private void Awake()
    {
        client = GetComponent<Client>();
        client.OnClientStateChanged += HandleClientStateChanged;

        spawnAS = CreateAudioSource(spawnSFX);
        unsatisfiedAS = CreateAudioSource(unsatisfiedSFX);
        satisfiedAS = CreateAudioSource(satisfiedSFX);
        pickUpMugAS = CreateAudioSource(pickUpMugSFX);
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
            pickUpMugAS.Play();
        }
    }
}

