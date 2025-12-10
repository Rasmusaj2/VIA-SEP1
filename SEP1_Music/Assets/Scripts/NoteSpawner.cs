using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    [SerializeField]
    private Transform[] lanes = new Transform[4];
    [SerializeField]
    private GameObjectPool[] notePools = new GameObjectPool[4];

    public Note SpawnNote(LaneType lane, double beat)
    {
        Transform laneTransform = lanes[(int)lane]; // Cast lane number (0-3) to get the correct lane transform
        Vector3 spawnPosition = new Vector3(
            laneTransform.position.x,
            0.5f * laneTransform.localScale.y
        );

        GameObjectPool pool = notePools[(int)lane];
        GameObject noteObject = pool.Acquire();
        Note note = noteObject.GetComponent<Note>();
        note.beat = beat;
        note.laneType = lane;
        note.transform.position = spawnPosition;

        return note;
    }

    public void DespawnNote(Note note)
    {
        GameObjectPool pool = notePools[(int)note.laneType];
        pool.Release(note.gameObject);
    }
}