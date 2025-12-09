using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum LaneType
{
    LeftLane,
    LeftMidLane,
    RightMidLane,
    RightLane
}

[Serializable]
public class Lane
{
    private List<Note> notes = new List<Note>();

    public Note GetFirstNote()
    {
        return notes[0];
    }

    public List<Note> GetNotes()
    {
        return notes;
    }
}
