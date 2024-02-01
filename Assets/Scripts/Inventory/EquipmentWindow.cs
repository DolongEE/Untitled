using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentWindow : MonoBehaviour
{
    public List<Item> equipItems;
    [SerializeField] private Transform equipmentSlotsParent;
    [SerializeField] private EquipmentSlot[] equipSlots;

    private void OnValidate()
    {
        equipSlots = equipmentSlotsParent.GetComponentsInChildren<EquipmentSlot>();
    }

    public void AcquireItem(Item _item)
    {
        int i = 0;
        if (Item.ItemType.Equipment != _item.itemType)
        {
            //장비가 아닐 경우 갯수 표기를 위한 함수
        }

        for (; i < equipSlots.Length; i++)
        {
            if (equipSlots[i].item == null)
            {
                equipSlots[i].AddItem(_item);
                return;
            }
        }
    }
    
    public void ReturnItem(Item _item)
    {
        for (int i = 0; i < equipSlots.Length; i++)
        {
            if (equipSlots[i].item == _item)
            {
                equipSlots[i].RemoveItem();
                return;
            }
        }
    }
}