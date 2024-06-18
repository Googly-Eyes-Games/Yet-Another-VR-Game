using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Scoreboard : MonoBehaviour
{
    [SerializeField] private int maxScoreboardEntries = 10;
    [SerializeField] private Transform scoreboardHolderTransform;
    [SerializeField] private GameObject scoreboardEntryObject;

    private void Start()
    {
        int score = TransitionsSceneManger.Get().LastSceneScore;
        
        if (score < 0)
            return;
        
        ScoreboardEntry scoreboardEntry = new ScoreboardEntry(
            $"test{Random.Range(0, 32)}",
            score
        );
        
        SaveManager.AddEntry(scoreboardEntry);
        
        ScoreboardSaveData savedScores = SaveManager.LoadScores();
        savedScores.highScores.Sort((a, b) => b.entryScore.CompareTo(a.entryScore));
        
        UpdateUI(savedScores);
    }

    private void UpdateUI(ScoreboardSaveData saveData)
    {
        foreach (Transform child in scoreboardHolderTransform)
        {
            Destroy(child.gameObject);
        }

        if (saveData == null)
            return;

        List<ScoreboardEntry> entriesToShow =
            saveData.highScores
                .OrderBy(x => x.entryScore)
                .Take(maxScoreboardEntries).ToList();

        foreach (ScoreboardEntry entry in entriesToShow)
        {
            if (entry.entryScore > 0)
            {
                Instantiate(scoreboardEntryObject, scoreboardHolderTransform).GetComponent<ScoreboardEntryUI>().Initialize(entry);
            }
        }
    }
}
