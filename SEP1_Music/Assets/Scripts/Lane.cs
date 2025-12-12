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

    public void AddNote(Note note)
    {
        notes.Add(note);
    }

    public void RemoveNote(Note note)
    {
        Note removing = notes[0];
        notes.Remove(note);
    }

    public int GetNotesCount()
    {
        return notes.Count;
    }

    public List<Note> GetNotes()
    {
        return notes;
    }
}
