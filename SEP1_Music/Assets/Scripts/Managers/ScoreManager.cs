using UnityEngine;
using UnityEngine.UIElements;

public class ScoreManager : MonoBehaviour
{
    public GameObject scoreLine;
    public UIDocument playSceneUIDoc;
    public int combo;
    public string scoreText;
    public int scoreNumber;
    public float accuracy;
    public string accuracyText = "100%";
    private float maxScore;


    private static float scoreLinePositionY;

    public void Start()
    {
        playSceneUIDoc.rootVisualElement.Q<Label>("scoretxt").dataSource = this;
        playSceneUIDoc.rootVisualElement.Q<Label>("combotxt").dataSource = this;
        playSceneUIDoc.rootVisualElement.Q<Label>("accuracytxt").dataSource = this;

        scoreText = $"{scoreNumber:D6}";

        scoreLinePositionY = scoreLine.transform.position.y;
    }

    public void EvaluateHit(float nodePosition)
    {
        switch (scoreLinePositionY - nodePosition)
        {
            case > 0.5f:
                this.scoreNumber += 200;
                combo++;
                break;
            case 0:
                this.scoreNumber += 300;
                combo++;
                break;
            case > -0.5f:
                this.scoreNumber += 200;
                combo++;
                break;
            case > -1:
                this.scoreNumber += 100;
                combo++;
                break;
            case > -1.5f:
                this.scoreNumber += 50;
                combo++;
                break;

        }

        scoreText = $"{scoreNumber:D6}";
        CalculateAccuracy();
        
    }

    public void MissedHit()
    {
        this.combo = 0;
        gameObject.GetComponent<InfinitePlayer>().health--;
    }

    public void ResetScore()
    {
        this.scoreNumber = 0;
        this.combo = 0;
        this.scoreText = "000000";
    }

    void CalculateAccuracy()
    {
        maxScore += 300;
        accuracy = scoreNumber / maxScore * 100;
        accuracyText = $"{accuracy:F2}%";
    }
}





