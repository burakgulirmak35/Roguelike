using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIMenuManager : MonoBehaviour
{

    [SerializeField] private Button btn_NextCharacter;
    [SerializeField] private Button btn_PreviousCharacter;
    [SerializeField] private Button btn_Start;
    [SerializeField] private TextMeshProUGUI txt_SelectedCharacterName;

    public static UIMenuManager Instance { get; private set; }
    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        btn_NextCharacter.onClick.AddListener(BtnNextCharacter);
        btn_PreviousCharacter.onClick.AddListener(BtnPreviousCharacter);
        btn_Start.onClick.AddListener(CharacterSelection.Instance.StartGame);
    }

    private void BtnNextCharacter()
    {
        CharacterSelection.Instance.NextCharacter();
        ChangeName();
    }

    private void BtnPreviousCharacter()
    {
        CharacterSelection.Instance.PreviousCharacter();
        ChangeName();
    }

    private void ChangeName()
    {
        txt_SelectedCharacterName.text = CharacterSelection.Instance.SelectedCharacterName();
    }

}
