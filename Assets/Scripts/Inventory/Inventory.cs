using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using static UnityEditor.Progress;

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
            if (item.ID == items[i].ID)
                return true;
        }

        return false;
    }

    public int ItemCount(string itemId)
    {
        return items.FindIndex(x => x.ID == itemId);
    }

    public bool CanAddItem(Item item, int amount)
    {
        int freeSpaces = 0;

        foreach (ItemSlot itemSlot in itemSlots)
        {
            if (itemSlot.Item == null || itemSlot.Item.ID == item.ID)
            {
                //freeSpaces += item.MaximumStacks - itemSlot.Amount;
            }
        }
        return freeSpaces >= amount;
    }

    public virtual Item RemoveItem(string itemID)
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            Item item = itemSlots[i].Item;
            if (item != null && item.ID == itemID)
            {
                itemSlots[i].Amount--;
                return item;
            }
        }
        return null;
    }
    public virtual bool AddItem(Item item)
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (itemSlots[i].CanAddStack(item))
            {
                itemSlots[i].Item = item;
                itemSlots[i].Amount++;
                return true;
            }
        }

        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (itemSlots[i].Item == null)
            {
                itemSlots[i].Item = item;
                itemSlots[i].Amount++;
                return true;
            }
        }
        return false;
    }
}