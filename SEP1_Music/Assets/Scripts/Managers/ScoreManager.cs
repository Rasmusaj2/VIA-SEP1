using System.IO;
using Math = System.Math;
using UnityEngine;
using UnityEngine.UIElements;

public class ScoreManager : MonoBehaviour
{
    public GameObject scoreLine;
    public UIDocument playSceneUIDoc;
    public int combo = 0;
    public string scoreText;
    public int scoreNumber = 0;
    public float accuracy;
    public string accuracyText = "100%";
    private float maxScore;
    public string hitText;

    public const double SCORE_PERFECT_THRESHOLD = 0.02;
    public const double SCORE_GREAT_THRESHOLD = 0.04;
    public const double SCORE_GOOD_THRESHOLD = 0.08;
    public const double SCORE_OK_THRESHOLD = 0.12;


    private static float scoreLinePositionY;

    public void Start()
    {
        playSceneUIDoc = gameObject.GetComponent<UIDocument>();

        //De forskellige UI elementer der viser score databindes til scoremanageren.
        playSceneUIDoc.rootVisualElement.Q<Label>("scoretxt").dataSource = this;
        playSceneUIDoc.rootVisualElement.Q<Label>("combotxt").dataSource = this;
        playSceneUIDoc.rootVisualElement.Q<Label>("accuracytxt").dataSource = this;
        playSceneUIDoc.rootVisualElement.Q<Label>("hittext").dataSource = this;
        playSceneUIDoc.rootVisualElement.Q<Label>("lbentryscoretxt").dataSource = this;

        scoreText = $"{scoreNumber:D6}";

        scoreLinePositionY = scoreLine.transform.position.y;
    }

    public bool EvaluateHit(double noteTime, double hitTime)
    {
        bool hit = true;
        double timeDifference = Math.Abs(hitTime - noteTime);
        //Score uddeles baseret på forskellen på positionen af noden og scorelinen. Derudover ændres UI for at give feedback
        switch (timeDifference)
        {
            case < SCORE_PERFECT_THRESHOLD:
                scoreNumber += 300;
                hitText = "Perfect!";
                combo++;
                break;
            case < SCORE_GREAT_THRESHOLD:
                scoreNumber += 200;
                hitText = "Great!";
                combo++;
                break;
            case < SCORE_GOOD_THRESHOLD:
                scoreNumber += 100;
                hitText = "Good";
                combo++;
                break;
            case < SCORE_OK_THRESHOLD:
                scoreNumber += 50;
                hitText = "Ok";
                combo++;
                break;
            default:
                hit = false;
                hitText = "Missed";
                combo = 0;
                break;

        }

        scoreText = $"{scoreNumber:D6}";
        CalculateAccuracy();

        return hit;
    }

    public void ResetScore()
    {
        this.scoreNumber = 0;
        this.combo = 0;
        this.scoreText = "000000";
    }

    void CalculateAccuracy()
    {
        //Udregner accuracy display i UI ud fra de point man kunne have haft.
        maxScore += 300;
        accuracy = scoreNumber / maxScore * 100;
        accuracyText = $"{accuracy:F2}%";
    }

    public void SaveScore(string playerName)
    {
        Leaderboard leaderboard;

        if (CrossSceneManager.SelectedMap != null)
        {
            leaderboard = CrossSceneManager.SelectedMap.leaderboard;
            leaderboard.AddEntry(playerName, scoreNumber);
            CrossSceneManager.SelectedMap.SaveLeaderboard();
        }
        else
        {
            string jsonString = Path.Combine(JSONPersistence.Appdatapath, "leaderboard.json");
            leaderboard = JSONPersistence.LoadFromJSON<Leaderboard>(jsonString) ?? new Leaderboard();
            leaderboard.AddEntry(playerName, scoreNumber);
            JSONPersistence.SaveLeaderboardToJson(leaderboard, jsonString);
        }
      
    }
}





