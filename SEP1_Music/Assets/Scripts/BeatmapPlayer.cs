using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

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
    public ScoreManager scoreManager;
    public Transform hitEffectContainer;

    private Lane[] lanes = new Lane[4];

    private ParticleSystem[] hitEffects = new ParticleSystem[4];

    private double noteHitThreshold = 0.15;
    private double nextNoteBeat = 0.0;
    private float notesSpawnHeight = 5.0f;
    private float notesDestroyHeight = -5.0f;

    void Awake()
    {
        for (int i = 0; i < lanes.Length; i++)
        {
            lanes[i] = new Lane();
        }
    }

    void Start()
    {
        for (int i = 0; i < lanes.Length; i++)
        {
            hitEffects[i] = hitEffectContainer.GetChild(i).GetComponent<ParticleSystem>();
        }

        timeline.Play();
    }

    void Update()
    {
        {
            float height = timelineDisplay.ToDisplacement(nextNoteBeat - timeline.beat);
            while (height <= notesSpawnHeight)
            {
                SpawnNote((LaneType)Random.Range(0, 4), nextNoteBeat);
                nextNoteBeat += 1.0;

                height = timelineDisplay.ToDisplacement(nextNoteBeat - timeline.beat);
            }
        }

        UpdateNotePositions();
        RemoveDeadNotes();
    }

    private Note SpawnNote(LaneType laneType, double beat)
    {
        Lane lane = lanes[(int)laneType];
        Note note = noteSpawner.SpawnNote(laneType, beat);
        lane.AddNote(note);

        return note;
    }

    private void DespawnNote(Note note)
    {
        Lane lane = lanes[(int)note.laneType];
        if (lane.GetNotesCount() == 0)
            return;

        noteSpawner.DespawnNote(note);
        lane.RemoveNote(note);
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
                    DespawnNote(note);
                    scoreManager.EvaluateHit(0.0, 999.9);
                }
            }
        }
    }

    private Note GetTargetNote(LaneType laneType)
    {
        Lane lane = lanes[(int)laneType];
        for (int i = 0; i < lane.GetNotesCount(); i++)
        {
            Note note = lane.GetNotes()[i];
            double noteTime = timeline.ToSeconds(note.beat);
            if (noteTime + noteHitThreshold > timeline.time)
            {
                return note;
            }
        }

        return null;
    }

    private void ExecuteNoteHit(Note note, double hitTime)
    {
        double noteTime = timeline.ToSeconds(note.beat);
        bool hit = scoreManager.EvaluateHit(noteTime, hitTime);

        if (hit)
        {
            DespawnNote(note);
        }
    }

    public void Hit(LaneType laneType, HitPhase phase, double time)
    {
        if (phase != HitPhase.Attack)
            return;

        ParticleSystem particle = hitEffects[(int)laneType];
        particle.Play();

        Note note = GetTargetNote(laneType);
        if (note == null)
            return;

        double hitTime = time - timeline.realStartTime;

        ExecuteNoteHit(note, hitTime);
    }
}
