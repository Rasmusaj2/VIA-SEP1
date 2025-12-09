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

    void Awake()
    {
        for (int i = 0; i < lanes.Length; i++)
        {
            lanes[i] = new Lane();
        }
    }

    void Start()
    {
        if (CrossSceneManager.SelectedMap != null)
        {
            beatmap = CrossSceneManager.SelectedMap.beatmap;
            Debug.Log($"Loaded beatmap: {beatmap.MapName} with {beatmap.Nodes.Count} nodes.");
        }
        else
        {
            Debug.LogError("No beatmap selected in CrossSceneManager.");
        }
    }

    void Update()
    {
        UpdateNotePositions();
    }

    private void UpdateNotePositions()
    {
        for (int l = 0; l < lanes.Length; l++)
        {
            List<Note> notes = lanes[l].GetNotes();
            for (int i = 0; i < notes.Count; i++)
            {
                Note note = notes[i];
                timelineDisplay.PositionToTimeline(note.transform, timeline.ToBeats(note.time));
            }
        }
    }

    public void Hit(LaneType lane, HitPhase phase, double time) { }
}
