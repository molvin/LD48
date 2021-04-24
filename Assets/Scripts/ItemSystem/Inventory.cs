using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public GameObject player;
    public GameObject itemPanel;

    public static Inventory instance;
    
    public List<Item> itemSlots = new List<Item>();

    public int maxSlots = 4;

    private void Start()
    {
        instance = this;
        UpdatePanelSlots();
    }

    public void UpdatePanelSlots()
    {
        int index = 0;
        foreach (Transform child in itemPanel.transform)
        {
            InventorySlotController iSlotCon = child.GetComponent<InventorySlotController>();

            if (index < itemSlots.Count)
                iSlotCon.item = itemSlots[index];
            else
                iSlotCon.item = null;

            iSlotCon.UpdateInformation();

            index++;
        }
    }

    public void Add(Item item)
    {
        if(itemSlots.Count < maxSlots)
            itemSlots.Add(item);

        UpdatePanelSlots();
    }

    public void Remove(Item item)
    {
        itemSlots.Remove(item);
        UpdatePanelSlots();
    }
}