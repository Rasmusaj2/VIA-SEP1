using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

public class PlaySceneUIScript : MonoBehaviour
{
    public BeatmapPlayer player;
    public string scoreText;
    public string accuracyText = "100%";
    public string hitText;
    public string combo;

    private const string SCORE_PERFECT_TEXT = "Perfect!";
    private const string SCORE_GREAT_TEXT = "Great!";
    private const string SCORE_GOOD_TEXT = "Good";
    private const string SCORE_OK_TEXT = "OK";
    private const string SCORE_MISSED_TEXT = "Missed";
    private const string SCORE_GAMEOVER_TEXT = "Game Over!";

    private UIDocument uiDoc;
    private ScoreManager scoreManager;

    void OnEnable()
    {
        scoreManager.onScoreChange += OnScoreChange;
        player.onGameOver += OnGameOver;
    }

    void OnDisable()
    {
        scoreManager.onScoreChange -= OnScoreChange;
        player.onGameOver -= OnGameOver;
    }

    void Awake()
    {
        scoreManager = player.GetComponent<ScoreManager>();
        uiDoc = gameObject.GetComponent<UIDocument>();
    }

    private void Start()
    {
        VisualElement ui = uiDoc.rootVisualElement;
        //De forskellige UI elementer der viser score databindes til scoremanageren.
        ui.Q<Label>("scoretxt").dataSource = this;
        ui.Q<Label>("combotxt").dataSource = this;
        ui.Q<Label>("accuracytxt").dataSource = this;
        ui.Q<Label>("hittext").dataSource = this;

        scoreText = $"{scoreManager.scoreNumber:D6}";
    }

    private void OnScoreChange()
    {
        scoreText = $"{scoreManager.scoreNumber:D6}";
        accuracyText = $"{scoreManager.accuracy:F2}%";
        combo = scoreManager.combo.ToString();

        switch (scoreManager.accuracyLevel)
        {
            case AccuracyLevel.PERFECT: hitText = SCORE_PERFECT_TEXT; break;
            case AccuracyLevel.GREAT: hitText = SCORE_GREAT_TEXT; break;
            case AccuracyLevel.GOOD: hitText = SCORE_GOOD_TEXT; break;
            case AccuracyLevel.OK: hitText = SCORE_OK_TEXT; break;
            case AccuracyLevel.MISSED: hitText = SCORE_MISSED_TEXT; break;
        }
    }

    private void OnGameOver()
    {
        hitText = SCORE_GAMEOVER_TEXT;
    }
}
