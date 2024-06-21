using System;
using erulathra;
using UnityEngine;
using UnityTimer;

public class GameplayTimeSubsystem : SceneSubsystem
{
    public Action<int> OnCountDownTick;
    public Action OnCountDownEnd;
    
    private Timer countdownTimer;

    private int lastWholeSeconds = Int32.MaxValue;
    
    public override void Initialize()
    {
    }

    public override void PreAwake()
    {
        float countDownSeconds = GameplaySettings.Global.CountDownSeconds;
        countdownTimer = Timer.Register(countDownSeconds, CountDownEnd, CountDownTick, autoDestroyOwner: this);
    }

    private void CountDownEnd()
    {
        OnCountDownEnd?.Invoke();
    }

    private void CountDownTick(float seconds)
    {
        float countDownSeconds = GameplaySettings.Global.CountDownSeconds;
        int wholeSecondsRemain = Mathf.CeilToInt(countDownSeconds - seconds);

        if (lastWholeSeconds != wholeSecondsRemain)
        {
            OnCountDownTick?.Invoke(wholeSecondsRemain);
        }

        lastWholeSeconds = wholeSecondsRemain;
    }
}