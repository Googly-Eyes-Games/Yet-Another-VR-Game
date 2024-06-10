using System;
using erulathra;
using NaughtyAttributes;
using UnityEngine;

public class GameplaySFX : SFXComponentBase
{
    [Foldout("Clips")]
    [SerializeField]
    private AudioClip heartLoseSFX;
    
    [Foldout("Clips")]
    [SerializeField]
    private AudioClip newLevelSFX;

    private AudioSource heartLoseAS;
    private AudioSource newLevelAS;

    private void Awake()
    {
        heartLoseAS = CreateAudioSource(heartLoseSFX);
        newLevelAS = CreateAudioSource(newLevelSFX);
        
        HeartsSubsystem heartsSubsystem = SceneSubsystemManager.GetSubsystem<HeartsSubsystem>();
        heartsSubsystem.OnHeartsNumberChanged += PlayHeartLose;

        LevelSubsystem levelSubsystem = SceneSubsystemManager.GetSubsystem<LevelSubsystem>();
        levelSubsystem.OnNextLevel += PlayNewLevelSound;
    }

    private void PlayNewLevelSound(int levelNumber)
    {
        newLevelAS.Play();
    }

    private void PlayHeartLose(int newHearts, int deltaHearts)
    {
        if (deltaHearts < 0)
        {
            heartLoseAS.Play();
        }
    }

    protected override AudioSource CreateAudioSource(AudioClip clip)
    {
        AudioSource audioSource = base.CreateAudioSource(clip);
        audioSource.spatialBlend = 0;

        return audioSource;
    }
}
