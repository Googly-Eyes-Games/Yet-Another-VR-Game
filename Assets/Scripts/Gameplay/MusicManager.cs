using System;
using Config;
using UnityEngine;
using UnityEngine.Events;

public class MusicManager : MonoBehaviour
{
    [SerializeField]
    private MusicConfig musicConfig;

    [SerializeField]
    private MusicBeat[] musicBeats;
    
    [SerializeField]
    private AudioSource audioSource;
    
    public void StartPlaying()
    {
        audioSource.Play();
    }
    
    void Start()
    {
        audioSource.clip = musicConfig.audioClip;
    }

    void Update()
    {
        if (!audioSource.isPlaying)
            return;

        foreach (MusicBeat beat in musicBeats)
        {
            float sampledTime = audioSource.timeSamples / (musicConfig.audioClip.frequency * beat.GetBeatLength(musicConfig.bmp));
            beat.Process(sampledTime);
        }
    }

}

[Serializable]
public class MusicBeat
{
    [SerializeField]
    private float steps;
    
    [SerializeField]
    private UnityEvent trigger;

    private int lastTick;

    public float GetBeatLength(float bmp)
    {
        return 60f / (bmp * steps);
    }

    public void Process(float time)
    {
        int flooredTime = Mathf.FloorToInt(time);
        if (flooredTime != lastTick)
        {
            lastTick = flooredTime;
            trigger.Invoke();
        }
    }
}

