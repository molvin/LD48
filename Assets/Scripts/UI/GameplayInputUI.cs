using GameplayAbilitySystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayInputUI : MonoBehaviour
{
    private Canvas m_Canvas;
    [Header("Interaction Buttons")]
    [SerializeField] private Button m_MoveActionButton;
    [SerializeField] private Button m_CharacterPortrait;
    [SerializeField] private HorizontalLayoutGroup m_ActionBar;
    [SerializeField] private HorizontalLayoutGroup m_InventoryBar;

    private Button[] m_ActionButtons;
    private Button[] m_InventoryButtons;

    private Dictionary<int, TypeTag> m_AbilityButtonMapping = new Dictionary<int, TypeTag>();
    private Dictionary<int, TypeTag> m_InventoryButtonMapping = new Dictionary<int, TypeTag>();

    //private Dictionary<uint, Button> m_ActionButtons;

    // Start is called before the first frame update
    void Start()
    {
        m_Canvas = GetComponent<Canvas>();
        if (!m_Canvas)
            Debug.LogAssertion("There is no canvas");

        m_MoveActionButton.onClick.AddListener(delegate { SelectMove(); });
        m_CharacterPortrait.onClick.AddListener(delegate { ClickedCharacterPortrait(); });


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

        LoadAbilities(GameStateManager.Instance.PlayerAgent.AbilitySystem);
    }

    public void LoadAbilities(AbilitySystem ability_owner)
    {
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

    // Update is called once per frame
    void Update()
    {
        
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

    public void SelectInventoryItem(int index)
    {
        Debug.Log("Clicked inventory item: " + index);
    }

    public void ClickedCharacterPortrait()
    {
        Debug.Log("Clicked character portrait");
    }
}
