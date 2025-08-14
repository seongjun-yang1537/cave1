using UnityEditor;
using UnityEngine;

public class WorldCurvedWorldWindow : EditorWindow
{
    private const string GLOBAL_SHADER_PARAM = "_WorldCurvedAmount";

    private const string PREF_KEY_EDITOR = "WorldCurvedWorldWindow_EditorValue";
    private const string PREF_KEY_RUNTIME = "WorldCurvedWorldWindow_RuntimeValue";

    private float editorValue;
    private float runtimeValue;

    [MenuItem("World/Curved World Window")]
    public static void ShowWindow()
    {
        var window = GetWindow<WorldCurvedWorldWindow>("Curved World");
        window.minSize = new Vector2(300, 150);
    }

    private void OnEnable()
    {
        editorValue = EditorPrefs.GetFloat(PREF_KEY_EDITOR, 0f);
        runtimeValue = EditorPrefs.GetFloat(PREF_KEY_RUNTIME, 0f);

        if (Application.isPlaying)
        {
            Shader.SetGlobalFloat(GLOBAL_SHADER_PARAM, runtimeValue);
        }
        else
        {
            Shader.SetGlobalFloat(GLOBAL_SHADER_PARAM, editorValue);
        }
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Curved World Settings", EditorStyles.boldLabel);
        EditorGUILayout.Space();

        // Editor Value
        EditorGUI.BeginChangeCheck();
        editorValue = EditorGUILayout.Slider("Editor Value", editorValue, 0f, 1f);
        if (EditorGUI.EndChangeCheck())
        {
            EditorPrefs.SetFloat(PREF_KEY_EDITOR, editorValue);
            if (!Application.isPlaying)
                Shader.SetGlobalFloat(GLOBAL_SHADER_PARAM, editorValue);
        }

        if (!Application.isPlaying && GUILayout.Button("Reset Editor Value"))
        {
            editorValue = 0f;
            EditorPrefs.SetFloat(PREF_KEY_EDITOR, 0f);
            Shader.SetGlobalFloat(GLOBAL_SHADER_PARAM, 0f);
        }

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Runtime Value", EditorStyles.boldLabel);

        // Runtime Value
        EditorGUI.BeginChangeCheck();
        runtimeValue = EditorGUILayout.Slider("Runtime Value", runtimeValue, 0f, 1f);
        if (EditorGUI.EndChangeCheck())
        {
            EditorPrefs.SetFloat(PREF_KEY_RUNTIME, runtimeValue);
            if (Application.isPlaying)
                Shader.SetGlobalFloat(GLOBAL_SHADER_PARAM, runtimeValue);
        }

        if (Application.isPlaying && GUILayout.Button("Reset Runtime Value"))
        {
            runtimeValue = 0f;
            EditorPrefs.SetFloat(PREF_KEY_RUNTIME, 0f);
            Shader.SetGlobalFloat(GLOBAL_SHADER_PARAM, 0f);
        }

        EditorGUILayout.Space(10);
        EditorGUILayout.HelpBox("Editor 모드에서는 'Editor Value'만 반영되고, Play 모드에서는 'Runtime Value'만 반영됩니다.", MessageType.Info);

        SceneView.RepaintAll();
    }
}
