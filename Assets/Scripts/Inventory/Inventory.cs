using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static bool inventoryActivated = false;
    public static Inventory instance;
    public GameObject bag;

    public List<Item> items;

    [SerializeField]
    private Transform slotParent;
    [SerializeField]
    private Slot[] slots;

#if UNITY_EDITOR
    private void OnValidate()
    {
        slots = slotParent.GetComponentsInChildren<Slot>();
        //UnityEditor.EditorUtility.SetDirty(this);
    }
#endif
    void Awake()
    {
        RefreshSlot();
    }
    void Update()
    {
        InventoryOnOff();
    }

    private void InventoryOnOff()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryActivated = !inventoryActivated;

            if(inventoryActivated)
            {
                bag.SetActive(true);
            }
            else
                bag.SetActive(false);
        }
    }

    public void RefreshSlot()
    {
        int i = 0;
        for (; i < items.Count && i < slots.Length; i++)
        {
            slots[i].item = items[i];
        }
        for (; i < slots.Length; i++)
        {
            slots[i].item = null;
        }
    }

    public void AcquireItem(Item item)
    {
        int i = 0;
        if(Item.ItemType.Equipment != item.itemType)
        {
            //장비가 아닐 경우 갯수 표기를 위한 함수
        }

        for (; i < slots.Length; i++)
        {
            if (slots[i].item == null)
            {
                slots[i].AddItem(item);
                return;
            }
        }
    }
}