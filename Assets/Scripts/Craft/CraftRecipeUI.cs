using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftRecipeUI : MonoBehaviour
{
    [SerializeField] private ItemSlot[] itemSlots;

    private void OnValidate()
    {
        itemSlots = GetComponentsInChildren<ItemSlot>();
    }

    void Start()
    {
        
    }

}
