using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUseItem : MonoBehaviour
{
    [SerializeField]
    private Inventory inventory;
    
    public void DoPickup(Item item)
    {
        if(inventory.isFull())
        {
            return;
        }
        inventory.AddItem(item.itemData);
        Destroy(item.gameObject);
    }

    void Update()
    {
        HandleInput();
    }

    private void HandleInput()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            UseHealthItem();
        }
    }

    private void UseHealthItem()
    {
        inventory.UseHealthItem(gameObject);
    }
}