using UnityEngine;

public class Metronome : MonoBehaviour
{
    public Timeline timeline;
    public AudioClip beatSoundClip;
    public AudioClip semiBeatSoundClip;
    [Range(0.0f, 1000.0f)]
    public double audioDelayCompensationMilliseconds = 40.0;
    [Min(0)]
    public double scheduleAhead = 0.2;

    private double nextEventTime;
    private int source = 0;
    private AudioSource[] audioSources = new AudioSource[8];

    void Start()
    {
        for (int i = 0; i < audioSources.Length; i++)
        {
            GameObject child = new GameObject("AudioPlayer");
            child.transform.parent = transform;
            audioSources[i] = child.AddComponent<AudioSource>();
        }

        nextEventTime = 0.0;
    }

    void Update()
    {
        double audioDelayCompensationSeconds = 0.001 * audioDelayCompensationMilliseconds;

        if (timeline.time + scheduleAhead > nextEventTime)
        {
            int beat = (int)(timeline.ToBeats(nextEventTime));
            int subdivision = beat % timeline.beatsPerMeasure;

            audioSources[source].clip = subdivision == 0 ? beatSoundClip : semiBeatSoundClip;
            audioSources[source].PlayScheduled(timeline.ToRealTime(nextEventTime) - audioDelayCompensationSeconds);
            source = (source + 1) % audioSources.Length;

            Debug.Log("Scheduled source " + source + " to start at time " + nextEventTime + " (beat " + beat + ")");

            nextEventTime = timeline.ToSeconds(beat + 1.0);
        }
    }
}
