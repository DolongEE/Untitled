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
    // ItemSlot이 EquipmentSlot에 Drop될 경우 EquipItem을 해야 한다.
    public override void OnDrop(PointerEventData eventData)
    {
        // 아이템을 EquipmentSlot에 놓았을 때 장착이 됨.
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