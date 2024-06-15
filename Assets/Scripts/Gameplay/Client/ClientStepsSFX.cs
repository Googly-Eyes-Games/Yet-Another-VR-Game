using UnityEngine;

public class ClientStepsSFX : MonoBehaviour
{
    [SerializeField]
    private AudioSource stepsAudioSource;
    
    [SerializeField]
    private AudioClip[] stepsAudioClips;

    private int currentClipIndex = 0;
    
    public void PlayStepSound()
    {
        stepsAudioSource.clip = stepsAudioClips[currentClipIndex];
        stepsAudioSource.Play();
        
        currentClipIndex = (currentClipIndex + 1) % stepsAudioClips.Length;
    }
}
