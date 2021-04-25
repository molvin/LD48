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


    //private Dictionary<uint, Button> m_ActionButtons;

    // Start is called before the first frame update
    void Start()
    {
        m_Canvas = GetComponent<Canvas>();
        if (!m_Canvas)
            Debug.LogAssertion("There is no canvas");

        m_MoveActionButton.onClick.AddListener(delegate { SelectMove(); });
        m_CharacterPortrait.onClick.AddListener(delegate { ClickedCharacterPortrait(); });


        Button[] action_buttons = m_ActionBar.GetComponentsInChildren<Button>();
        for (uint i = 0; i < action_buttons.Length; i++)
        {
            uint j = i;
            action_buttons[i].onClick.AddListener(delegate { SelectAction(j); });
        }
           

        Button[] inventory_buttons = m_InventoryBar.GetComponentsInChildren<Button>();
        for (uint i = 0; i < inventory_buttons.Length; i++)
        {
            uint j = i;
            inventory_buttons[i].onClick.AddListener(delegate { SelectInventoryItem(j); });

        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SelectMove()
    {
        Debug.Log("Selected Move");
    }

    public void SelectAction(uint index)
    {
        Debug.Log("Clicked action: " + index);
    }

    public void SelectInventoryItem(uint index)
    {
        Debug.Log("Clicked inventory item: " + index);
    }

    public void ClickedCharacterPortrait()
    {
        Debug.Log("Clicked character portrait");
    }
}
