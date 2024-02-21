using UnityEngine.EventSystems;

public class EquipmentSlot : ItemSlot
{
    public override void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (Item.IsEquipped == true)
                Managers.INVENTORY.UnEquipItemFromEquip((EquippableItem)Item);
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
                if (DragSlot.instance.GetDraggedItem().IsEquipped == false)
                {
                    Managers.INVENTORY.EquipItemFromInventory((EquippableItem)DragSlot.instance.GetDraggedItem());
                    DragSlot.instance.ResetDraggedSlot();
                }
            }
        }
    }
}