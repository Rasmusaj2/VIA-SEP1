using UnityEngine;

public class MainMenuMusic : MonoBehaviour
{
    public AudioClip menuMusic;

    void Start()
    {
        if(menuMusic != null)
        {
            // Debug.Log("MainMenuMusic playing menu music");
            AudioManager.Instance.currentMode = AudioManager.MusicNode.Menu;
            AudioManager.Instance.PlayLoopSong(menuMusic);
        }
        else
        {
            // Debug.Log("MainMenuMusic no music assigned");
        }
    }
}