using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CraftWindow : MonoBehaviour
{
    [Header("References")]
    [SerializeField] CraftRecipeUI recipeUIPrefab;
    [SerializeField] RectTransform recipeUIParent;
    [SerializeField] List<CraftRecipeUI> craftingRecipeUIs;

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
        recipeUIParent.GetComponentsInChildren(includeInactive: true, result: craftingRecipeUIs); 
        UpdateCraftingRecipes();  
    }

    public void UpdateCraftingRecipes()
    {         
        for (int i = 0; i < CraftingRecipes.Count; i++)
        {
            if (craftingRecipeUIs.Count == i)
            {
                craftingRecipeUIs.Add(Instantiate(recipeUIPrefab, recipeUIParent, false));
            }
            else if (craftingRecipeUIs[i] == null)
            {
                craftingRecipeUIs[i] = Instantiate(recipeUIPrefab, recipeUIParent, false);
            }

            //craftingRecipeUIs[i].inventory = Managers.INVENTORY.inventory;
            craftingRecipeUIs[i].CraftingRecipe = CraftingRecipes[i];
        } 

        for (int i = CraftingRecipes.Count; i < craftingRecipeUIs.Count; i++)
        {
            craftingRecipeUIs[i].CraftingRecipe = null;
        }

        
    }
}