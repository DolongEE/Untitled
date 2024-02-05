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
    // ItemSlot�� EquipmentSlot�� Drop�� ��� EquipItem�� �ؾ� �Ѵ�.
    public override void OnDrop(PointerEventData eventData)
    {
        // �������� EquipmentSlot�� ������ �� ������ ��.
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