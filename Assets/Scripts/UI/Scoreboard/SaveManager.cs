using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class SaveManager
{
    private static string SavePath => $"{Application.persistentDataPath}/highscores.json";

    public static void SaveScores(ScoreboardSaveData scoreboardSaveData)
    {
        string json = JsonUtility.ToJson(scoreboardSaveData, true);
        File.WriteAllText(SavePath, json);
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