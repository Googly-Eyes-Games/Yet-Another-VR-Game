using System;
using erulathra;
using NaughtyAttributes;
using UnityEngine;

public class GameplaySFX : SFXComponentBase
{
    [Foldout("Clips")]
    [SerializeField]
    private AudioClip heartLoseSFX;

    private AudioSource heartLoseAS;

    private void Awake()
    {
        heartLoseAS = CreateAudioSource(heartLoseSFX);
        
        HeartsSubsystem heartsSubsystem = SceneSubsystemManager.GetSubsystem<HeartsSubsystem>();
        heartsSubsystem.OnLivesNumberChanged += PlayHeartLose;
    }

    private void PlayHeartLose(int newHearts)
    {
        heartLoseAS.Play();
    }

    protected override AudioSource CreateAudioSource(AudioClip clip)
    {
        AudioSource audioSource = base.CreateAudioSource(clip);
        audioSource.spatialBlend = 0;

        return audioSource;
    }
}
