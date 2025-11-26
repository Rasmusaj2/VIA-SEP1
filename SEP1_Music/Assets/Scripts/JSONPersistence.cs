using UnityEngine;
using System.Text;
using System.IO;

public static class JSONPersistence
{
    private static readonly string Appdatapath = Application.persistentDataPath;
    private static readonly string subdirectory = "/MusicGame/";

    public static bool SuccessfulInitializeDirectory()

    {
        string path = Path.Combine(Appdatapath, subdirectory);
        if (!System.IO.Directory.Exists(path))
        {
            try
            {
                System.IO.Directory.CreateDirectory(path);
            }
            catch (System.Exception e)
            {
                Debug.LogError($"Failed to create directory ({path}): " + e.Message);
                return false;
            }
        }
        return true;
    }

    public static void SaveToJSON<T>(T data, string filename)
    {
        string fullPath = Path.Combine(Appdatapath, subdirectory, filename);
        string jsonString = JsonUtility.ToJson(data, true);
        System.IO.File.WriteAllText(fullPath, jsonString, Encoding.UTF8);
    }

    public static T LoadFromJSON<T>(string filename)
    {
        string fullPath = Path.Combine(Appdatapath, subdirectory, filename);
        if (!System.IO.File.Exists(fullPath))
        {
            Debug.LogWarning($"File not found: {fullPath}");
            return default(T);
        }
        string jsonString = System.IO.File.ReadAllText(fullPath, Encoding.UTF8);
        T data = JsonUtility.FromJson<T>(jsonString);
        return data;
    }
}

