﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
    using UnityEditor;
#endif

public class Blink : MonoBehaviour
{
    [Header("Blink parameters:")]
    public float BlinkDuration = 1;
    public float BlinkFrequency = 10;
    public SpriteRenderer[] SpriteRenderers;

    //Private parameters
    private float BlinkEndTime;
    private float BlinkDeltaTime;
    private bool BlinkState;

    private void Start()
    {
        enabled = false;
    }

    //BlinkCoroutine
    IEnumerator BlinkCoroutine()
    {
        while( Time.time < BlinkEndTime )
        {
            foreach (SpriteRenderer renderer in SpriteRenderers)
                renderer.enabled = BlinkState;
            BlinkState = !BlinkState;
            yield return new WaitForSeconds(BlinkDeltaTime);
        }
            foreach (SpriteRenderer renderer in SpriteRenderers)
                renderer.enabled = true;
    }

    //Initialize and stard a blink coroutine
    public void StartBlink()
    {
        if(SpriteRenderers != null)
        {
            StopAllCoroutines();
            BlinkEndTime = Time.time + BlinkDuration;
            BlinkDeltaTime = 1f / BlinkFrequency;
            BlinkState = false;
            StartCoroutine("BlinkCoroutine");
        }
    }
}

#if UNITY_EDITOR
//Custom editor of the mainbar for debug only
[CustomEditor(typeof(Blink))]
public class BlinkEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Blink myBlink = (Blink)target;

        //Combo
        if (GUILayout.Button("Blink"))
            if (EditorApplication.isPlaying)
                myBlink.StartBlink();
            else
                throw (new System.Exception("This button is PlayMode Only"));
       
    }
}
#endif