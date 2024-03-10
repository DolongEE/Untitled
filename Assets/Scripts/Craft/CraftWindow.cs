using System.Collections.Generic;
using UnityEngine;

public class CraftWindow : MonoBehaviour
{
    [Header("References")]
    [SerializeField] CraftRecipeUI recipeUIPrefab;
    [SerializeField] RectTransform recipeUIParent;
    [SerializeField] List<CraftRecipeUI> craftingRecipeUI;

    [Header("Public Variables")]
    public List<CraftRecipeSO> CraftingRecipes;

    private void OnValidate()
    {
        Init();
    }

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        //recipeUIParent = GetComponentInChildren<ContentSizeFitter>().gameObject.GetComponent<RectTransform>();
        recipeUIParent.GetComponentsInChildren(includeInactive: true, result: craftingRecipeUI);
        UpdateCraftingRecipes();
    }

    public void UpdateCraftingRecipes()
    {
        for (int i = 0; i < CraftingRecipes.Count; i++)
        {
            if (craftingRecipeUI.Count == i)
            {
                craftingRecipeUI.Add(Instantiate(recipeUIPrefab, recipeUIParent, false));
            }
            else if (craftingRecipeUI[i] == null)
            {
                craftingRecipeUI[i] = Instantiate(recipeUIPrefab, recipeUIParent, false);
            }

            //craftingRecipeUIs[i].inventory = Managers.INVENTORY.inventory;
            craftingRecipeUI[i].CraftingRecipe = CraftingRecipes[i];
        }

        for (int i = CraftingRecipes.Count; i < craftingRecipeUI.Count; i++)
        {
            craftingRecipeUI[i].CraftingRecipe = null;
        }
    }
}