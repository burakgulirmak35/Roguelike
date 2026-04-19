using UnityEditor;
using UnityEngine;

public class DisableSoftParticles
{
    [MenuItem("Tools/Disable Soft Particles on All Materials")]
    static void DisableAll()
    {
        string[] guids = AssetDatabase.FindAssets("t:Material");
        int count = 0;

        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            Material mat = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (mat == null) continue;

            bool dirty = false;

            if (mat.HasProperty("_SoftParticlesEnabled") && mat.GetFloat("_SoftParticlesEnabled") != 0f)
            {
                mat.SetFloat("_SoftParticlesEnabled", 0f);
                dirty = true;
            }

            if (mat.IsKeywordEnabled("_SOFTPARTICLES_ON"))
            {
                mat.DisableKeyword("_SOFTPARTICLES_ON");
                dirty = true;
            }

            if (mat.IsKeywordEnabled("_FADING_ON"))
            {
                mat.DisableKeyword("_FADING_ON");
                dirty = true;
            }

            if (dirty)
            {
                EditorUtility.SetDirty(mat);
                count++;
            }
        }

        AssetDatabase.SaveAssets();
        Debug.Log($"[DisableSoftParticles] {count} material güncellendi.");
    }
}
