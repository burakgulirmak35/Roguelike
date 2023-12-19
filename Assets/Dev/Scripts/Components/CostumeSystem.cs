using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CostumeSystem : MonoBehaviour
{
    [Header("Character")]
    [SerializeField] private List<GameObject> Characters = new List<GameObject>();

    public void SetCharacter()
    {
        for (int i = 0; i < Characters.Count; i++)
        {
            Characters[i].SetActive(false);
        }
        Characters[PlayerPrefs.GetInt("SelectedCharacterIndex")].SetActive(true);
    }

    public SkinnedMeshRenderer GetSkinnedMesh()
    {
        return Characters[PlayerPrefs.GetInt("SelectedCharacterIndex")].GetComponent<SkinnedMeshRenderer>();
    }
}
