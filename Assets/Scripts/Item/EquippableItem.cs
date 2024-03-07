using System.Collections;
using UnityEngine;

public enum EItemType
{
    Weapon,
    Helmet,
    Armor,
    Tools,
}

[CreateAssetMenu(menuName = "Items/Equipment Item")]
public class EquippableItem : Item
{
    [Header("Weapon Stats")]
    public int hp;
    public int damage;
    public int defense;
    public EItemType wItemType;

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