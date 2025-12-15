using UnityEngine;
using UnityEngine.UIElements;

public class PlaySceneUIScript : MonoBehaviour
{
    
    private VisualElement ui;
    public string leaderboardName;
    private Button saveBtn;
    private Button cancelBtn;
    private ScoreManager scoreManager;
    private VisualElement lbEntryPanel;


    private void Start()
    {
        scoreManager = gameObject.GetComponent<ScoreManager>();


        ui = gameObject.GetComponent<UIDocument>().rootVisualElement;

        ui.Q<TextField>("lbentryname").dataSource = this;
        

        lbEntryPanel.Q<VisualElement>("lbentrypanel");

        saveBtn = ui.Q<Button>("lbentrysavebtn");
        saveBtn.clicked += OnSaveBtnClicked;
        cancelBtn = ui.Q<Button>("lbentrycancelbtn");
        cancelBtn.clicked += OnCancelButtonClicked;
    }

    void OnSaveBtnClicked()
    {
        if (name != null)
        {
            scoreManager.SaveScore(name);
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


}
