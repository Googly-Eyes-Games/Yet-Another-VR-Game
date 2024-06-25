using System;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameOverEntryInput : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI scoreText;

    [SerializeField]
    private TMP_InputField inputField;

    [SerializeField]
    private int maxCharacters = 16;
    
    private int score;
    private string nick = "GUEST";
    
    private TouchScreenKeyboard overlayKeyboard;
    
    void Start()
    {
        score = TransitionsSceneManger.Get().LastSceneScore;
        
        if (score < 0)
            return;

        scoreText.text = score.ToString();
        
        ShowKeyboard();
    }

    private void Update()
    {
        if (overlayKeyboard != null)
        {
            // Sanitize input
            Regex rgx = new Regex("[^a-zA-Z0-9 -]");
            overlayKeyboard.text = rgx.Replace(overlayKeyboard.text, "");
            
            inputField.text = overlayKeyboard.text;
            nick = overlayKeyboard.text;
        }
    }

    public void ShowKeyboard()
    {
        overlayKeyboard = TouchScreenKeyboard.Open(nick,  TouchScreenKeyboardType.Default);
        overlayKeyboard.characterLimit = maxCharacters;
    }
    
    public void Next()
    {
        if (nick != null)
        {
            nick = nick.ToUpperInvariant();
        
            ScoreboardEntry scoreboardEntry = new ScoreboardEntry(
                nick,
                score
            );
            
            SaveManager.AddEntry(scoreboardEntry);
        }
    }
}
