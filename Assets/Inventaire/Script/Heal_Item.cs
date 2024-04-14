using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="Heal_Item", menuName="Items/Heal_Item")]
public class Heal_Item : ItemData
{
    [SerializeField]
    public int health;

    public override void Use(GameObject player)
    {
        PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        if (playerHealth != null)
        {
            playerHealth.Heal(health);
        }
    }
}