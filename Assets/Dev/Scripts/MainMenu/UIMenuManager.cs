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
    [SerializeField] private Button btn_SupportDev;
    [SerializeField] private TextMeshProUGUI txt_SelectedCharacterName;
    [SerializeField] private PanelPrivacy panel_Privacy;
    [SerializeField] private GameObject panel_CharacterSelection;

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
        btn_SupportDev.onClick.AddListener(() => AdsManager.Instance.ShowRewarded(null));

        bool accepted = panel_Privacy.IsAccepted();
        panel_Privacy.gameObject.SetActive(!accepted);
        panel_CharacterSelection.SetActive(accepted);

        panel_Privacy.OnAccepted += () => panel_CharacterSelection.SetActive(true);
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
