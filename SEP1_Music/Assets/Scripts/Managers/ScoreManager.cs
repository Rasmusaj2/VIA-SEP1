using Math = System.Math;
using UnityEngine;

public enum AccuracyLevel
{
    MISSED, OK, GOOD, GREAT, PERFECT,
}

public class ScoreManager : MonoBehaviour
{
    public int combo = 0;
    public int scoreNumber = 0;
    public float accuracy;
    public AccuracyLevel accuracyLevel;
    public float maxScore;

    public delegate void ScoreEvent();
    public event ScoreEvent onScoreChange;

    private const double SCORE_PERFECT_THRESHOLD = 0.02;
    private const double SCORE_GREAT_THRESHOLD = 0.04;
    private const double SCORE_GOOD_THRESHOLD = 0.08;
    private const double SCORE_OK_THRESHOLD = 0.12;

    void Start()
    {
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
                accuracyLevel = AccuracyLevel.PERFECT;
                combo++;
                break;
            case < SCORE_GREAT_THRESHOLD:
                scoreNumber += 200;
                accuracyLevel = AccuracyLevel.GREAT;
                combo++;
                break;
            case < SCORE_GOOD_THRESHOLD:
                scoreNumber += 100;
                accuracyLevel = AccuracyLevel.GOOD;
                combo++;
                break;
            case < SCORE_OK_THRESHOLD:
                scoreNumber += 50;
                accuracyLevel = AccuracyLevel.OK;
                combo++;
                break;
            default:
                hit = false;
                accuracyLevel = AccuracyLevel.MISSED;
                combo = 0;
                break;
        }

        CalculateAccuracy();
        onScoreChange?.Invoke();

        return hit;
    }

    public void ResetScore()
    {
        scoreNumber = 0;
        combo = 0;
    }

    void CalculateAccuracy()
    {
        //Udregner accuracy display i UI ud fra de point man kunne have haft.
        maxScore += 300;
        accuracy = scoreNumber / maxScore * 100;
    }
}





