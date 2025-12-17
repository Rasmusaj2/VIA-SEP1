using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public static class AudioLoader
{
    public static AudioClip loadedAudio;

    public static IEnumerator LoadAudio(string filePath)
    {
        string url = "file://" + filePath;

        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.MPEG))
        {
            DownloadHandlerAudioClip dh = (DownloadHandlerAudioClip)www.downloadHandler;
            dh.streamAudio = true; // keep as true to avoid freezing process when reading file

            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Error loading audio " + www.error);
                yield break;
            }

            loadedAudio = DownloadHandlerAudioClip.GetContent(www);

            if (loadedAudio == null)
            {
                Debug.Log("Loaded audio is null");
                yield break;
            }
        }
    }
}