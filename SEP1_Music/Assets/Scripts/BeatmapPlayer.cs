using System.Collections.Generic;
using UnityEngine;

public enum HitPhase
{
    Attack,
    Release,
}

public class BeatmapPlayer : MonoBehaviour
{
    [Header("References")]
    public Beatmap beatmap;
    public Timeline timeline;
    public TimelineDisplay timelineDisplay;
    public NoteSpawner noteSpawner;

    [SerializeField]
    private Lane[] lanes = new Lane[4];

    private double nextNoteBeat = 32.0;
    private float notesSpawnHeight = 5.0f;
    private float notesDestroyHeight = -5.0f;

    void Awake()
    {
        lanes = new Lane[4];

        for (int i = 0; i < lanes.Length; i++)
        {
            lanes[i] = new Lane();
        }
    }

    void Start()
    {
    }

    void Update()
    {
        {
            float height = timelineDisplay.ToDisplacement(nextNoteBeat - timeline.beat);
            while (height <= notesSpawnHeight)
            {
                SpawnNote((LaneType)Random.Range(0, 4), nextNoteBeat);
                Debug.Log(nextNoteBeat);
                nextNoteBeat += 0.25;

                height = timelineDisplay.ToDisplacement(nextNoteBeat - timeline.beat);
                Debug.Log(height);
            }
        }

        UpdateNotePositions();
        RemoveDeadNotes();
    }

    private Note SpawnNote(LaneType laneType, double beat)
    {
        Debug.Log("Index: " + (int)laneType);
        Debug.Log(lanes.Length);
        Lane lane = lanes[(int)laneType];
        Note note = noteSpawner.SpawnNote(laneType, beat);
        lane.AddNote(note);

        return note;
    }

    private void DespawnNote(LaneType laneType)
    {
        Lane lane = lanes[(int)laneType];
        Note note = lane.GetFirstNote();
        noteSpawner.DespawnNote(note);
        lane.RemoveNote();
    }

    private void UpdateNotePositions()
    {
        for (int l = 0; l < lanes.Length; l++)
        {
            List<Note> notes = lanes[l].GetNotes();
            for (int i = 0; i < notes.Count; i++)
            {
                Note note = notes[i];
                timelineDisplay.PositionToTimeline(note.transform, note.beat);
            }
        }
    }

    private void RemoveDeadNotes()
    {
        for (int l = 0; l < lanes.Length; l++)
        {
            List<Note> notes = lanes[l].GetNotes();
            // Iterate in reverse order as we are removing notes
            for (int i = notes.Count - 1; i >= 0; i--)
            {
                Note note = notes[i];
                float height = timelineDisplay.ToDisplacement(note.beat - timeline.beat);
                if (height < notesDestroyHeight)
                {
                    DespawnNote((LaneType)l);
                }
            }
        }
    }

    public void Hit(LaneType lane, HitPhase phase, double time) { }
}
