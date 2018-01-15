using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEditorInternal;

[CustomEditor(typeof(ChargePattern), true)]
public class ChargeEditor : Editor
{

    public override bool UseDefaultMargins()
    {
        return false;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.BeginVertical(EditorStyles.inspectorDefaultMargins, new GUILayoutOption[0]);
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button(new GUIContent("Random"), EditorStyles.miniButton, GUILayout.Width(110f)))
        {

        }
        GUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();
        EditorGUILayout.BeginVertical(EditorStyles.inspectorFullWidthMargins, new GUILayoutOption[0]);
        drawGUI();
        EditorGUILayout.EndVertical();
        EditorGUILayout.BeginVertical(EditorStyles.inspectorDefaultMargins, new GUILayoutOption[0]);
        EditorGUILayout.EndVertical();

        serializedObject.ApplyModifiedProperties();
    }

    void OnEnable()
    {
        list = new ReorderableList(serializedObject,
                serializedObject.FindProperty("charges"),
                true, true, true, true);

        list.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, "Charges");
        };

        list.drawElementCallback =
            (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                var element = list.serializedProperty.GetArrayElementAtIndex(index);
                rect.y += 2;

                EditorGUI.LabelField(new Rect(rect.x, rect.y, 60, EditorGUIUtility.singleLineHeight), "Direction");
                element.FindPropertyRelative("angle").floatValue = CustomGUI.Knob(new Rect(rect.x + 5, rect.y + EditorGUIUtility.singleLineHeight, 40, 40), element.FindPropertyRelative("angle").floatValue, false);
                EditorGUI.LabelField(new Rect(rect.x + 65, rect.y, 60, EditorGUIUtility.singleLineHeight), "Distance");
                element.FindPropertyRelative("distance").floatValue = EditorGUI.Slider(new Rect(rect.x + 65, rect.y + EditorGUIUtility.singleLineHeight, rect.width - 65 - 80, EditorGUIUtility.singleLineHeight), element.FindPropertyRelative("distance").floatValue, 0, 10);
            };

        list.elementHeight = EditorGUIUtility.singleLineHeight + 40 + 6;
    }

    public GUIStyle modulePadding = new GUIStyle();
    public GUIStyle controlRectStyle = new GUIStyle();
    ReorderableList list;
    public void drawGUI()
    {
        GUILayout.BeginVertical("ShurikenEffectBg", new GUILayoutOption[0]);

        EditorGUIUtility.labelWidth = 0.0f;
        EditorGUIUtility.labelWidth -= 4f;
        EditorGUILayout.BeginVertical();

        for (int i = 0; i <= 1; i++)
        {
            GUIContent content = new GUIContent();
            switch (i)
            {
                case 0:
                    content.text = "Global parameters";
                    content.tooltip = "Yahaha! You found me!";
                    break;
                case 1:
                    content.text = "Burst";
                    content.tooltip = "Setup burst";
                    break;
            }
            Rect rect;
            GUIStyle style;

            if (i == 0)
            {
                rect = GUILayoutUtility.GetRect(4f, 25f);
                style = (GUIStyle)"ShurikenEmitterTitle";
            }
            else
            {
                rect = GUILayoutUtility.GetRect(4f, 15f);
                style = (GUIStyle)"ShurikenModuleTitle";
            }

            style.clipping = TextClipping.Clip;
            style.padding.right = 45;

            modulePadding.padding = new RectOffset(3, 3, 4, 2);
            Rect position = EditorGUILayout.BeginVertical(modulePadding, new GUILayoutOption[0]);
            position.y -= 4f;
            position.height += 4f;
            GUI.Label(position, GUIContent.none, "ShurikenModuleBg");

            switch (i)
            {
                case 0:
                    ((ChargePattern)target).rate = EditorGUILayout.FloatField("Rate", ((ChargePattern)target).rate);
                    break;
                case 1:
                    list.DoLayoutList();
                    break;
            }

            EditorGUILayout.EndVertical();

            if (GUI.Toggle(rect, true, content, style))
            { }
            GUILayout.Space(1f);
        }

        GUILayout.Space(-1f);
        GUILayout.EndVertical();
        GUILayout.EndVertical();
        GUILayout.FlexibleSpace();
    }
}
