using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using Corelib.Utils;
using VoxelEngine;

namespace World
{
    public class WorldDebugOverlay : System.IDisposable
    {
        private enum ToolMode
        {
            AddVoxel,
            RemoveVoxel,
            FillBox,
            CarveBox,
            FillSphere,
            CarveSphere
        }

        private ToolMode mode;
        private Vector3 position = Vector3.zero;
        private Vector3 boxCenter = Vector3.zero;
        private Vector3 boxSize = Vector3.one;
        private Vector3 sphereCenter = Vector3.zero;
        private float sphereRadius = 1f;

        private readonly BoxBoundsHandle boxHandle = new();
        private readonly SphereBoundsHandle sphereHandle = new();

        private GUIContent[] modeContents;
        private Rect overlayRect = new(10, 20, 250f, 150f);
        private GUIStyle overlayStyle;

        private bool visible;

        public bool Visible
        {
            get => visible;
            set
            {
                if (visible == value) return;
                visible = value;
                if (visible)
                {
                    SceneView.duringSceneGui += OnSceneGUI;
                    CreateModeContents();
                }
                else
                {
                    SceneView.duringSceneGui -= OnSceneGUI;
                }
            }
        }

        public void Dispose()
        {
            Visible = false;
        }

        private void DrawOverlay(int id)
        {
            // if (world != null)
            // {
            //     EditorGUILayout.LabelField("World", world.name);
            // }
            // else
            // {
            //     EditorGUILayout.LabelField("World", "None");
            // }

            var newMode = (ToolMode)GUILayout.Toolbar((int)mode, modeContents, GUILayout.Height(EditorGUIUtility.singleLineHeight));
            if (newMode != mode)
            {
                mode = newMode;
                FocusOnHandle();
            }

            switch (mode)
            {
                case ToolMode.AddVoxel:
                case ToolMode.RemoveVoxel:
                    position = EditorGUILayout.Vector3Field("Position", position);
                    break;
                case ToolMode.FillBox:
                case ToolMode.CarveBox:
                    boxCenter = EditorGUILayout.Vector3Field("Center", boxCenter);
                    boxSize = EditorGUILayout.Vector3Field("Size", boxSize);
                    break;
                case ToolMode.FillSphere:
                case ToolMode.CarveSphere:
                    sphereCenter = EditorGUILayout.Vector3Field("Center", sphereCenter);
                    sphereRadius = EditorGUILayout.FloatField("Radius", sphereRadius);
                    break;
            }

            if (GUILayout.Button("Apply", GUILayout.Height(EditorGUIUtility.singleLineHeight)))
                ApplyTool();

            GUI.DragWindow(new Rect(0, 0, overlayRect.width, 20));
        }

        private void OnSceneGUI(SceneView sceneView)
        {
            if (!WorldSystem.HasField()) return;

            Event e = Event.current;
            if (e.type == EventType.KeyDown)
            {
                if (e.keyCode >= KeyCode.Alpha1 && e.keyCode <= KeyCode.Alpha6)
                {
                    mode = (ToolMode)((int)e.keyCode - (int)KeyCode.Alpha1);
                    SceneView.RepaintAll();
                    e.Use();
                }
                else if (e.keyCode == KeyCode.Return || e.keyCode == KeyCode.KeypadEnter)
                {
                    ApplyTool();
                    e.Use();
                }
            }

            Handles.color = Color.yellow;
            switch (mode)
            {
                case ToolMode.AddVoxel:
                case ToolMode.RemoveVoxel:
                    EditorGUI.BeginChangeCheck();
                    position = Handles.PositionHandle(position, Quaternion.identity);
                    Handles.DrawWireCube(Vector3Int.RoundToInt(position) + Vector3.one * 0.5f, Vector3.one);
                    Handles.Label(position, "Voxel");
                    if (EditorGUI.EndChangeCheck()) SceneView.RepaintAll();
                    break;
                case ToolMode.FillBox:
                case ToolMode.CarveBox:
                    EditorGUI.BeginChangeCheck();
                    boxCenter = Handles.PositionHandle(boxCenter, Quaternion.identity);
                    boxHandle.center = boxCenter;
                    boxHandle.size = boxSize;
                    boxHandle.DrawHandle();
                    if (EditorGUI.EndChangeCheck())
                    {
                        boxCenter = SnapVector(boxHandle.center);
                        boxSize = SnapVector(boxHandle.size);
                        SceneView.RepaintAll();
                    }
                    break;
                case ToolMode.FillSphere:
                case ToolMode.CarveSphere:
                    EditorGUI.BeginChangeCheck();
                    sphereCenter = Handles.PositionHandle(sphereCenter, Quaternion.identity);
                    sphereHandle.center = sphereCenter;
                    sphereHandle.radius = sphereRadius;
                    sphereHandle.DrawHandle();
                    if (EditorGUI.EndChangeCheck())
                    {
                        sphereCenter = SnapVector(sphereHandle.center);
                        sphereRadius = sphereHandle.radius;
                        SceneView.RepaintAll();
                    }
                    break;
            }

            Handles.BeginGUI();
            if (overlayStyle == null)
                overlayStyle = new GUIStyle(GUI.skin.window);

            Rect windowRect = new(overlayRect);
            windowRect.y = sceneView.position.height - (overlayRect.y + overlayRect.height);

            GUI.Box(windowRect, GUIContent.none, overlayStyle);
            GUI.Window(122143, windowRect, DrawOverlay, GUIContent.none, GUIStyle.none);
            Handles.EndGUI();
        }

        private void ApplyTool()
        {
            if (!WorldSystem.HasField()) return;

            switch (mode)
            {
                case ToolMode.AddVoxel:
                    WorldSystem.AddVoxel(Vector3Int.RoundToInt(position));
                    break;
                case ToolMode.RemoveVoxel:
                    WorldSystem.RemoveVoxel(Vector3Int.RoundToInt(position));
                    break;
                case ToolMode.FillBox:
                    WorldSystem.FillBox(new PBox(boxCenter, boxSize));
                    break;
                case ToolMode.CarveBox:
                    WorldSystem.CarveBox(new PBox(boxCenter, boxSize));
                    break;
                case ToolMode.FillSphere:
                    WorldSystem.FillSphere(new PSphere(sphereCenter, sphereRadius));
                    break;
                case ToolMode.CarveSphere:
                    WorldSystem.CarveSphere(new PSphere(sphereCenter, sphereRadius));
                    break;
            }

            SceneView.RepaintAll();
        }


        private void CreateModeContents()
        {
            modeContents = new[]
            {
                new GUIContent("", EditorGUIUtility.IconContent("Toolbar Plus").image, "Add Voxel"),
                new GUIContent("", EditorGUIUtility.IconContent("Toolbar Minus").image, "Remove Voxel"),
                new GUIContent("+", EditorGUIUtility.IconContent("PreMatCube").image, "Fill Box"),
                new GUIContent("-", EditorGUIUtility.IconContent("PreMatCube").image, "Carve Box"),
                new GUIContent("+", EditorGUIUtility.IconContent("SphereCollider Icon").image, "Fill Sphere"),
                new GUIContent("-", EditorGUIUtility.IconContent("d_SphereCollider Icon").image, "Carve Sphere")
            };
        }


        private void FocusOnHandle()
        {
            SceneView sv = SceneView.lastActiveSceneView;
            if (sv == null) return;

            Vector3 center;
            Vector3 size;
            switch (mode)
            {
                case ToolMode.AddVoxel:
                case ToolMode.RemoveVoxel:
                    center = Vector3Int.RoundToInt(position) + Vector3.one * 0.5f;
                    size = Vector3.one;
                    break;
                case ToolMode.FillBox:
                case ToolMode.CarveBox:
                    center = boxCenter;
                    size = boxSize;
                    break;
                case ToolMode.FillSphere:
                case ToolMode.CarveSphere:
                    center = sphereCenter;
                    size = Vector3.one * sphereRadius * 2f;
                    break;
                default:
                    center = Vector3.zero;
                    size = Vector3.one;
                    break;
            }

            sv.Frame(new Bounds(center, size), false);
        }

        private static Vector3 SnapVector(Vector3 value)
        {
            return new Vector3(
                Mathf.Round(value.x),
                Mathf.Round(value.y),
                Mathf.Round(value.z)
            );
        }
    }
}
