using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Rendering;

static class CustomGUI
{
    #region KNOB

    #region CONSTRUCTOR
    public static float Knob(float value, bool up = true)
    {
        Rect pos = EditorGUILayout.GetControlRect(false, 40, GUILayout.Width(40));
        return new KnobContext(pos, value, 0, Color.black, Color.red, up).GetValue();
    }
    public static float Knob(float value, Color pickerColor, bool up = true)
    {
        Rect pos = EditorGUILayout.GetControlRect(false, 40, GUILayout.Width(40));
        return new KnobContext(pos, value, 0, pickerColor, Color.red, up).GetValue();
    }
    public static float Knob(float value, float spread, bool up = true)
    {
        Rect pos = EditorGUILayout.GetControlRect(false, 40, GUILayout.Width(40));
        return new KnobContext(pos, value, spread, Color.black, Color.red, up).GetValue();
    }
    public static float Knob(float value, float spread, Color pickerColor, Color rangeColor, bool up = true)
    {
        Rect pos = EditorGUILayout.GetControlRect(false, 40, GUILayout.Width(40));
        return new KnobContext(pos, value, spread, pickerColor, rangeColor, up).GetValue();
    }

    public static float Knob(Rect pos, float value, bool up = true)
    {
        return new KnobContext(pos, value, 0, Color.black, Color.red, up).GetValue();
    }
    public static float Knob(Rect pos, float value, Color pickerColor, bool up = true)
    {
        return new KnobContext(pos, value, 0, pickerColor, Color.red, up).GetValue();
    }
    public static float Knob(Rect pos, float value, float spread, bool up = true)
    {
        return new KnobContext(pos, value, spread, Color.black, Color.red, up).GetValue();
    }
    public static float Knob(Rect pos, float value, float spread, Color pickerColor, Color rangeColor, bool up = true)
    {
        return new KnobContext(pos, value, spread, pickerColor, rangeColor, up).GetValue();
    }
    #endregion

    #region INTERNAL
    private class KnobState
    {
        public bool isDragging;
    }

    private class KnobContext
    {
        int id;
        float value;
        Rect position;
        Vector2 knobSize;
        float spread;
        Color pickerColor;
        Color rangeColor;
        bool up;

        public KnobContext(Rect position, float value, float spread, Color pickerColor, Color rangeColor, bool up = true)
        {
            this.id = GUIUtility.GetControlID(GetType().GetHashCode(), FocusType.Passive, position);
            this.position = position;
            this.value = value;
            this.spread = spread;
            this.pickerColor = pickerColor;
            this.rangeColor = rangeColor;
            this.knobSize = new Vector2(40, 40);
            this.up = up;
        }

        public float GetValue()
        {
            switch (Event.current.type)
            {
                case EventType.MouseDown:
                    return this.OnMouseDown();
                case EventType.MouseUp:
                    return this.OnMouseUp();
                case EventType.MouseDrag:
                    return this.OnMouseDrag();
                case EventType.Repaint:
                    return this.OnRepaint();
                default:
                    return this.value;
            }
        }

        public KnobState state()
        {
            return (KnobState)GUIUtility.GetStateObject(typeof(KnobState), this.id);
        }

        private float OnMouseDown()
        {
            if (!this.position.Contains(Event.current.mousePosition))
                return this.value;
            GUIUtility.hotControl = this.id;
            this.state().isDragging = true;
            GUI.changed = true;
            Event.current.Use();
            float res = Mathf.Atan2((this.position.y + this.knobSize.y / 2f) - Event.current.mousePosition.y, (this.position.x + this.knobSize.x / 2f) - Event.current.mousePosition.x) / Mathf.PI * 180f + 90 * (up?-1:1);
            if (res <= -180)
                res += 360;
            return res;
        }

        private float OnMouseUp()
        {
            if (GUIUtility.hotControl == this.id)
            {
                Event.current.Use();
                GUIUtility.hotControl = 0;
                state().isDragging = false;
            }
            return value;
        }

        private float OnMouseDrag()
        {
            if (GUIUtility.hotControl != this.id)
                return this.value;
            if (!state().isDragging)
                return this.value;
            GUI.changed = true;
            Event.current.Use();
            float res = Mathf.Atan2((this.position.y + this.knobSize.y / 2f) - Event.current.mousePosition.y, (this.position.x + this.knobSize.x / 2f) - Event.current.mousePosition.x) / Mathf.PI * 180f + 90 * (up ? -1 : 1);
            if (res <= -180)
                res += 360;
            return res;
        }

        private float OnRepaint()
        {
            this.DrawValueArc((float)((double)(value / 360f) * Mathf.PI * 2 + Mathf.PI / 2f * (up ? 3 : 1)), rangeColor, (spread / 360f) * Mathf.PI, true, Color.grey);
            this.DrawValueArc((float)((double)(value / 360f) * Mathf.PI * 2 + Mathf.PI / 2f * (up ? 3 : 1)), pickerColor, 0.1f, false, Color.grey);
            GUI.Label(new Rect((float)((double)this.position.x + 5.0), (float)((double)this.position.y + (double)this.knobSize.y / 2.0 - 8.0), this.position.width, 20f), value.ToString("0") + "°");
            return this.value;
        }

        static Material knobMaterial;

        private static void CreateKnobMaterial()
        {
            if ((bool)((UnityEngine.Object)knobMaterial))
                return;
            knobMaterial = new Material(AssetDatabase.GetBuiltinExtraResource(typeof(Shader), "Internal-GUITextureClip.shader") as Shader);
            knobMaterial.hideFlags = HideFlags.HideAndDontSave;
            knobMaterial.mainTexture = (Texture)EditorGUIUtility.Load("Assets/Editor/Knob.png");
            knobMaterial.name = "Knob Material";
            if ((UnityEngine.Object)knobMaterial.mainTexture == (UnityEngine.Object)null)
                Debug.Log((object)"Did not find 'Assets/Editor/Knob.png'");
        }

        private Vector3 GetUVForPoint(Vector3 point)
        {
            return new Vector3((point.x - this.position.x) / this.knobSize.x, (float)(((double)point.y - (double)this.position.y - (double)this.knobSize.y) / -(double)this.knobSize.y));
        }

        private void VertexPointWithUV(Vector3 point)
        {
            GL.TexCoord(this.GetUVForPoint(point));
            GL.Vertex(point);
        }

        private void DrawValueArc(float angle, Color col, float wide, bool clear, Color clearColor)
        {
            if (Event.current.type != EventType.Repaint)
                return;
            CreateKnobMaterial();
            knobMaterial.SetPass(0);
            if (clear)
            {
                GL.Begin(GL.QUADS);
                GL.Color(clearColor);
                this.VertexPointWithUV(new Vector3(this.position.x, this.position.y, 0.0f));
                this.VertexPointWithUV(new Vector3(this.position.x + this.knobSize.x, this.position.y, 0.0f));
                this.VertexPointWithUV(new Vector3(this.position.x + this.knobSize.x, this.position.y + this.knobSize.y, 0.0f));
                this.VertexPointWithUV(new Vector3(this.position.x, this.position.y + this.knobSize.y, 0.0f));
                GL.End();
            }

            GL.Begin(GL.TRIANGLES);
            GL.Color(col * (!GUI.enabled ? 0.5f : 1f));

            Vector3 pointCenter = new Vector3(this.position.x + this.knobSize.x / 2f, this.position.y + this.knobSize.y / 2f, 0.0f);
            float ang;
            for (ang = -wide; ang < wide - 0.1f; ang += 0.1f)
            {
                this.VertexPointWithUV(pointCenter);
                this.VertexPointWithUV(new Vector3(position.x + 20 + Mathf.Cos(angle + ang) * 20, position.y + 20 + Mathf.Sin(angle + ang) * 20, 0));
                this.VertexPointWithUV(new Vector3(position.x + 20 + Mathf.Cos(angle + ang + 0.1f) * 20, position.y + 20 + Mathf.Sin(angle + ang + 0.1f) * 20, 0));
            }
            this.VertexPointWithUV(pointCenter);
            this.VertexPointWithUV(new Vector3(position.x + 20 + Mathf.Cos(angle + ang - 0.1f) * 20, position.y + 20 + Mathf.Sin(angle + ang - 0.1f) * 20, 0));
            this.VertexPointWithUV(new Vector3(position.x + 20 + Mathf.Cos(angle + wide) * 20, position.y + 20 + Mathf.Sin(angle + wide) * 20, 0));

            GL.End();
        }
    }
    #endregion

    #endregion

    #region GRID

    #region CONSTRUCTOR
    public static List<Vector2> Grid(List<Vector2> value, List<int> angle, List<Color> col, params GUILayoutOption[] options)
    {
        Rect pos = EditorGUILayout.GetControlRect(options);
        return new GridContext(pos, value, angle, col).GetValue();
    }
    #endregion

    #region INTERNAL
    private class GridState
    {
        public bool isDragging;
        public int current;
    }

    private class GridContext
    {
        int id;
        Rect position;

        List<Vector2> value;
        List<int> angle;
        List<Color> colors;

        public GridContext(Rect position, List<Vector2> value, List<int> angle, List<Color> colors)
        {
            this.id = GUIUtility.GetControlID(GetType().GetHashCode(), FocusType.Passive, position);
            this.position = position;

            this.value = value;
            this.angle = angle;
            this.colors = colors;
        }

        public List<Vector2> GetValue()
        {
            switch (Event.current.type)
            {
                case EventType.MouseDown:
                    return this.OnMouseDown();
                case EventType.MouseUp:
                    return this.OnMouseUp();
                case EventType.MouseDrag:
                    return this.OnMouseDrag();
                case EventType.Repaint:
                    return this.OnRepaint();
                default:
                    return this.value;
            }
        }

        public GridState state()
        {
            return (GridState)GUIUtility.GetStateObject(typeof(GridState), this.id);
        }

        private List<Vector2> OnMouseDown()
        {
            if (!this.position.Contains(Event.current.mousePosition))
                return this.value;

            this.state().current = -1;
            for (int i = 0; i < value.Count; i++)
            {
                if(new Rect(position.x + value[i].x - 14, position.y + value[i].y - 14, 28, 28).Contains(Event.current.mousePosition))
                {
                    this.state().current = i;
                    break;
                }
            }
            if(this.state().current == -1)
                return this.value;

            GUIUtility.hotControl = this.id;
            this.state().isDragging = true;
            GUI.changed = true;
            Event.current.Use();

            value[this.state().current] = Event.current.mousePosition - new Vector2(position.x, position.y);

            return value;
        }

        private List<Vector2> OnMouseUp()
        {
            if (GUIUtility.hotControl == this.id)
            {
                Event.current.Use();
                GUIUtility.hotControl = 0;
                state().isDragging = false;
                this.state().current = -1;
            }
            return value;
        }

        private List<Vector2> OnMouseDrag()
        {
            if (GUIUtility.hotControl != this.id)
                return this.value;
            if (!state().isDragging || this.state().current == -1)
                return this.value;
            GUI.changed = true;
            Event.current.Use();
            value[this.state().current] = Event.current.mousePosition - new Vector2(position.x, position.y);

            return value;
        }

        public static Material pawnMaterial;
        public static Material pawnColorMaterial;
        public static Material wireMaterial;

        public static void CreateMaterial()
        {
            if ((bool)((UnityEngine.Object)pawnMaterial))
                return;
            pawnMaterial = new Material(AssetDatabase.GetBuiltinExtraResource(typeof(Shader), "Internal-GUITextureClip.shader") as Shader);
            pawnMaterial.hideFlags = HideFlags.HideAndDontSave;
            pawnMaterial.mainTexture = (Texture)EditorGUIUtility.Load("Assets/Editor/Pawn.png");
            pawnMaterial.name = "Pawn Material";
            if ((UnityEngine.Object)pawnMaterial.mainTexture == (UnityEngine.Object)null)
                Debug.Log((object)"Did not find 'Assets/Editor/Pawn.png'");

            pawnColorMaterial = new Material(AssetDatabase.GetBuiltinExtraResource(typeof(Shader), "Internal-GUITextureClip.shader") as Shader);
            pawnColorMaterial.hideFlags = HideFlags.HideAndDontSave;
            pawnColorMaterial.mainTexture = (Texture)EditorGUIUtility.Load("Assets/Editor/PawnColor.png");
            pawnColorMaterial.name = "Pawn Color Material";
            if ((UnityEngine.Object)pawnColorMaterial.mainTexture == (UnityEngine.Object)null)
                Debug.Log((object)"Did not find 'Assets/Editor/PawnColor.png'");

            wireMaterial = (Material)EditorGUIUtility.LoadRequired("SceneView/2DHandleLines.mat");
            wireMaterial.SetInt("_HandleZTest", (int)CompareFunction.Always);
        }

        private Vector3 GetUVForPoint(Vector3 point, Vector3 pos) {
            return new Vector3((point.x - pos.x + 18) / 36f, (point.y - pos.y + 18) / 36f);
        }

        private void VertexPointWithUV(Vector3 point, Vector3 pos) {
            GL.TexCoord(this.GetUVForPoint(point, pos));
            GL.Vertex(point);
        }

        private void drawPawn(Vector2 pos, float angle, Color col, string text)
        {
            pawnMaterial.SetPass(0);
            GL.Begin(GL.QUADS);
            GL.Color(Color.white);
            GL.TexCoord(new Vector2(0, 0));
            GL.Vertex(new Vector3(pos.x - 18, pos.y - 18, 0));
            GL.TexCoord(new Vector2(1, 0));
            GL.Vertex(new Vector3(pos.x + 18, pos.y - 18, 0));
            GL.TexCoord(new Vector2(1, 1));
            GL.Vertex(new Vector3(pos.x + 18, pos.y + 18, 0));
            GL.TexCoord(new Vector2(0, 1));
            GL.Vertex(new Vector3(pos.x - 18, pos.y + 18, 0));
            GL.End();


            GL.Begin(GL.TRIANGLES);
            GL.Color(Color.red);
            //GL.TexCoord(new Vector2(0.5f, 0.5f));
            //GL.Vertex(new Vector3(pos.x, pos.y, 0));

            VertexPointWithUV(new Vector3(pos.x + Mathf.Cos(angle) * 10, pos.y + Mathf.Sin(angle) * 10, 0), pos);

            VertexPointWithUV(new Vector3(pos.x + Mathf.Cos(angle + 0.4f) * 18, pos.y + Mathf.Sin(angle + 0.4f) * 18, 0), pos);
            VertexPointWithUV(new Vector3(pos.x + Mathf.Cos(angle - 0.4f) * 18, pos.y + Mathf.Sin(angle - 0.4f) * 18, 0), pos);
            GL.End();


            pawnColorMaterial.SetPass(0);
            GL.Begin(GL.QUADS);
            GL.Color(col);
            GL.TexCoord(new Vector2(0, 0));
            GL.Vertex(new Vector3(pos.x - 18, pos.y - 18, 0));
            GL.TexCoord(new Vector2(1, 0));
            GL.Vertex(new Vector3(pos.x + 18, pos.y - 18, 0));
            GL.TexCoord(new Vector2(1, 1));
            GL.Vertex(new Vector3(pos.x + 18, pos.y + 18, 0));
            GL.TexCoord(new Vector2(0, 1));
            GL.Vertex(new Vector3(pos.x - 18, pos.y + 18, 0));
            GL.End();

            GUIStyle style = GUIStyle.none;
            style.alignment = TextAnchor.MiddleCenter;
            GUI.Label(new Rect(pos.x - 18f, pos.y - 18f, 36f, 36f), text, style);
        }

        private List<Vector2> OnRepaint()
        {
            CreateMaterial();
            GUI.BeginClip(position);
            GUI.Box(new Rect(0, 0, position.width, position.height), GUIContent.none, (GUIStyle)"CurveEditorBackground");

            wireMaterial.SetPass(0);
            GL.PushMatrix();
            GL.Begin(1);
            GL.Color(new Color(0.0f, 0.0f, 0.0f, 0.15f));
            DrawLine(new Vector2(75,0), new Vector2(75, 250));
            GL.Color(new Color(0.0f, 0.0f, 0.0f, 0.1f));
            for (int i = 1; i < 25; i++)
                DrawLine(new Vector2(0, i * 10), new Vector2(150, i * 10));
            for (int i = 1; i <= 15; i++)
                DrawLine(new Vector2(i * 10 - 5, 0), new Vector2(i * 10 - 5, 250));
            GL.End();
            GL.PopMatrix();

            for (int i = 0; i < value.Count; i++)
                drawPawn(value[i], (angle[i] / 360f) * Mathf.PI * 2f - Mathf.PI / 2f, colors[i], "" + (char)('A' + i));
            
            GUI.EndClip();
            return this.value;
        }

        private void DrawLine(Vector2 p1, Vector2 p2)
        {
            GL.Vertex((Vector3)p1);
            GL.Vertex((Vector3)p2);
        }
    }
    #endregion

    #endregion
}