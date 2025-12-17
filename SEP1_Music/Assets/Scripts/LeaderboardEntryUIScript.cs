using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

public class LeaderboardEntryUIScript : MonoBehaviour
{
    public BeatmapPlayer player;
    public string name;

    private ScoreManager scoreManager;
    private UIDocument uiDoc;
    private Button saveBtn;
    private Button cancelBtn;
    private VisualElement lbEntryPanel;

    void Awake()
    {
        scoreManager = player.GetComponent<ScoreManager>();
        uiDoc = gameObject.GetComponent<UIDocument>();
    }

    private void Start()
    {
        VisualElement ui = uiDoc.rootVisualElement;
        ui.Q<TextField>("lbentryname").dataSource = this;


        lbEntryPanel = ui.Q<VisualElement>("lbentrypanel");

        saveBtn = ui.Q<Button>("lbentrysavebtn");
        saveBtn.clicked += OnSaveBtnClicked;
        cancelBtn = ui.Q<Button>("lbentrycancelbtn");
        cancelBtn.clicked += OnCancelButtonClicked;

        lbEntryPanel.visible = false;
        lbEntryPanel.SetEnabled(false);
    }

    void OnSaveBtnClicked()
    {
        if (name != null)
        {
            SaveScore(name);
            CloseSaveScorePanel();
        }
    }

    void OnCancelButtonClicked()
    {
        CloseSaveScorePanel();
    }

    public void DisplaySaveScorePanel()
    {
        lbEntryPanel.visible = true;
        lbEntryPanel.SetEnabled(true);
    }

    void CloseSaveScorePanel()
    {
        lbEntryPanel.visible = false;
        lbEntryPanel.SetEnabled(false);
    }

    public void SaveScore(string playerName)
    {
        Leaderboard leaderboard;

        if (CrossSceneManager.SelectedMap != null)
        {
            leaderboard = CrossSceneManager.SelectedMap.leaderboard;
            leaderboard.AddEntry(playerName, scoreManager.scoreNumber);
            CrossSceneManager.SelectedMap.SaveLeaderboard();
        }
        else
        {
            string jsonString = Path.Combine(JSONPersistence.Appdatapath, "leaderboard.json");
            leaderboard = JSONPersistence.LoadFromJSON<Leaderboard>(jsonString) ?? new Leaderboard();
            leaderboard.AddEntry(playerName, scoreManager.scoreNumber);
            JSONPersistence.SaveLeaderboardToJson(leaderboard, jsonString);
        }
    }
}
