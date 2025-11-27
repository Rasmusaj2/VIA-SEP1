using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LeaderboardEntry
{
    public string playerName;
    public int score;

    // Default constructor for serialization to restore object from json
    public LeaderboardEntry() { }
    public LeaderboardEntry(string playerName, int score)
    {
        this.playerName = playerName;
        this.score = score;
    }
}

[Serializable]
public class Leaderboard
{
    public List<LeaderboardEntry> leaderboard = new List<LeaderboardEntry>();

    public Leaderboard() { }

    public void AddEntry(string playerName, int score)
    {
        leaderboard.Add(new LeaderboardEntry(playerName, score));
        SortLeaderboard();
    }
    public void SortLeaderboard()
    {
        leaderboard.Sort((a, b) => b.score.CompareTo(a.score));
    }
}