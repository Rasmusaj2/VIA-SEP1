using UnityEngine;
using UnityEngine.SceneManagement;


// Cross Scene Manager to hold data that needs to load between scenes

public static class CrossSceneManager
{
    public static Map SelectedMap;
    public static bool FirstBoot = true;

    public static Settings settings;

}
