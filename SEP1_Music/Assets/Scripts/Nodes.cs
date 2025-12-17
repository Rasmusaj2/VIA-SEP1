using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;

// SERIALIZED FIELDS CANNOT BE PROPERTIES, MUST BE PUBLIC FIELDS OR [SERIALIZEFIELD] PRIVATE FIELDS

[Serializable]
public class BeatNode
{
    public float time;
    public LaneType lane;
    public BeatNode(float time, LaneType lane)
    {
        this.time = time;
        this.lane = lane;
    }
}

[Serializable]
public class Beatmap
{
    public string MapName;
    public string Author;
    public string Artist;

    public double Tempo;
    public double StartOffset;

    public List<BeatNode> Nodes;

    public Beatmap(List<BeatNode> nodes, string author, string artist, double tempo)
    {
        // Make sure nodes are kept in order for playback
        // Use of lambda function to make sure to sort based on node-time (could be done by overriding compareTo as well)
        nodes.Sort((a,b) => a.time.CompareTo(b.time));

        Nodes = nodes;
        Author = author;
        Artist = artist;
        Tempo = tempo;
        StartOffset = 0.0;
    }
}

public class Map
{ 
    public string MapFolder { get; }
    public Beatmap beatmap { get; }
    public Leaderboard leaderboard { get; }
    public string AudioFilePath { get; }
    public string CoverFilePath { get; }

    public Map(string mapFolder)
    {
        MapFolder = mapFolder;
        beatmap = JSONPersistence.LoadFromJSON<Beatmap>(Path.Combine(mapFolder, "beatmap.json"));
        leaderboard = JSONPersistence.LoadFromJSON<Leaderboard>(Path.Combine(mapFolder, "leaderboard.json")) ?? new Leaderboard();
        AudioFilePath = Path.Combine(mapFolder, "audio.mp3") ?? null;
        CoverFilePath = Path.Combine(mapFolder, "cover.png") ?? null;
    }

    public void SaveLeaderboard()
    {
        JSONPersistence.SaveToJSON<Leaderboard>(leaderboard, Path.Combine(MapFolder, "leaderboard.json"));
    }
}

public static class UserMaps
{
    public static string MapFolder = Path.Combine(JSONPersistence.Appdatapath, "maps");
    public static List<Map> Maps = new List<Map>();

    public static void RefreshMaps()
    {
        Maps.Clear();
        if (Directory.Exists(MapFolder))
        {
            string[] mapDirectories = Directory.GetDirectories(MapFolder);
            foreach (string dir in mapDirectories)
            {
                if (!File.Exists(Path.Combine(dir, "beatmap.json")))
                {
                    Debug.LogWarning($"No beatmap.json found in directory: {dir}");
                    continue;
                }

                Map map = new Map(dir);
                if (map.beatmap != null)
                {
                    Maps.Add(map);
                }
                else
                {
                    Debug.LogWarning($"Beatmap not found or invalid in directory: {dir}");
                }
            }
        }
        else
        {
            Debug.LogWarning($"Maps directory does not exist: {MapFolder}");
        }
    }

}