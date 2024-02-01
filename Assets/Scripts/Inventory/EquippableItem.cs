using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Equipment Item")]
public class EquippableItem : Item
{
    [Header("Weapon Stats")]
    public int damage;
    public int defense;
}