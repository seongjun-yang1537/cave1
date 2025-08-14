using UnityEngine;
using UnityEditor;
using System.IO;

public class ModelGeneratorWindow : EditorWindow
{
    private string modelName = "";
    private string parentName = "";

    private const string TEMPLATE_FOLDER_PATH = "Assets/Scripts/Ingame/Entity/Editor/Template/Scripts";

    [MenuItem("Tools/Game/Model Generator")]
    public static void ShowWindow()
    {
        var window = GetWindow<ModelGeneratorWindow>("Model Generator");
        window.SetIcon();
    }

    private void OnEnable()
    {
        SetIcon();
    }

    private void OnGUI()
    {
        GUILayout.Label("Model Generator", EditorStyles.boldLabel);
        EditorGUILayout.HelpBox("새 모델을 생성합니다. 이름은 PascalCase(첫 글자 대문자)로 입력해주세요. 스크립트 내부의 대/소문자(Template/template)는 자동으로 변환됩니다.", MessageType.Info);

        EditorGUILayout.Space(10);

        modelName = EditorGUILayout.TextField("Model Name (PascalCase)", modelName);
        parentName = EditorGUILayout.TextField("Parent Name (PascalCase)", parentName);

        EditorGUILayout.Space(10);

        if (GUILayout.Button("Generate Model"))
        {
            GenerateModel();
        }
    }

    private void GenerateModel()
    {
        if (string.IsNullOrEmpty(modelName) || string.IsNullOrEmpty(parentName))
        {
            EditorUtility.DisplayDialog("Error", "Model Name과 Parent Name을 모두 입력해야 합니다.", "OK");
            return;
        }

        string targetDirectory = Path.GetDirectoryName(TEMPLATE_FOLDER_PATH);
        string newModelPath = Path.Combine(targetDirectory, modelName);

        if (Directory.Exists(newModelPath))
        {
            EditorUtility.DisplayDialog("Error", $"'{newModelPath}' 폴더가 이미 존재합니다. 다른 이름을 사용해주세요.", "OK");
            return;
        }

        if (!CopyAndPasteFolder(TEMPLATE_FOLDER_PATH, newModelPath))
        {
            EditorUtility.DisplayDialog("Error", $"템플릿 폴더 복사에 실패했습니다: {TEMPLATE_FOLDER_PATH}", "OK");
            return;
        }

        AssetDatabase.Refresh();

        string modelNamePascal = modelName;
        string modelNameCamel = ToCamelCase(modelName);
        string parentNamePascal = parentName;
        string parentNameCamel = ToCamelCase(parentName);

        ProcessDirectory(newModelPath, modelNamePascal, modelNameCamel, parentNamePascal, parentNameCamel);

        AssetDatabase.Refresh();

        this.ShowNotification(new GUIContent("Success!"));
        Debug.Log($"[Model Generator] '{modelName}' 모델을 성공적으로 생성했습니다.");
    }

    private void ProcessDirectory(string path, string modelNamePascal, string modelNameCamel, string parentNamePascal, string parentNameCamel)
    {
        string[] directories = Directory.GetDirectories(path, "*", SearchOption.AllDirectories);
        string[] files = Directory.GetFiles(path, "*.*", SearchOption.AllDirectories);

        foreach (string file in files)
        {
            if (Path.GetExtension(file) == ".cs")
            {
                string content = File.ReadAllText(file);
                content = content.Replace("Template", modelNamePascal);
                content = content.Replace("template", modelNameCamel);
                content = content.Replace("Parent", parentNamePascal);
                content = content.Replace("parent", parentNameCamel);
                File.WriteAllText(file, content);
            }
        }

        foreach (string file in files)
        {
            if (Path.GetFileName(file).Contains("Template"))
            {
                string newFileName = Path.GetFileName(file).Replace("Template", modelNamePascal);
                string newFilePath = Path.Combine(Path.GetDirectoryName(file), newFileName);
                AssetDatabase.MoveAsset(file, newFilePath);
            }
        }

        System.Array.Sort(directories, (a, b) => b.Length.CompareTo(a.Length));
        foreach (string dir in directories)
        {
            if (Path.GetFileName(dir).Contains("Template"))
            {
                string newDirName = Path.GetFileName(dir).Replace("Template", modelNamePascal);
                string newDirPath = Path.Combine(Path.GetDirectoryName(dir), newDirName);
                AssetDatabase.MoveAsset(dir, newDirPath);
            }
        }
    }

    private bool CopyAndPasteFolder(string sourcePath, string destPath)
    {
        if (!Directory.Exists(sourcePath)) return false;

        FileUtil.CopyFileOrDirectory(sourcePath, destPath);
        return true;
    }

    private string ToCamelCase(string str)
    {
        if (string.IsNullOrEmpty(str) || char.IsLower(str[0]))
        {
            return str;
        }
        return char.ToLowerInvariant(str[0]) + str.Substring(1);
    }

    private void SetIcon()
    {
        string iconName = EditorGUIUtility.isProSkin ? "d_UnityEditor.ConsoleWindow" : "UnityEditor.ConsoleWindow";
        titleContent.image = EditorGUIUtility.IconContent(iconName).image;
    }
}