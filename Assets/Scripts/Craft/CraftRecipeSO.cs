using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ItemAmount
{
    public Item Item;
    [Range(1, 10)] public int Amount;
}

[CreateAssetMenu(fileName = "CraftRecipeSO", menuName = "ScriptableObjects/CraftRecipeSO", order = 5)]
public class CraftRecipeSO : ScriptableObject
{
    public List<ItemAmount> Materials;
    public List<ItemAmount> Results;

    public bool CanCraft(Inventory itemContainer)
    {
        return HasMaterials(itemContainer) && HasSpace(itemContainer);
    }

    private bool HasMaterials(Inventory itemContainer)
    {
        foreach (ItemAmount itemAmount in Materials)
        {
            if (itemContainer.items.Contains(itemAmount.Item) == false)
            {
                Debug.LogWarning("You don't have the required materials.");
                return false;
            }

            if (itemContainer.ItemCount(itemAmount.Item.ID) < itemAmount.Amount)
            {
                Debug.LogWarning("You don't have the required amount.");
                return false;
            }
        }
        return true;
    }

    private bool HasSpace(Inventory itemContainer)
    {
        foreach (ItemAmount itemAmount in Results)
        {
            if (!itemContainer.CanAddItem(itemAmount.Item, itemAmount.Amount))
            {
                Debug.LogWarning("Your inventory is full.");
                return false;
            }
        }
        return true;
    }

    public void Craft(Inventory itemContainer)
    {
        if (CanCraft(itemContainer))
        {            
            RemoveMaterials(itemContainer);
            AddResults(itemContainer);
        }
    }

    private void RemoveMaterials(Inventory itemContainer)
    {
        foreach (ItemAmount itemAmount in Materials)
        {
            for (int i = 0; i < itemAmount.Amount; i++)
            {
                itemContainer.ReturnItem(itemAmount.Item);
            }
        }
    }

    private void AddResults(Inventory itemContainer)
    {
        foreach (ItemAmount itemAmount in Results)
        {
            for (int i = 0; i < itemAmount.Amount; i++)
            {
                itemContainer.AcquireItem(itemAmount.Item.GetCopy());
            }
        }
    }
}
