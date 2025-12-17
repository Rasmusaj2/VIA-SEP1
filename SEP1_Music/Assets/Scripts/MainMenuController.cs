using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;


public class MainMenuController : MonoBehaviour
{
    // Scener skal tilføjes i scene list under build profiles
    public string playScene = "PlayScene";
    public string leaderboardScene = "LeaderBoardScene";
    public string mapSelectorScene = "MapSelectorScene";

    public VisualElement mainMenuUI;    
    public Button playButton;
    public Button leaderboardButton;
    public Button exitButton;
    public Button optionsButton;
    public Button optionsCancelButton;

    public Button startCloseButton;
    public Button infinitePlayButton;
    public Button mapSelectButton;

    public VisualElement optionsPanel;
    public VisualElement gamemodePanel;

    private void Start()
    {
        Debug.Log("MainMenuController started");
        UserMaps.RefreshMaps();

        // Init persistent data directory at game boot
        if (CrossSceneManager.FirstBoot) {
            bool initSuccess = JSONPersistence.SuccessfulInitializeDirectory();
            if (initSuccess)
            {
                Debug.Log("Persistent data directory initialized successfully.");
            }
            else
            {
                Debug.LogError("Failed to initialize persistent data directory.");
            }
            CrossSceneManager.FirstBoot = false;
        }

        // Elementer fra UI hentes, hvorefter deres Actions tilknyttes metoder.
        mainMenuUI = GetComponent<UIDocument>().rootVisualElement;

        playButton = mainMenuUI.Q<Button>("startbutton");
        playButton.clicked += OnPlayButtonClicked;

        optionsButton = mainMenuUI.Q<Button>("optionsbutton");
        optionsButton.clicked += OnOptionsButtonClicked;

        leaderboardButton = mainMenuUI.Q<Button>("leaderboardbutton");
        leaderboardButton.clicked += OnLeaderboardButtonClicked;

        exitButton = mainMenuUI.Q<Button>("exitbutton");
        exitButton.clicked += OnExitButtonClicked;

        optionsCancelButton = mainMenuUI.Q<Button>("optionscancelbutton");
        optionsCancelButton.clicked += OnOptionsCancelButtonClicked;

        optionsPanel = mainMenuUI.Q<VisualElement>("optionspanel");
        gamemodePanel = mainMenuUI.Q<VisualElement>("gamemodepanel");

        
        startCloseButton = mainMenuUI.Q<Button>("startclosebutton");
        startCloseButton.clicked += OnPlayScreenCloseClicked;
        infinitePlayButton = mainMenuUI.Q<Button>("infiniteplaybutton");
        infinitePlayButton.clicked += OnInfinitePlayButtonClicked;
        mapSelectButton = mainMenuUI.Q<Button>("mapselectbutton");
        mapSelectButton.clicked += OnMapSelectButtonClicked;

    }

    private void OnPlayButtonClicked()
    {
        Debug.Log("Play button clicked");
        gamemodePanel.SetEnabled(true);
        gamemodePanel.visible = true;
    }

    private void OnPlayScreenCloseClicked()
    {
        Debug.Log("PlayScreen cancel button clicked");
        gamemodePanel.SetEnabled(false);
        gamemodePanel.visible = false;
    }

    private void OnInfinitePlayButtonClicked()
    {
        Debug.Log("Play button clicked");
        CrossSceneManager.SelectedMap = null;
        SceneManager.LoadScene(playScene);
    }

    private void OnMapSelectButtonClicked()
    {
        Debug.Log("Map Select button clicked");
        SceneManager.LoadScene(mapSelectorScene);
    }

    private void OnOptionsButtonClicked()
    {
        Debug.Log("Options button clicked");
        optionsPanel.SetEnabled(true);
        optionsPanel.visible = true;
    }

    private void OnLeaderboardButtonClicked()
    {
        Debug.Log("Leaderboard button clicked");
        SceneManager.LoadScene(leaderboardScene);
    }

    private void OnExitButtonClicked()
    {
        Debug.Log("Exit button clicked");
        
        //Lukker spillet for spilleren (ignoreres af editor)
        Application.Quit();
    }

    private void OnOptionsCancelButtonClicked()
    {
        Debug.Log("Options cancel button clicked");
        optionsPanel.SetEnabled(false);
        optionsPanel.visible = false;
    }
}
