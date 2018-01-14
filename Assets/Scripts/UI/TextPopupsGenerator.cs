using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class TextPopupsGenerator : MonoBehaviour
{
    public GameObject scorePopupPrefab;

    public void generateScorePopup( string text, Vector3 position)
    {
        if(scorePopupPrefab)
        {
            GameObject popup = Instantiate(scorePopupPrefab, transform);

            Vector3 screenPos = Camera.main.WorldToScreenPoint(position);
            popup.transform.position = screenPos;
            popup.GetComponent<Text>().text = text;

        }
    }
}

#if UNITY_EDITOR

//Custom editor of the mainbar for debug only
[CustomEditor(typeof(TextPopupsGenerator))]
public class TextPopupsGeneratorEditor : Editor
{
    public Vector2 position;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        TextPopupsGenerator myTextPopupsGenerator = (TextPopupsGenerator)target;

        
        EditorGUILayout.LabelField("Editor :", EditorStyles.boldLabel);
        EditorGUILayout.Vector2Field("position", position);
        if (GUILayout.Button("test"))
            myTextPopupsGenerator.generateScorePopup("yolo", position);
    }
}
#endif