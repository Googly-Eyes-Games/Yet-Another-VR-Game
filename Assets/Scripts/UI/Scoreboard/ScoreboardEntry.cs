using System;

[Serializable]
public class ScoreboardEntry
{
    public string entryName;
    public int entryScore;

    public ScoreboardEntry(string entryName, int entryScore)
    {
        this.entryName = entryName;
        this.entryScore = entryScore;
    }
}
