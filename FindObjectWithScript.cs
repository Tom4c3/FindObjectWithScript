using UnityEngine;
using UnityEditor;
using System.Linq;

public class FindObjectsWithScriptEditor : EditorWindow
{
    private MonoScript scriptToFind;

    [MenuItem("Tools/Find Objects With Script")]
    public static void ShowWindow()
    {
        GetWindow<FindObjectsWithScriptEditor>("Find Objects With Script");
    }

    private void OnGUI()
    {
        EditorGUILayout.LabelField("Find objects with specified script attached:");
        scriptToFind = EditorGUILayout.ObjectField("Script to find:", scriptToFind, typeof(MonoScript), false) as MonoScript;

        if (GUILayout.Button("Find in Scene"))
        {
            FindObjectsWithSpecifiedScriptInScene();
        }

        if (GUILayout.Button("Find Prefabs"))
        {
            FindPrefabsWithSpecifiedScript();
        }

        if (GUILayout.Button("Find Objects in Assets"))
        {
            FindObjectsWithSpecifiedScriptInAssets();
        }
    }

    private void FindObjectsWithSpecifiedScriptInScene()
    {
        if (scriptToFind == null)
        {
            Debug.LogWarning("No script file was selected.");
            return;
        }

        var type = scriptToFind.GetClass();
        if (type == null)
        {
            Debug.LogError("Failed to find the specified script type.");
            return;
        }

        var objectsInScene = FindObjectsOfType(type);
        foreach (var obj in objectsInScene)
        {
            Debug.Log($"{obj.name} ({obj.GetType().Name})", obj);
        }
    }

    private void FindPrefabsWithSpecifiedScript()
    {
        if (scriptToFind == null)
        {
            Debug.LogWarning("No script file was selected.");
            return;
        }

        var type = scriptToFind.GetClass();
        if (type == null)
        {
            Debug.LogError("Failed to find the specified script type.");
            return;
        }

        string[] allPrefabs = AssetDatabase.FindAssets("t:Prefab");
        int foundCount = 0;

        foreach (string guid in allPrefabs)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject prefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (prefab == null) continue;

            var components = prefab.GetComponentsInChildren(type, true);
            if (components.Length > 0)
            {
                foundCount++;
                Debug.Log($"{prefab.name} ({path})", prefab);
            }
        }

        Debug.Log($"Found {foundCount} prefabs with {type.Name} script attached.");
    }

    private void FindObjectsWithSpecifiedScriptInAssets()
    {
        if (scriptToFind == null)
        {
            Debug.LogWarning("No script file was selected.");
            return;
        }

        var type = scriptToFind.GetClass();
        if (type == null)
        {
            Debug.LogError("Failed to find the specified script type.");
            return;
        }

        string[] allObjects = AssetDatabase.FindAssets("t:Object");
        int foundCount = 0;

        foreach (string guid in allObjects)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject obj = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (obj == null) continue;

            var components = obj.GetComponentsInChildren(type, true);
            if (components.Length > 0)
            {
                foundCount++;
                Debug.Log($"{obj.name} ({path})", obj);
            }
        }

        Debug.Log($"Found {foundCount} objects in Assets with {type.Name} script attached.");
    }
}
