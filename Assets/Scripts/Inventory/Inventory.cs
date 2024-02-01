using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Item> items;
    [SerializeField] private Transform slotParent;
    [SerializeField] private ItemSlot[] itemSlots;

    private void Start()
    {
        InitRefreshSlot();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        itemSlots = slotParent.GetComponentsInChildren<ItemSlot>();
        InitRefreshSlot();
        //UnityEditor.EditorUtility.SetDirty(this);
    }
#endif

    public void InitRefreshSlot()
    {
        int i = 0;
        for (; i < itemSlots.Length; i++)
        {
            itemSlots[i].item = null;
        }
        for (; i < items.Count && i < itemSlots.Length; i++)
        {
            itemSlots[i].item = items[i];
        }
    }

    //public bool AddItem(Item _item)
    //{
    //    for (int i = 0; i < itemSlots.Length; i++)
    //    {
    //        if (itemSlots[i].item == _item)
    //        {
    //            itemSlots[i].item = _item;
    //            return true;
    //        }
    //    }
    //    return false;
    //}
    //public bool RemoveItem(Item _item)
    //{
    //    for (int i = 0; i < itemSlots.Length; i++)
    //    {
    //        if (itemSlots[i].item == _item)
    //        {
    //            itemSlots[i].item = null;
    //            return true;
    //        }
    //    }
    //    return false;
    //}

    public void AcquireItem(Item _item)
    {
        int i = 0;
        if (Item.ItemType.Equipment != _item.itemType)
        {
            //장비가 아닐 경우 갯수 표기를 위한 함수
        }

        for (; i < itemSlots.Length; i++)
        {
            if (itemSlots[i].item == null)
            {
                itemSlots[i].AddItem(_item);
                return;
            }
        }
    }

    public void ReturnItem(Item _item)
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (itemSlots[i].item == _item)
            {
                itemSlots[i].RemoveItem();
                return;
            }
        }
    }
}