using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentInventory : MonoBehaviour
{
    public List<EquippableItem> equipItems;
    [SerializeField] private Transform equipmentBag;
    [SerializeField] private EquipmentSlot[] equipSlots;

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

    public void AcquireItem(EquippableItem _item)
    {
        equipItems.Add(_item);
        for (int i = 0; i < equipSlots.Length; i++)
        {
            if (equipSlots[i].Item == null)
            {
                equipSlots[i].SetItemImage(_item);
                return;
            }
        }
    }

    public void ReturnItem(EquippableItem _item)
    {
        equipItems.Remove(_item);
        for (int i = 0; i < equipSlots.Length; i++)
        {
            if (equipSlots[i].Item == _item)
            {
                equipSlots[i].RemoveItemImage();
                return;
            }
        }
    }
}