using System.Collections;
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
    public Metronome metronome;
    public NoteSpawner noteSpawner;
    public ScoreManager scoreManager;
    public Health health;

    public delegate void BeginEvent();
    public delegate void GameOverEvent();
    public delegate void HitEvent(LaneType laneType);
    public event BeginEvent onBegin;
    public event GameOverEvent onGameOver;
    public event HitEvent onHit;

    private Lane[] lanes = new Lane[4];

    private double noteHitThreshold = 0.15;
    private double nextNoteBeat = 0.0;
    private float notesSpawnHeight = 5.0f;
    private float notesDestroyHeight = -5.0f;

    private bool gameOver = false;

    void Awake()
    {
        for (int i = 0; i < lanes.Length; i++)
        {
            lanes[i] = new Lane();
        }
    }

    void Start()
    {
        Begin();
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

    private void Begin()
    {
        gameOver = false;

        Map map = CrossSceneManager.SelectedMap;
        if (map != null)
        {
            timeline.beatsPerMinute = map.beatmap.Tempo;
            StartCoroutine(LoadAndScheduleMusic());
        }
        else
        {
            Debug.LogWarning("No map selected");
        }

        health.Reset();
        timeline.Play();
        onBegin?.Invoke();
    }

    private void GameOver()
    {
        gameOver = true;
        Debug.Log("Game Over!");
        timeline.Stop();
        DespawnAllNotes();
        AudioManager.Instance.StopSong();
        onGameOver?.Invoke();
    }

    private IEnumerator LoadAndScheduleMusic()
    {
        Map map = CrossSceneManager.SelectedMap;
        if (map != null)
        {
            yield return AudioLoader.LoadAudio(map.AudioFilePath);
            AudioClip clip = AudioLoader.loadedAudio;
            double audioDelayCompensationSeconds = 0.001 * metronome.audioDelayCompensationMilliseconds;
            double scheduleTime = timeline.audioStartTime - audioDelayCompensationSeconds + map.beatmap.StartOffset;
            AudioManager.Instance.PlaySong(clip, 0.0, scheduleTime);
            Debug.Log("Playing song");
        }
        else
        {
            Debug.LogWarning("No map selected");
        }
    }

    private void TakeDamage()
    {
        health.TakeDamage();
        if (health.health == 0)
        {
            GameOver();
        }
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

    private void DespawnAllNotes()
    {
        for (int l = 0; l < lanes.Length; l++)
        {
            List<Note> notes = lanes[l].GetNotes();
            // Iterate in reverse order as we are removing notes
            for (int i = notes.Count - 1; i >= 0; i--)
                DespawnNote(notes[i]);
        }
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
                    TakeDamage();
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
        else
        {
            TakeDamage();
        }
    }

    public void Hit(LaneType laneType, HitPhase phase, double time)
    {
        if (gameOver)
            return;

        if (phase != HitPhase.Attack)
            return;

        onHit?.Invoke(laneType);

        Note note = GetTargetNote(laneType);
        if (note == null)
            return;

        double hitTime = time - timeline.realStartTime;

        ExecuteNoteHit(note, hitTime);
    }
}
