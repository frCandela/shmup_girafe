﻿using System.Collections;
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
                throw new UnityException("No Spawner");

            if (playable.GetTime() > 0.15f)
                return;
            foreach (Spawn s in data)
                spawner.Spawn(s.enemy, s.position, -s.angle);
            playable.SetDone(true);
        }
    }
}
