using System;

public enum Lanes
{
    LeftLane,
    LeftMidLane,
    RightMidLane,
    RightLane
}

[Serializable]
public class BeatNode
{
    public float time { get; }
    public Lanes lane { get; }
    public BeatNode(float time, Lanes lane)
    {
        this.time = time;
        this.lane = lane;
    }
}

[Serializable]
public class Beatmap
{
    public string MapName { get; }
    public string Author { get; }
    public string Artist { get; }

    public float NodeSpeed { get; }


    public BeatNode[] Nodes { get; }

    public Beatmap(BeatNode[] nodes, string author, string artist, float nodeSpeed)
    {
        // Make sure nodes are kept in order for playback
        // Use of lambda function to make sure to sort based on node-time (could be done by overriding compareTo as well)
        Array.Sort(nodes, (a,b) => a.time.CompareTo(b.time));

        Nodes = nodes;
        Author = author;
        Artist = artist;
        NodeSpeed = nodeSpeed;
    }
}