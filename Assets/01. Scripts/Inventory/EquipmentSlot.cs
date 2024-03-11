using UnityEngine.EventSystems;

public class EquipmentSlot : ItemSlot
{
    public EItemType equipmentType;
    public override void OnPointerClick(PointerEventData eventData)
    {
        if (Item == null)
            return;

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
                    EquippableItem equippableItem = DragSlot.instance.GetDraggedItem() as EquippableItem;
                    
                    if(equippableItem.wItemType == equipmentType)
                    {

                        Managers.INVENTORY.EquipItemFromInventory((EquippableItem)DragSlot.instance.GetDraggedItem());
                        DragSlot.instance.ResetDraggedSlot();
                    }
                }
            }
        }
    }
}