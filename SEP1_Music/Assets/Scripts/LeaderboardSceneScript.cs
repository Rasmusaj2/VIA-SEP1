using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LeaderboardSceneScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Leaderboard leaderboard;
    public TMP_Text leaderboardText;
    public Button returnButton;

    public string mainMenuSceneName = "MainMenu";
    public int leaderboardEntryCount = 20;

    void Start()
    {

        returnButton.onClick.AddListener(() => {
            SceneManager.LoadScene(mainMenuSceneName);
        });

        reloadLeaderboard();
        updateLeaderboardText();

    }
    
    void reloadLeaderboard()
    {
        leaderboard = JSONPersistence.LoadFromJSON<Leaderboard>(Path.Combine(JSONPersistence.Appdatapath, "leaderboard.json")) ?? new Leaderboard();
    }

    void updateLeaderboardText()
    {
        string newLeaderboardText = "FREE PLAY LEADERBOARD\n";
        int leaderboardDisplayCount = Mathf.Min(leaderboardEntryCount, leaderboard.leaderboard.Count);
        for (int i = 0; i < leaderboardDisplayCount; i++)
        {
            LeaderboardEntry entry = leaderboard.leaderboard[i];
            newLeaderboardText += $"{i + 1}. {entry.playerName} - {entry.score}\n";
        }
        
        leaderboardText.text = newLeaderboardText;

    }
}
