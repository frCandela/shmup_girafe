using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class Checkpoint : PlayableAsset {
    public ExposedReference<LeaderboardText> leaderText;
    public int point;

    public CheckpointBehavior template = new CheckpointBehavior();

    public override Playable CreatePlayable(PlayableGraph graph, GameObject go) {
        var playable = ScriptPlayable<CheckpointBehavior>.Create(graph, template);
        CheckpointBehavior clone = playable.GetBehaviour();
        clone.point = point;
        clone.text = leaderText.Resolve(graph.GetResolver());
        return playable;
    }
}
