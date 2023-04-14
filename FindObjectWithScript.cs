using UnityEngine;
using UnityEditor;
using System.Linq;

public class FindObjectsWithScriptEditor : EditorWindow
{
    private MonoScript scriptToFind;

    [MenuItem("Tools/Find Objects with Specified Script")]
    public static void ShowWindow()
    {
        GetWindow<FindObjectsWithScriptEditor>("Find Objects with Specified Script");
    }

    private void OnGUI()
    {
        GUILayout.Label("Select a script to search for:", EditorStyles.boldLabel);
        scriptToFind = EditorGUILayout.ObjectField("Script to Find", scriptToFind, typeof(MonoScript), false) as MonoScript;

        if (GUILayout.Button("Find Objects"))
        {
            FindObjectsWithSpecifiedScript();
        }
    }

    private void FindObjectsWithSpecifiedScript()
    {
        if (scriptToFind == null)
        {
            Debug.LogWarning("No script file was selected.");
            return;
        }

        var type = scriptToFind.GetClass();
        var scriptName = scriptToFind.name;
        if (type == null)
        {
            Debug.LogError("Failed to find the specified script type.");
            return;
        }

        var objects = FindObjectsOfType(type).OfType<Component>();
        Debug.Log($"Found {objects.Count()} objects with {scriptName} script attached.");
        foreach (var obj in objects)
        {
            Debug.Log(obj.gameObject.name, obj.gameObject);
        }
    }
}
