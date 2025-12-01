using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;


public class MainMenuController : MonoBehaviour
{
    // Scener skal tilføjes i scene list under build profiles
    string playScene = "PlayScene";
    string leaderboardScene = "LeaderBoardScene";
    string optionsScene = "OptionsScene";

    public VisualElement ui;    
    public Button playButton;
    public Button leaderboardButton;
    public Button exitButton;
    public Button optionsButton;

    private void Start()
    {

        // Elementer fra UI hentes, hvorefter deres Actions tilknyttes metoder.
        ui = GetComponent<UIDocument>().rootVisualElement;

        playButton = ui.Q<Button>("startbutton");
        playButton.clicked += OnPlayButtonClicked;

        optionsButton = ui.Q<Button>("optionsbutton");
        optionsButton.clicked += OnOptionsButtonClicked;

        leaderboardButton = ui.Q<Button>("leaderboardbutton");
        leaderboardButton.clicked += OnLeaderboardButtonClicked;

        exitButton = ui.Q<Button>("exitbutton");
        exitButton.clicked += OnExitButtonClicked;
    }

    private void OnPlayButtonClicked()
    {
        Debug.Log("Play button clicked");
        SceneManager.LoadScene(playScene);
    }

    private void OnOptionsButtonClicked()
    {
        Debug.Log("Options button clicked");
        SceneManager.LoadScene(optionsScene);
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
}
