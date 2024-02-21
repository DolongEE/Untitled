using System.Collections;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Equipment Item")]
public class EquippableItem : Item
{
    [Header("Weapon Stats")]
    public int damage;
    public int defense;

    public override Item GetCopy()
    {
        EquippableItem newItem = Instantiate(this);
        newItem.IsEquipped = false;
        newItem.damage = damage;
        newItem.defense = defense;

        return newItem;
    }

    public void Equip()
    {
        IsEquipped = true;
    }
    public void UnEquip()
    {
        IsEquipped= false;
    }
}