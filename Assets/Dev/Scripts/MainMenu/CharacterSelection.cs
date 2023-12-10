using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelection : MonoBehaviour
{
    [SerializeField] private List<GameObject> Characters = new List<GameObject>();
    [SerializeField] private int DefaultCharacterIndex;
    private int SelectedCharacterIndex;
    [SerializeField] private List<string> CharacterNames = new List<string>();

    public static CharacterSelection Instance { get; private set; }
    private void Awake()
    {
        Instance = this;
        SelectedCharacterIndex = PlayerPrefs.GetInt("SelectedCharacterIndex", DefaultCharacterIndex);
        SetCharacter();
    }

    private void SetCharacter()
    {
        for (int i = 0; i < Characters.Count; i++)
        {
            Characters[i].SetActive(false);
        }
        Characters[SelectedCharacterIndex].SetActive(true);
    }

    public string SelectedCharacterName()
    {
        return CharacterNames[SelectedCharacterIndex];
    }

    public void NextCharacter()
    {
        SelectedCharacterIndex++;
        if (SelectedCharacterIndex > Characters.Count - 1)
            SelectedCharacterIndex = 0;

        SetCharacter();
    }

    public void PreviousCharacter()
    {
        SelectedCharacterIndex--;
        if (SelectedCharacterIndex < 0)
            SelectedCharacterIndex = Characters.Count - 1;

        SetCharacter();
    }

    public void StartGame()
    {
        PlayerPrefs.SetInt("SelectedCharacterIndex", SelectedCharacterIndex);
        SceneManager.LoadScene("Game");
    }

}
