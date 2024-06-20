using System;
using erulathra;
using TMPro;
using UnityEngine;

public class PlayerHud : MonoBehaviour
{
    [SerializeField]
    private TMP_Text bigMessageText;

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

    private void Update()
    {
        // I just want to enable checkbox
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
    }

    private void HandleCountDownEnd()
    {
        bigMessageText.enabled = false;
    }
}
