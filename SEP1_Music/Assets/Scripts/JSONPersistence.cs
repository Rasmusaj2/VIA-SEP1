using UnityEngine;
using System.Text;
using System.IO;

public static class JSONPersistence
{
    // remember to serialize your classes by adding [Serializable] above them and making it public


    public static readonly string Appdatapath = Application.persistentDataPath;

    public static bool SuccessfulInitializeDirectory()

    {
        try
        {
            Debug.Log($"Attempting to initialize directory: {Appdatapath}");
            System.IO.Directory.CreateDirectory(Appdatapath);
            Debug.Log($"Created directory at: {Appdatapath}");
            System.IO.Directory.CreateDirectory(Path.Combine(Appdatapath, "maps"));
            Debug.Log($"Created directory at: {Path.Combine(Appdatapath, "maps")}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Failed to create directory ({Appdatapath}): " + e.Message);
            return false;
        }
        return true;
    }


    // Generic method for saving and loading serializable JSON objects
    // Uses generic type T to allow for any serializable class to be saved
    // Usage: SaveToJSON<Class>(yourObject, "filename.json");
    // Filename should include .json extension, automatically saved to Application.persistentDataPath
    public static void SaveToJSON<T>(T data, string filename)
    {
        string jsonString = JsonUtility.ToJson(data, true);
        System.IO.File.WriteAllText(filename, jsonString, Encoding.UTF8);
    }

    // Usage: Class object = LoadFromJSON<Class>("filename.json");
    // Returns default(T) if file not found, automatically loads from Application.persistentDataPath
    public static T LoadFromJSON<T>(string filename)
    {
        if (!System.IO.File.Exists(filename))
        {
            Debug.LogWarning($"File not found: {filename}");
            return default(T);
        }
        string jsonString = System.IO.File.ReadAllText(filename, Encoding.UTF8);
        T data = JsonUtility.FromJson<T>(jsonString);
        return data;
    }

    // Leaderboard specific methods for convenience
    public static void SaveLeaderboardToJson(Leaderboard leaderboard, string filename)
    {
        string jsonString = JsonUtility.ToJson(leaderboard, true);
        System.IO.File.WriteAllText(filename, jsonString, Encoding.UTF8);
    }
    
}

