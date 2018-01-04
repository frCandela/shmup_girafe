using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SpawnClip), true)]
public class SpawnEditor : Editor
{
    SpawnClip playable;

    public override void OnInspectorGUI()
    {
        playable = (SpawnClip)this.target;

        if (playable.enemies == null)
            playable.enemies = new List<Spawn>();

        serializedObject.Update();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("spawner"));

        EditorGUILayout.BeginHorizontal();

        EditorGUILayout.BeginVertical(GUILayout.ExpandWidth(true));
        if (GUILayout.Button("Add Pawn"))
        {
            Spawn sp = new Spawn();
            sp.position = UnityEngine.Random.insideUnitCircle;
            sp.col = UnityEngine.Random.ColorHSV(0, 1, 1, 1, 1, 1, 0.5f, 0.6f);
            playable.enemies.Add(sp);
            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
        }

        for (int i = 0; i < playable.enemies.Count; i++)
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Delete " + (char)('A' + (char)i), GUILayout.Width(60)))
            {
                playable.enemies.RemoveAt(i);
                serializedObject.ApplyModifiedProperties();
                serializedObject.Update();
                break;
            }
            EditorGUILayout.ObjectField(serializedObject.FindProperty("enemies").GetArrayElementAtIndex(i).FindPropertyRelative("enemy"), typeof(GameObject), GUIContent.none, GUILayout.ExpandWidth(true));
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndVertical();
        EditorGUILayout.BeginVertical(GUILayout.MinWidth(150), GUILayout.Width(150));
        List<Vector2> pos = playable.enemies.ConvertAll(getPos);
        List<Color> col = playable.enemies.ConvertAll(getCol);
        pos = CustomGUI.Grid(pos, col, GUILayout.Width(150), GUILayout.Height(250));
        for (int i = 0; i < playable.enemies.Count; i++)
            playable.enemies[i].position = new Vector2((pos[i].x - 75) / 75f, (pos[i].y - 125) / 125f);
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();

        serializedObject.ApplyModifiedProperties();

        this.DrawDefaultInspector();
    }

    private Vector2 getPos(Spawn input)
    {
        Vector2 res = new Vector2(input.position.x * 75 + 75, input.position.y * 125 + 125);

        return res;
    }
    private Color getCol(Spawn input)
    {
        return input.col;
    }
}
