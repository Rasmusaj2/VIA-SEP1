using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentScene : MonoBehaviour
{
    void Start()
    {
        SceneManager.LoadScene("MainMenu");
    }
}
