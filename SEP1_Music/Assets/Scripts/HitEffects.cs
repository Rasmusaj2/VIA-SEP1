using UnityEngine;

public class HitEffects : MonoBehaviour
{
    public Transform hitEffectContainer;
    public BeatmapPlayer player;
    private ParticleSystem[] hitEffects = new ParticleSystem[4];

    void OnEnable()
    {
        player.onHit += onHit;
    }

    void OnDisable()
    {
        player.onHit -= onHit;
    }

    void Start()
    {
        for (int i = 0; i < hitEffectContainer.childCount; i++)
        {
            hitEffects[i] = hitEffectContainer.GetChild(i).GetComponent<ParticleSystem>();
        }
    }

    private void onHit(LaneType laneType)
    {
        ParticleSystem particle = hitEffects[(int)laneType];
        particle.Play();
    }
}