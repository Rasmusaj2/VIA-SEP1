using UnityEngine;

public class InfinityModeMusic : MonoBehaviour
{
    void Start()
    {
        AudioManager.Instance.currentMode = AudioManager.MusicNode.Infinity;
        AudioManager.Instance.PlayRandomSong();
    }
}
