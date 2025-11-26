

public enum Lanes
{
    LeftLane,
    LeftMidLane,
    RightMidLane,
    RightLane
}

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