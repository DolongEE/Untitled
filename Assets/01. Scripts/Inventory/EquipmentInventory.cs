using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentInventory : MonoBehaviour
{
    public List<EquippableItem> equipItems;
    [SerializeField] private Transform equipmentBag;
    [SerializeField] public EquipmentSlot[] equipSlots;

    private void Start()
    {
        InitRefreshSlot();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        equipmentBag = transform.Find("equipmentBag");
        equipSlots = equipmentBag.GetComponentsInChildren<EquipmentSlot>();
    }
#endif

    private void InitRefreshSlot()
    {
        int i = 0;
        for (; i < equipSlots.Length; i++)
        {
            equipSlots[i].Item = null;
        }
        for (; i < equipItems.Count && i < equipSlots.Length; i++)
        {
            equipSlots[i].Item = equipItems[i];
        }
    }

    public bool AcquireItem(EquippableItem _item)
    {
        if (equipItems.Count < equipSlots.Length)
        {
            equipItems.Add(_item);
            for (int i = 0; i < equipSlots.Length; i++)
            {
                if (equipSlots[i].equipmentType == _item.wItemType)
                {
                    equipSlots[i].SetItemImage(_item);
                    return true;
                }
            }
        }
        return false;
    }

    public bool ReturnItem(EquippableItem _item)
    {
        equipItems.Remove(_item);
        for (int i = 0; i < equipSlots.Length; i++)
        {
            if (equipSlots[i].Item == _item)
            {
                equipSlots[i].RemoveItemImage();
                return true;
            }
        }

        return false;
    }
}