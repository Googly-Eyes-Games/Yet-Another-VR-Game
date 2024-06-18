using System.Collections.Generic;
using System.IO;
using erulathra;
using UnityEngine;

public static class SaveManager
{
    private static string SavePath => Path.Combine(Application.persistentDataPath,"highScores.json");
    

    public static void SaveScores(ScoreboardSaveData scoreboardSaveData)
    {
        string json = JsonUtility.ToJson(scoreboardSaveData, true);
        File.WriteAllText(SavePath, json);
    }

    public static void AddEntry(ScoreboardEntry scoreboardEntry)
    {
        ScoreboardSaveData saveData = LoadScores();

        ScoreboardEntry found = saveData.highScores.Find(
            x => x.entryName == scoreboardEntry.entryName);

        if (found == null)
        {
            saveData.highScores.Add(scoreboardEntry);
        }
        else
        {
            found.entryScore = scoreboardEntry.entryScore;
        }

        SaveScores(saveData);
    }

    public static ScoreboardSaveData LoadScores()
    {
        if (!File.Exists(SavePath))
        {
            Debug.Log("File not found! Creating new save file.");
            return new ScoreboardSaveData();
        }

        string json = File.ReadAllText(SavePath);
        return JsonUtility.FromJson<ScoreboardSaveData>(json);
    }
}