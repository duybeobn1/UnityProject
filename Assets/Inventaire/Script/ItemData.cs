using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName="Item",menuName="Items/New items")]
public class ItemData : ScriptableObject
{
    public string nameI;
    public string descriptionI;
    public Sprite visual;
    public GameObject prefab;

    public virtual void Use(GameObject user)
    {

    }
}
