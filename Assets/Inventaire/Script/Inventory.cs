using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    [SerializeField]
    private List<ItemData> content = new List<ItemData>();
    
    [SerializeField]
    private GameObject inventoryPanel;

    [SerializeField]
    private Transform inventorySlotsParent;

    const int INVENTORY_SIZE = 15;

    private void Start()
    {
        RefreshContent();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        }
    }
    public void AddItem(ItemData item)
    {
        content.Add(item);
        RefreshContent();
    }
    private void RefreshContent()
    {
        int slotCount = inventorySlotsParent.childCount;
        for (int i = 0; i < slotCount; i++)
        {
            Slot current_slot = inventorySlotsParent.GetChild(i).GetComponent<Slot>();
            if (i < content.Count && content[i] != null)
            {
                current_slot.item = content[i];
                current_slot.itemVisual.sprite = content[i].visual;
                current_slot.itemVisual.enabled = true;
            }
            else
            {
                current_slot.item = null;
                current_slot.itemVisual.sprite = null;
                current_slot.itemVisual.enabled = false; 
            }
        }
    }

    public bool isFull()
    {
        return INVENTORY_SIZE == content.Count;
    }

    public void UseHealthItem(GameObject player)
    {
        for (int i = 0; i < content.Count; i++)
        {
            if (content[i] is Heal_Item potion)
            {
                potion.Use(player);
                content.RemoveAt(i);
                RefreshContent();
                break;
            }
        }
    }
    
}
