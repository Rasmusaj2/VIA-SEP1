using UnityEngine;

public class TestScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log("Testing JSON Persistence");
        Leaderboard leaderboard = new Leaderboard();
        leaderboard.AddEntry("Alice", 1500);
        leaderboard.AddEntry("Bob", 1200);
        foreach (LeaderboardEntry entry in leaderboard.leaderboard)
        {
            Debug.Log($"Player: {entry.playerName}, Score: {entry.score}");
        }
        JSONPersistence.SaveLeaderboardToJson(leaderboard, "leaderboard.json");

        Leaderboard loaded_leaderboard = JSONPersistence.LoadFromJSON<Leaderboard>("leaderboard.json");
        Debug.Log("Loaded Leaderboard:");
        foreach (LeaderboardEntry entry in loaded_leaderboard.leaderboard)
        {
            Debug.Log($"Player: {entry.playerName}, Score: {entry.score}");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
