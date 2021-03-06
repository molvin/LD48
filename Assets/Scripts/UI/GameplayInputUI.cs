using GameplayAbilitySystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayInputUI : MonoBehaviour
{
    private Canvas m_Canvas;
    [Header("Interaction Buttons")]
    [SerializeField] private GameObject m_CharacterSelectionWindow;
    [SerializeField] private GameObject m_GamePlayUI;
    [SerializeField] private Button m_MoveActionButton;
    [SerializeField] private Button m_CharacterPortrait;
    [SerializeField] private Button m_EndTurnButton;
    [SerializeField] private Button m_SelectCharacterButton;
    [SerializeField] private Button m_DismissCharacterButton;
    [SerializeField] private HorizontalLayoutGroup m_ActionBar;
    [SerializeField] private HorizontalLayoutGroup m_InventoryBar;


    private Button[] m_ActionButtons;
    private Button[] m_InventoryButtons;
    private AbilitySystem Owner;

    private Dictionary<int, TypeTag> m_AbilityButtonMapping = new Dictionary<int, TypeTag>();
    private Dictionary<int, TypeTag> m_InventoryButtonMapping = new Dictionary<int, TypeTag>();

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        m_CharacterSelectionWindow.gameObject.SetActive(false);
        m_GamePlayUI.gameObject.SetActive(false);
        GameStateManager.Instance.OnHasDoneActionUpdate += UpdateActionButtons;
        GameStateManager.Instance.OnAvailableCharacter += CharacterTakeOver;
    }

    public void Setup()
    {
        m_Canvas = GetComponent<Canvas>();
        if (!m_Canvas)
            Debug.LogAssertion("There is no canvas");

 

        m_MoveActionButton.onClick.AddListener(delegate { SelectMove(); });
        m_CharacterPortrait.onClick.AddListener(delegate { ClickedCharacterPortrait(); });
        m_EndTurnButton.onClick.AddListener(delegate { SelectEndTurn(); });
        m_SelectCharacterButton.onClick.AddListener(delegate { SelectCharacter(); });
        m_DismissCharacterButton.onClick.AddListener(delegate { DismissCharacter(); });

        m_ActionButtons = m_ActionBar.GetComponentsInChildren<Button>();
        for (int i = 0; i < m_ActionButtons.Length; i++)
        {
            int j = i;
            m_ActionButtons[i].onClick.AddListener(delegate { SelectAction(j); });
        }


        m_InventoryButtons = m_InventoryBar.GetComponentsInChildren<Button>();
        for (int i = 0; i < m_InventoryButtons.Length; i++)
        {
            int j = i;
            m_InventoryButtons[i].onClick.AddListener(delegate { SelectInventoryItem(j); });

        }
    }

    private void OnDestroy()
    {
        GameStateManager.Instance.OnHasDoneActionUpdate -= UpdateActionButtons;
        GameStateManager.Instance.OnAvailableCharacter -= CharacterTakeOver;
    }

    public void LoadAbilities(AbilitySystem ability_owner)
    {
        m_GamePlayUI.gameObject.SetActive(true);
        Owner = ability_owner;
        List<TypeTag> type_tags = ability_owner.GetGrantedAbilityTypes();

        for (int i = 0; i < m_ActionButtons.Length; i++)
        {
            Button button = m_ActionButtons[i];
            if (type_tags.Count > i && i > 0)
            {
                TypeTag ability_tag = type_tags[i];
                button.GetComponent<Image>().sprite = ability_tag.Icon;
                button.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = ability_tag.Name;
                m_AbilityButtonMapping.Add(i, ability_tag);
            }
            else
            {
                button.gameObject.SetActive(false);
            }
        }
    }

    public void LoadInventory()
    {
        for (int i = 0; i < m_InventoryButtons.Length; i++)
        {
            m_InventoryButtons[i].gameObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Owner != null)
        {
            foreach (var AbilityButton in m_AbilityButtonMapping)
            {
                m_ActionButtons[AbilityButton.Key].enabled = !Owner.IsOnCooldown(AbilityButton.Value.GetType());
            }
        }
    }

    public void SelectMove()
    {
        GameStateManager.Instance.GoToActionState(TypeTag.MoveAbility);
    }

    public void SelectAction(int index)
    {
        if (!m_AbilityButtonMapping.ContainsKey(index))
            return;

        GameStateManager.Instance.GoToActionState(m_AbilityButtonMapping[index].GetType());
        Debug.Log("Clicked action: " + index);
    }
    public void SelectEndTurn()
    {
        GameStateManager.Instance.GoToEndTurnState();
    }

    public void SelectInventoryItem(int index)
    {
        Debug.Log("Clicked inventory item: " + index);
    }

    public void ClickedCharacterPortrait()
    {
        Debug.Log("Clicked character portrait");
    }

    public void UpdateActionButtons(bool has_done_ability)
    {
        m_EndTurnButton.interactable    = !has_done_ability;
        m_MoveActionButton.interactable = !has_done_ability;

        for (int i = 0; i < m_ActionButtons.Length; i++)
        {
            Button button = m_ActionButtons[i];
            if (!button.gameObject.activeSelf)
                continue;

            button.interactable = !has_done_ability;

        }
    }
    public void UpdateMoveButton(bool has_moved)
    {
        m_MoveActionButton.interactable = !has_moved;
    }

    private PlayableAgent temp_character;

    public void CharacterTakeOver(PlayableAgent player)
    {
        m_CharacterSelectionWindow.gameObject.SetActive(true);
        temp_character = player;
    }

    public void SelectCharacter()
    {
        Debug.Log("A button!!");
        m_CharacterSelectionWindow.gameObject.SetActive(false);
        GameStateManager.Instance.PlayerAgent = temp_character;
        GameStateManager.Instance.IsWaitingForSelection = false;
    }

    public void DismissCharacter()
    {
        m_CharacterSelectionWindow.gameObject.SetActive(false);
        GameStateManager.Instance.PlayerAgent = null;
        GameStateManager.Instance.IsWaitingForSelection = false;
    }
}
