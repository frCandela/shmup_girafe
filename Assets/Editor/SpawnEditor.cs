using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SpawnClip), true)]
public class SpawnEditor : Editor
{
    SpawnClip playable;

    public SpawnEditor() {
        Undo.undoRedoPerformed += this.Repaint;
    }

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
            sp.angle = 180;
            playable.enemies.Add(sp);
            serializedObject.ApplyModifiedProperties();
            serializedObject.Update();
        }

        for (int i = 0; i < playable.enemies.Count; i++)
        {
            EditorGUILayout.Space();

            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Delete " + (char)('A' + (char)i), GUILayout.Width(60)))
            {
                playable.enemies.RemoveAt(i);
                serializedObject.ApplyModifiedProperties();
                serializedObject.Update();
                break;
            }
            SerializedProperty enemy = serializedObject.FindProperty("enemies").GetArrayElementAtIndex(i);
            EditorGUILayout.ObjectField(enemy.FindPropertyRelative("enemy"), typeof(GameObject), GUIContent.none, GUILayout.Width(150));
            enemy.FindPropertyRelative("angle").intValue = (int)CustomGUI.Knob(enemy.FindPropertyRelative("angle").intValue, Color.red);
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.EndVertical();
        EditorGUILayout.BeginVertical(GUILayout.MinWidth(150), GUILayout.Width(150));
        List<Vector2> pos = playable.enemies.ConvertAll(getPos);
        List<int> angle = playable.enemies.ConvertAll(getAngle);
        List<Color> col = playable.enemies.ConvertAll(getCol);
        EditorGUI.BeginChangeCheck();
        pos = CustomGUI.Grid(pos, angle, col, GUILayout.Width(150), GUILayout.Height(250));
        if(EditorGUI.EndChangeCheck()) {
            Undo.RecordObject(playable, "Modify Spawn");
        }
        for (int i = 0; i < playable.enemies.Count; i++)
            playable.enemies[i].position = new Vector2((pos[i].x - 75) / 75f, (pos[i].y - 125) / 125f);
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();

        serializedObject.ApplyModifiedProperties();

        showDefault = EditorGUILayout.Toggle("Show advanced configuration", showDefault);
        if(showDefault)
            this.DrawDefaultInspector();
    }
    static bool showDefault = false;

    private Vector2 getPos(Spawn input)
    {
        Vector2 res = new Vector2(input.position.x * 75 + 75, input.position.y * 125 + 125);

        return res;
    }

    private int getAngle(Spawn input) {
        return input.angle;
    }

    private Color getCol(Spawn input)
    {
        return input.col;
    }
}
