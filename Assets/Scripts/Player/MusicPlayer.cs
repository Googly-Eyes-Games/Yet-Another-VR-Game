using erulathra;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicPlayer : MonoBehaviour
{
    private AudioSource musicAudioSource;
    private void Awake()
    {
        musicAudioSource = GetComponent<AudioSource>();

        GameplayTimeSubsystem gameplayTimeSubsystem = SceneSubsystemManager.GetSubsystem<GameplayTimeSubsystem>();
        if (gameplayTimeSubsystem)
        {
            gameplayTimeSubsystem.OnCountDownEnd += HandleCountDownEnd;
        }
    }

    private void HandleCountDownEnd()
    {
        musicAudioSource.Play();
        musicAudioSource.loop = true;
    }
}
