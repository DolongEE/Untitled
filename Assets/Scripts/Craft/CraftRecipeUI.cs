using System.Collections.Generic;
using UnityEngine;

public class CraftRecipeUI : MonoBehaviour
{
    [SerializeField] RectTransform arrowParent;
    [SerializeField] private ItemSlot[] itemSlots;

    private CraftRecipeSO craftingRecipe;
    public CraftRecipeSO CraftingRecipe
    {
        get { return craftingRecipe; }
        set { SetCraftingRecipe(value); }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        itemSlots = GetComponentsInChildren<ItemSlot>(includeInactive: true);
    }
#endif

    public void OnCraftButtonClick()
    {
        if (craftingRecipe != null && Managers.INVENTORY.inventory != null)
        {
            craftingRecipe.Craft(Managers.INVENTORY.inventory);
        }
    }

    private void SetCraftingRecipe(CraftRecipeSO newCraftingRecipe)
    {
        craftingRecipe = newCraftingRecipe;

        if (craftingRecipe != null)
        {
            int slotIndex = 0;
            slotIndex = SetSlots(craftingRecipe.Materials, slotIndex);
            arrowParent.SetSiblingIndex(slotIndex);
            slotIndex = SetSlots(craftingRecipe.Results, slotIndex);

            for (int i = slotIndex; i < itemSlots.Length; i++)
            {
                itemSlots[i].transform.parent.gameObject.SetActive(false);
            }

            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        } 
    }

    private int SetSlots(IList<ItemAmount> itemAmountList, int slotIndex)
    {
        for (int i = 0; i < itemAmountList.Count; i++, slotIndex++)
        {
            ItemAmount itemAmount = itemAmountList[i];
            ItemSlot itemSlot = itemSlots[slotIndex];
             
            itemSlot.Item = itemAmount.Item;
            itemSlot.Amount = itemAmount.Amount;
            itemSlot.transform.parent.gameObject.SetActive(true);            
        }
        return slotIndex; 
    }

}
