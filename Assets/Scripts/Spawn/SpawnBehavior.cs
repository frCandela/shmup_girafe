using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

// A behaviour that is attached to a playable
public class SpawnBehavior : PlayableBehaviour
{
    public Spawner spawner;
    public List<Spawn> data;

    public override void OnBehaviourPlay(Playable playable, FrameData info)
    {
        if (Application.isPlaying)
        {
            if (!spawner)
            {
                UnityEditor.EditorApplication.isPlaying = false;
                throw new UnityException("No Spawner");
            }

            foreach (Spawn s in data)
                spawner.Spawn(s.enemy, s.position);
            playable.SetDone(true);
        }
    }
}
