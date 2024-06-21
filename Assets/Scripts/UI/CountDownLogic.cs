using System;
using erulathra;
using TMPro;
using UnityEngine;

public class CountDownLogic : MonoBehaviour
{
    [SerializeField]
    private TMP_Text bigMessageText;

    [SerializeField]
    private AudioSource audioSource;
    
    private void Awake()
    {
        if (!enabled)
            return;

        GameplayTimeSubsystem gameplayTimeSubsystem = SceneSubsystemManager.GetSubsystem<GameplayTimeSubsystem>();
        if (gameplayTimeSubsystem)
        {
            gameplayTimeSubsystem.OnCountDownTick += UpdateCountDown;
            gameplayTimeSubsystem.OnCountDownEnd += HandleCountDownEnd;
        }
    }

    private void UpdateCountDown(int wholeSecondsRemain)
    {
        string nordicNumber = string.Empty;
        for (int i = 0; i < wholeSecondsRemain; i++)
        {
            nordicNumber += "I";
        }

        bigMessageText.enabled = true;
        bigMessageText.text = nordicNumber;

        audioSource.Play();
    }

    private void HandleCountDownEnd()
    {
        bigMessageText.enabled = false;
    }
}
