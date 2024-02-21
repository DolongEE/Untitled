using UnityEngine;
using UnityEngine.EventSystems;

public class ItemSlot : ItemDrag, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public bool CanAddStack(Item item)
    {
        return Item != null && Item.ID == item.ID && item.itemType != Item.ItemType.Equipment;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (Item != null)
        {
            // 아이템 반투명 해져서 들어올려짐
            DragSlot.instance.SetDraggedItem(this);
            DragSlot.instance.SetColor(0.5f);
            DragSlot.instance.transform.position = eventData.position;
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (Item != null)
        {
            // 아이템 옮겨지는 중
            DragSlot.instance.transform.position = eventData.position;
            // 드래그 중에는 다른 툴팁이 뜨지 않도록
            Managers.INVENTORY.toolTip.HideTooltip2D();
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        // 드래그 끝
        // 아이템이 인벤토리 바깥으로 넘어가게 될 경우 바닥에 버려짐.
        //UIManager.Instance.inventoryManager.DropItem(_item);
        DragSlot.instance.SetColor(0);
        DragSlot.instance.ResetDraggedSlot();
    }
    // EquipmentSlot이 ItemSlot에 Drop될 경우 UnequipItem을 해야 하고,
    // ItemSlot에 Drop될 경우 Item Swap을 해야 한다.
    public virtual void OnDrop(PointerEventData eventData)
    {
        // 아이템 다른 슬롯으로 옮겨짐
        if (DragSlot.instance.GetDraggedItem() != null)
        {
            // 빈 슬롯에도 아이템을 놓을 수 있게 하기 위함.
            if (Item == null)
            {
                if (DragSlot.instance.GetDraggedItem().IsEquipped == true)
                {
                    Managers.INVENTORY.UnEquipItemFromEquip((EquippableItem)DragSlot.instance.GetDraggedItem());
                }
                else
                {
                    ChangeDraggedSlot();
                    return;
                }
            }
            else
            {
                if (DragSlot.instance.GetDraggedItem().IsEquipped == true)
                {
                    Managers.INVENTORY.UnEquipItemFromEquip((EquippableItem)DragSlot.instance.GetDraggedItem());
                }
                else
                {
                    ChangeDraggedSlot();
                    return;
                }
            }
        }
    }
    public void ChangeDraggedSlot()
    {
        ItemSlot draggedSlot = DragSlot.instance.dragSlot;
        Item tempItem = Item;
        int tempAmount = Amount;

        SetItemImage(draggedSlot.Item);
        Amount = draggedSlot.Amount;

        if (tempItem != null)
        {
            if (tempItem.itemType == Item.ItemType.Equipment)
            {
                draggedSlot.SetItemImage(tempItem);
            }
            else
            {
                draggedSlot.SetItemImage(tempItem);
                draggedSlot.Amount = tempAmount;
                draggedSlot.RefreshAmount();
            }
        }
        else
        {
            draggedSlot.RemoveItemImage();
        }

        RefreshAmount();
    }
}