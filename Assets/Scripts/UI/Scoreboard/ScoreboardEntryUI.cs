using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreboardEntryUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI entryNameText;
    [SerializeField] private TextMeshProUGUI entryScoreText;

    public void Initialize(ScoreboardEntryData scoreboardEntryData)
    {
        entryNameText.text = scoreboardEntryData.entryName;
        entryScoreText.text = scoreboardEntryData.entryScore.ToString();
    }
    
}
