using System;

[Serializable]
public struct ScoreboardEntryData
{
    public string entryName;
    public int entryScore;

    public ScoreboardEntryData(string entryName, int entryScore)
    {
        this.entryName = entryName;
        this.entryScore = entryScore;
    }
}
