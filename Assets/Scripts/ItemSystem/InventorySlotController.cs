using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlotController : MonoBehaviour
{
    public Item item;

    private Text displayText;
    private Image displayImage;

    private void Start()
    {
        displayText = transform.Find("Text").GetComponent<Text>();
        displayImage = transform.Find("Image").GetComponent<Image>();
        
        UpdateInformation();
    }

    public void Use()
    {
        if (item)
            item.Use();
    }

    public virtual void UpdateInformation()
    {
        if (item)
        {
            displayText.text = item.itemName;
            displayImage.sprite = item.itemImage;
            displayImage.color = Color.white;
        } 
        else
        {
            displayText.text = "";
            displayImage.sprite = null;
            displayImage.color = Color.clear;
        }
    }
}