using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public List<Item> items;
    [SerializeField] private Transform itemBag;
    [SerializeField] private ItemSlot[] itemSlots;

    private void Start()
    {
        InitRefreshSlot();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        itemBag = transform.Find("itemBag");
        itemSlots = itemBag.GetComponentsInChildren<ItemSlot>();
        //UnityEditor.EditorUtility.SetDirty(this);
    }
#endif

    private void InitRefreshSlot()
    {
        int i = 0;
        for (; i < itemSlots.Length; i++)
        {
            itemSlots[i].Item = null;
        }
        for (; i < items.Count && i < itemSlots.Length; i++)
        {
            itemSlots[i].Item = items[i];
        }
    }

    public bool AcquireItem(Item _item)
    {
        if (items.Count < itemSlots.Length)
        {
            if (CheckItemInInventory(_item))
            {
                for (int i = 0; i < itemSlots.Length; i++)
                {
                    if (_item.ID == itemSlots[i].Item.ID)
                    {
                        items.Add(_item);
                        itemSlots[i].Amount++;
                        itemSlots[i].RefreshAmount();
                        return true;
                    }
                }
            }
            else
            {
                for (int i = 0; i < itemSlots.Length; i++)
                {
                    if (itemSlots[i].Item == null)
                    {
                        items.Add(_item);
                        itemSlots[i].Amount++;
                        itemSlots[i].SetItemImage(_item);
                        return true;
                    }
                }
            }
        }

        return false;
    }

    public void ReturnItem(Item _item)
    {
        items.Remove(_item);
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (itemSlots[i].Item == _item)
            {
                if (CheckItemInInventory(_item))
                {
                    itemSlots[i].Amount--;
                    itemSlots[i].RefreshAmount();
                    break;
                }
                else
                {
                    itemSlots[i].Amount--;
                    itemSlots[i].RemoveItemImage();
                    break;
                }
            }
        }
    }

    public bool IsInventoryFull()
    {
        if (items.Count < itemSlots.Length)
            return false;
        else
            return true;
    }
    public bool CheckItemInInventory(Item item)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (item.ID == items[i].ID && item.itemType != Item.ItemType.Equipment)
                return true;
        }

        return false;
    }

    public int ItemCount(string itemId)
    {
        int count = 0;
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (count != 0) break;
            if (itemId == itemSlots[i].Item.ID)
            {
                count = itemSlots[i].Amount;                
            }                
        }
        return count;
    }

    public bool CanAddItem(Item item, int amount)
    {

        foreach (ItemSlot itemSlot in itemSlots)
        {
            if (itemSlot.Item == null || itemSlot.Item.ID == item.ID)
            {
                return true;
            }
        }
        return false;
    }
}