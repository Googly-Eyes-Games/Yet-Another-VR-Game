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
    private AudioClip wantsBeerSFX;
    
    [Foldout("Clips")]
    [SerializeField]
    private AudioClip unsatisfiedSFX;
    
    [Foldout("Clips")]
    [SerializeField]
    private AudioClip satisfiedSFX;
    
    [Foldout("Clips")]
    [SerializeField]
    private AudioClip pickUpMugSFX;

    [SerializeField]
    [Range(0, 1)]
    private float goToBarPercentageToScreamAgain = 0.75f;

    private Client client;

    private AudioSource wantsBeerAS;
    private AudioSource unsatisfiedAS;
    private AudioSource satisfiedAS;
    private AudioSource pickUpMugAS;

    private bool screamedWantBeer = false;

    private void Awake()
    {
        client = GetComponent<Client>();
        client.OnClientStateChanged += HandleClientStateChanged;

        wantsBeerAS = CreateAudioSource(wantsBeerSFX);
        unsatisfiedAS = CreateAudioSource(unsatisfiedSFX);
        satisfiedAS = CreateAudioSource(satisfiedSFX);
        pickUpMugAS = CreateAudioSource(pickUpMugSFX);
    }

    private void Update()
    {
        if (client.GoToBarProgress > goToBarPercentageToScreamAgain && !screamedWantBeer)
        {
            screamedWantBeer = true;
            wantsBeerAS.Play();
        }
    }

    private void OnEnable()
    {
        screamedWantBeer = false;
        wantsBeerAS.Play();
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

