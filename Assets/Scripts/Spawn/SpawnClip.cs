using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class SpawnClip : PlayableAsset
{
    public ExposedReference<Spawner> spawner;
    public List<Spawn> enemies;

    public SpawnBehavior template = new SpawnBehavior();

    public override Playable CreatePlayable(PlayableGraph graph, GameObject go)
    {
        var playable = ScriptPlayable<SpawnBehavior>.Create(graph, template);
        SpawnBehavior clone = playable.GetBehaviour();
        clone.spawner = spawner.Resolve(graph.GetResolver());
        clone.data = enemies;
        return playable;
    }
}

[System.Serializable]
public class Spawn
{
    [SerializeField]
    public GameObject enemy;
    [SerializeField]
    public Vector2 position;
#if UNITY_EDITOR
    [SerializeField]
    public Color col;
#endif
}