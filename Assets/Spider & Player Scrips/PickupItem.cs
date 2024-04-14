using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupItem : MonoBehaviour
{
    [SerializeField]
    private float pickupRange = 50f;
    
    [SerializeField]
    public PlayerUseItem player_use_item;

    [SerializeField]
    private LayerMask layerMask;

    [SerializeField]
    private GameObject pickupText;
    
    void Update()
    {
        RaycastHit hit;

        if(Physics.Raycast(transform.position, transform.forward, out hit, pickupRange,layerMask))
        {
            if(hit.transform.CompareTag("Item"))
            {
                pickupText.SetActive(true);
                if(Input.GetKeyDown(KeyCode.E))
                {
                    player_use_item.DoPickup(hit.transform.gameObject.GetComponent<Item>());
                }
            }

        }
        else{
            pickupText.SetActive(false);
        }
    }
}
