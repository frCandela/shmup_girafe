using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

// A behaviour that is attached to a playable
public class CheckpointBehavior : PlayableBehaviour {
    public int point;
    public LeaderboardText text;

    public override void OnBehaviourPlay(Playable playable, FrameData info) {
        if (Application.isPlaying) {
            if (point < 1 && point > 4)
                throw new UnityException("No Point");

            text.UpdateScore(point);
            GameManager.instance.saveScore(point);
            
            playable.SetDone(true);
        }
    }
}
