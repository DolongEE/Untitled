using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentSlot : ItemSlot
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (InventoryManager.isEquippedItem == true)
                UIManager.Instance.inventoryManager.UnEquipItemFromEquip((EquippableItem)Item);
        }
    }
    // ItemSlot이 EquipmentSlot에 Drop될 경우 EquipItem을 해야 한다.
    public override void OnDrop(PointerEventData eventData)
    {
        // 아이템을 EquipmentSlot에 놓았을 때 장착이 됨.
        if (DragSlot.instance.GetDraggedItem() != null)
        {
            if (DragSlot.instance.GetDraggedItem().itemType == Item.ItemType.Equipment)
            {
                if (InventoryManager.isEquippedItem == false)
                {
                    UIManager.Instance.inventoryManager.EquipItemFromInventory((EquippableItem)DragSlot.instance.GetDraggedItem());
                    DragSlot.instance.ResetDraggedSlot();
                }
            }
        }
    }
}