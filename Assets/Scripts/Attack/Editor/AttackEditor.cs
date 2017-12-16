using UnityEditor;
using UnityEngine;
using UnityEditorInternal;

[CustomEditor(typeof(AttackType), true)]
public class AttackEditor : Editor {

    public override bool UseDefaultMargins() {
        return false;
    }

    public override void OnInspectorGUI() {
        serializedObject.Update();

        EditorGUILayout.BeginVertical(EditorStyles.inspectorDefaultMargins, new GUILayoutOption[0]);
        GUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button(new GUIContent("Open Editor..."), EditorStyles.miniButton, GUILayout.Width(110f)))
            Debug.Log("Test");
        GUILayout.EndHorizontal();
        EditorGUILayout.EndVertical();
        EditorGUILayout.BeginVertical(EditorStyles.inspectorFullWidthMargins, new GUILayoutOption[0]);
        drawGUI();
        EditorGUILayout.EndVertical();
        EditorGUILayout.BeginVertical(EditorStyles.inspectorDefaultMargins, new GUILayoutOption[0]);
        EditorGUILayout.EndVertical();

        serializedObject.ApplyModifiedProperties();
    }

    void OnEnable() {
        list = new ReorderableList(serializedObject,
                serializedObject.FindProperty("bursts"),
                true, true, true, true);

        list.drawHeaderCallback = (Rect rect) => {
            EditorGUI.LabelField(rect, "Bursts");
        };

        list.drawElementCallback =
            (Rect rect, int index, bool isActive, bool isFocused) => {
                var element = list.serializedProperty.GetArrayElementAtIndex(index);
                rect.y += 2;
                EditorGUI.PropertyField(
                    new Rect(rect.x, rect.y, 100, EditorGUIUtility.singleLineHeight),
                    element.FindPropertyRelative("type"), GUIContent.none);
                EditorGUI.PropertyField(
                    new Rect(rect.x + 105, rect.y, rect.width - 105 - 35, EditorGUIUtility.singleLineHeight),
                    element.FindPropertyRelative("prefab"), GUIContent.none);
                EditorGUI.PropertyField(
                    new Rect(rect.x + rect.width - 30, rect.y, 30, EditorGUIUtility.singleLineHeight),
                    element.FindPropertyRelative("bullets"), GUIContent.none);
            };
    }

    public GUIStyle modulePadding = new GUIStyle();
    public GUIStyle controlRectStyle = new GUIStyle();
    ReorderableList list;
    public void drawGUI() {
        GUILayout.BeginVertical("ShurikenEffectBg", new GUILayoutOption[0]);

        EditorGUIUtility.labelWidth = 0.0f;
        EditorGUIUtility.labelWidth -= 4f;
        EditorGUILayout.BeginVertical();

        for(int i = 0; i < 2; i++) {
            GUIContent content = new GUIContent();
            switch (i) {
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

            if(i == 0) {
                rect = GUILayoutUtility.GetRect(4f, 25f);
                style = (GUIStyle)"ShurikenEmitterTitle";
            } else {
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

            switch(i) {
                case 0:
                    EditorGUILayout.BeginHorizontal(GUILayout.ExpandWidth(true));
                    GUILayout.Label("Type:", GUILayout.Width(EditorGUIUtility.labelWidth - 5));
                    
                    string[] types = { "Type A", "Type B", "Type C"};
                    ((AttackType)target).type = GUILayout.Toolbar(((AttackType)target).type, types, GUILayout.ExpandWidth(true));

                    EditorGUILayout.EndHorizontal();

                    ((AttackType)target).rate = EditorGUILayout.FloatField("Rate", ((AttackType)target).rate);

                    break;
                case 1:
                    list.DoLayoutList();
                    break;
            }
            
            EditorGUILayout.EndVertical();
            
            if (GUI.Toggle(rect, true, content, style)) {}
            GUILayout.Space(1f);
        }
        
        GUILayout.Space(-1f);
        GUILayout.EndVertical();
        GUILayout.EndVertical();
        GUILayout.FlexibleSpace();
    }
}
