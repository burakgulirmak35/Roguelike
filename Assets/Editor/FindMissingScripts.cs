using UnityEditor;
using UnityEngine;

public class FindMissingScripts
{
    [MenuItem("Tools/Find Missing Scripts")]
    static void Find()
    {
        int found = 0;

        string[] prefabs = AssetDatabase.FindAssets("t:Prefab");
        foreach (string guid in prefabs)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            GameObject go = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            if (go == null) continue;

            foreach (Component c in go.GetComponentsInChildren<Component>(true))
            {
                if (c == null)
                {
                    Debug.LogWarning($"Missing Script in PREFAB: {path}", go);
                    found++;
                }
            }
        }

        string[] scenes = AssetDatabase.FindAssets("t:Scene");
        foreach (string guid in scenes)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Debug.Log($"Check scene manually if needed: {path}");
        }

        Debug.Log($"Scan complete. {found} missing script(s) found in prefabs.");
    }
}
