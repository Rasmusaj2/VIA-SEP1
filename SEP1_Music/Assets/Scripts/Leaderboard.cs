using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class LeaderboardEntry
{
    public string playerName;
    public int score;
    public LeaderboardEntry(string playerName, int score)
    {
        this.playerName = playerName;
        this.score = score;
    }
}

public class Leaderboard
{
    private List<LeaderboardEntry> leaderboard = new List<LeaderboardEntry>();

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