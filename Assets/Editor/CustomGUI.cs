using UnityEditor;
using UnityEngine;

static class CustomGUI {
    public static float Knob(float value) {
        Rect pos = EditorGUILayout.GetControlRect(false, 40, GUILayout.Width(40));
        return new KnobContext(pos, value, 0, Color.black, Color.red).GetValue();
    }
    public static float Knob(float value, Color pickerColor) {
        Rect pos = EditorGUILayout.GetControlRect(false, 40, GUILayout.Width(40));
        return new KnobContext(pos, value, 0, pickerColor, Color.red).GetValue();
    }
    public static float Knob(float value, float spread) {
        Rect pos = EditorGUILayout.GetControlRect(false, 40, GUILayout.Width(40));
        return new KnobContext(pos, value, spread, Color.black, Color.red).GetValue();
    }
    public static float Knob(float value, float spread, Color pickerColor, Color rangeColor) {
        Rect pos = EditorGUILayout.GetControlRect(false, 40, GUILayout.Width(40));
        return new KnobContext(pos, value, spread, pickerColor, rangeColor).GetValue();
    }

    public static float Knob(Rect pos, float value) {
        return new KnobContext(pos, value, 0, Color.black, Color.red).GetValue();
    }
    public static float Knob(Rect pos, float value, Color pickerColor) {
        return new KnobContext(pos, value, 0, pickerColor, Color.red).GetValue();
    }
    public static float Knob(Rect pos, float value, float spread) {
        return new KnobContext(pos, value, spread, Color.black, Color.red).GetValue();
    }
    public static float Knob(Rect pos, float value, float spread, Color pickerColor, Color rangeColor) {
        return new KnobContext(pos, value, spread, pickerColor, rangeColor).GetValue();
    }

    private class KnobState {
        public bool isDragging;
    }

    private class KnobContext {
        int id;
        float value;
        Rect position;
        Vector2 knobSize;
        float spread;
        Color pickerColor;
        Color rangeColor;

        public KnobContext(Rect position, float value, float spread, Color pickerColor, Color rangeColor) {
            this.id = GUIUtility.GetControlID(GetType().GetHashCode(), FocusType.Passive, position);
            this.position = position;
            this.value = value;
            this.spread = spread;
            this.pickerColor = pickerColor;
            this.rangeColor = rangeColor;
            this.knobSize = new Vector2(40, 40);
        }

        public float GetValue() {
            switch (Event.current.type) {
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

        public KnobState state() {
            return (KnobState)GUIUtility.GetStateObject(typeof(KnobState), this.id);
        }

        private float OnMouseDown() {
            if (!this.position.Contains(Event.current.mousePosition))
                return this.value;
            GUIUtility.hotControl = this.id;
            KnobState state = (KnobState)GUIUtility.GetStateObject(typeof(KnobState), this.id);
            state.isDragging = true;
            Event.current.Use();
            float res = Mathf.Atan2((this.position.y + this.knobSize.y / 2f) - Event.current.mousePosition.y, (this.position.x + this.knobSize.x / 2f) - Event.current.mousePosition.x) / 3.14159f * 180f - 90;
            if (res <= -180) {
                res += 360;
            }
            return res;
        }

        private float OnMouseUp() {
            if (GUIUtility.hotControl == this.id) {
                Event.current.Use();
                GUIUtility.hotControl = 0;
                state().isDragging = false;
            }
            return value;
        }

        private float OnMouseDrag() {
            if (GUIUtility.hotControl != this.id)
                return this.value;
            if (!state().isDragging)
                return this.value;
            GUI.changed = true;
            Event.current.Use();
            float res = Mathf.Atan2((this.position.y + this.knobSize.y / 2f) - Event.current.mousePosition.y, (this.position.x + this.knobSize.x / 2f) - Event.current.mousePosition.x) / 3.14159f * 180f - 90;
            if (res <= -180) {
                res += 360;
            }
            return res;
        }

        private float MouseOffset() {
            return (this.position.x - Event.current.mousePosition.x);
        }

        private float OnRepaint() {
            this.DrawValueArc((float)((double)(value / 360f) * 3.14159274101257 * 2 + 3.14159274101257 / 2f * 3), rangeColor, (spread / 360f) * 3.14159274101257f, true, Color.grey);
            this.DrawValueArc((float)((double)(value / 360f) * 3.14159274101257 * 2 + 3.14159274101257 / 2f * 3), pickerColor, 0.1f, false, Color.grey);
            GUI.Label(new Rect((float)((double)this.position.x + 5.0), (float)((double)this.position.y + (double)this.knobSize.y / 2.0 - 8.0), this.position.width, 20f), value.ToString("0") + "°");
            return this.value;
        }

        static Material knobMaterial;

        private static void CreateKnobMaterial() {
            if ((bool)((UnityEngine.Object)knobMaterial))
                return;
            knobMaterial = new Material(AssetDatabase.GetBuiltinExtraResource(typeof(Shader), "Internal-GUITextureClip.shader") as Shader);
            knobMaterial.hideFlags = HideFlags.HideAndDontSave;
            knobMaterial.mainTexture = (Texture)EditorGUIUtility.Load("Assets/Editor/Knob.png");
            knobMaterial.name = "Knob Material";
            if ((UnityEngine.Object)knobMaterial.mainTexture == (UnityEngine.Object)null)
                Debug.Log((object)"Did not find 'Assets/Editor/Knob.png'");
        }
        
        private Vector3 GetUVForPoint(Vector3 point) {
            return new Vector3((point.x - this.position.x) / this.knobSize.x, (float)(((double)point.y - (double)this.position.y - (double)this.knobSize.y) / -(double)this.knobSize.y));
        }

        private void VertexPointWithUV(Vector3 point) {
            GL.TexCoord(this.GetUVForPoint(point));
            GL.Vertex(point);
        }

        private void DrawValueArc(float angle, Color col, float wide, bool clear, Color clearColor) {
            if (Event.current.type != EventType.Repaint)
                return;
            CreateKnobMaterial();
            knobMaterial.SetPass(0);
            if (clear) {
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
            for (ang = -wide; ang < wide - 0.1f; ang += 0.1f) {
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
}