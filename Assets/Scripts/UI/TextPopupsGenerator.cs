using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class TextPopupsGenerator : MonoBehaviour
{
    public GameObject positiveScorePopupPrefab;
    public GameObject negativeScorePopupPrefab;

    private void Start()
    {
        if (!positiveScorePopupPrefab)
            throw new Exception("Error : no positiveScorePopupPrefab selected");
        if (!negativeScorePopupPrefab)
            throw new Exception("Error : no negativeScorePopupPrefab selected");
    }

    public void generateScorePopup(int value, Vector3 position)
    {
        GameObject popup;
        if (value > 0)
            popup = Instantiate(positiveScorePopupPrefab, transform);
        else if (value < 0)
            popup = Instantiate(negativeScorePopupPrefab, transform);
        else
            return;

        Vector3 screenPos = Camera.main.WorldToScreenPoint(position);
        popup.transform.position = screenPos;
        popup.GetComponent<Text>().text = "+" + value.ToString();
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
            myTextPopupsGenerator.generateScorePopup(42, position);
    }
}
#endif