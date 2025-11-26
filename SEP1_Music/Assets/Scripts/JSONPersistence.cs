using UnityEngine;
using System.Text;
using System.Text.Json;
using System.IO;

public static class JSONPersistence
{
    private static readonly string Appdatapath = Application.persistentDataPath;
    private static readonly string subdirectory = "/MusicGame/";

    private static JsonSerializerOptions serializerOptions = new JsonSerializerOptions()
    {
        WriteIndented = true,
        IndentSize = 2
    };

    public static bool SuccessfulInitialize()

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
        string jsonString = JsonSerializer.Serialize(data, serializerOptions);
        System.IO.File.WriteAllText(fullPath, jsonString, Encoding.UTF8);
    }
}

