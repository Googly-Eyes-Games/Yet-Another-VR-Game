using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Audio;

public abstract class SFXComponentBase : MonoBehaviour
{
    [Foldout("General")]
    [SerializeField]
    private AudioMixerGroup SFXMixerGroup;
    
    protected virtual AudioSource CreateAudioSource(AudioClip clip)
    {
        AudioSource newAudioSource = gameObject.AddComponent<AudioSource>();
        newAudioSource.playOnAwake = false;
        newAudioSource.outputAudioMixerGroup = SFXMixerGroup;
        newAudioSource.clip = clip;
        newAudioSource.spatialBlend = 1f;

        return newAudioSource;
    }
}