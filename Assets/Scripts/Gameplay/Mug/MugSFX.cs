using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(MugComponent))]
public class MugSFX : MonoBehaviour
{
    
    [Foldout("Clips")]
    [SerializeField]
    private AudioClip slideSFX;
    
    [Foldout("Clips")]
    [SerializeField]
    private AudioClip pickUpSFX;
    
    [Foldout("Clips")]
    [SerializeField]
    private AudioClip putSFX;
    
    [Foldout("General")]
    [SerializeField]
    private AudioMixerGroup SFXMixerGroup;

    private AudioSource slideAC;
    private AudioSource pickUpAC;
    private AudioSource putAC;

    private MugComponent mug;

    private void Awake()
    {
        slideAC = CreateAudioSource(slideSFX);
        slideAC.loop = true;
        
        pickUpAC = CreateAudioSource(pickUpSFX);
        putAC = CreateAudioSource(putSFX);

        mug = GetComponent<MugComponent>();
        mug.OnSlidingStateChanged += HandleSlidingStateChanged;
        mug.OnPickedUpByClient += HandlePickingByClient;
    }
    
    private void HandleSlidingStateChanged(bool slidingChanged)
    {
        if (slidingChanged)
        {
            slideAC.Play();
            putAC.Play();
        }
        else
        {
            slideAC.Stop();
        }
    }

    private void HandlePickingByClient()
    {
        pickUpAC.Play();
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
}
