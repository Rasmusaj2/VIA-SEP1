using UnityEngine;

public class NoteSpawner : MonoBehaviour
{
    [SerializeField]
    private Transform[] lanes = new Transform[4];
    [SerializeField]
    private GameObjectPool notePool;

    public Note SpawnNote(LaneType lane, double time)
    {
        Transform laneTransform = lanes[(int)lane]; // Cast lane number (0-3) to get the correct lane transform
        Vector3 spawnPosition = laneTransform.position + new Vector3(0f, 0.5f * laneTransform.localScale.y, 0f);

        GameObject noteObject = notePool.Acquire();
        Note note = noteObject.GetComponent<Note>();
        note.time = time;

        return note;
    }

    public void DespawnNote(Note note)
    {
        notePool.Release(note.gameObject);
    }
}