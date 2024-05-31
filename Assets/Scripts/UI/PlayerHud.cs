using erulathra;
using TMPro;
using UnityEngine;

public class PlayerHud : MonoBehaviour
{
    [SerializeField]
    private TMP_Text bigMessageText;

    private void Awake()
    {
        GameplayTimeSubsystem gameplayTimeSubsystem = SceneSubsystemManager.GetSubsystem<GameplayTimeSubsystem>();
        gameplayTimeSubsystem.OnCountDownTick += UpdateCountDown;
        gameplayTimeSubsystem.OnCountDownEnd += HandleCountDownEnd;
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
