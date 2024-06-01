using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(MugComponent))]
public class MugSFX : SFXComponentBase
{
    
    [Foldout("Clips")]
    [SerializeField]
    private AudioClip slideSFX;
    
    [Foldout("Clips")]
    [SerializeField]
    private AudioClip putSFX;
    
    private AudioSource slideAC;
    private AudioSource putAC;

    private MugComponent mug;

    private void Awake()
    {
        slideAC = CreateAudioSource(slideSFX);
        slideAC.loop = true;
        
        putAC = CreateAudioSource(putSFX);

        mug = GetComponent<MugComponent>();
        mug.OnSlidingStateChanged += HandleSlidingStateChanged;
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
}
