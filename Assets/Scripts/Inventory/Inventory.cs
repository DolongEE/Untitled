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

    public void AcquireItem(Item _item)
    {
        items.Add(_item);
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (itemSlots[i].Item == null)
            {
                itemSlots[i].AddItem(_item);
                return;
            }
        }
    }

    public void ReturnItem(Item _item)
    {
        items.Remove(_item);
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (itemSlots[i].Item == _item)
            {
                itemSlots[i].RemoveItem();
                return;
            }
        }
    }
}