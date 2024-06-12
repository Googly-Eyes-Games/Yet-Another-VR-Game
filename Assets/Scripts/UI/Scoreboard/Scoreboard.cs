using UnityEngine;

public class Scoreboard : MonoBehaviour
{
    [SerializeField] private int maxScoreboardEntries = 10;
    [SerializeField] private Transform scoreboardHolderTransform;
    [SerializeField] private GameObject scoreboardEntryObject;

    private void Start()
    {
        ScoreboardSaveData savedScores = SaveManager.LoadScores();
        savedScores.highScores.Sort((a, b) => b.entryScore.CompareTo(a.entryScore));
        
        UpdateUI(savedScores);
    }

    public void AddEntry(ScoreboardEntryData scoreboardEntryData)
    {
        ScoreboardSaveData savedScores = SaveManager.LoadScores();
        
        savedScores.highScores.Sort((a, b) => b.entryScore.CompareTo(a.entryScore));

        if (!savedScores.highScores.Exists(entry => entry.entryName == scoreboardEntryData.entryName && entry.entryScore == scoreboardEntryData.entryScore))
        {
            savedScores.highScores.Add(scoreboardEntryData);
            savedScores.highScores.Sort((a, b) => b.entryScore.CompareTo(a.entryScore));

            if (savedScores.highScores.Count > maxScoreboardEntries)
            {
                savedScores.highScores.RemoveAt(savedScores.highScores.Count - 1);
            }

            SaveManager.SaveScores(savedScores);
            UpdateUI(savedScores);
        }
    }

    private void UpdateUI(ScoreboardSaveData saveData)
    {
        foreach (Transform child in scoreboardHolderTransform)
        {
            Destroy(child.gameObject);
        }

        if (saveData == null)
            return;

        foreach (ScoreboardEntryData highScore in saveData.highScores)
        {
            Instantiate(scoreboardEntryObject, scoreboardHolderTransform).GetComponent<ScoreboardEntryUI>().Initialize(highScore);
        }
    }
}
