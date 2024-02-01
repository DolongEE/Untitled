using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentSlot : ItemSlot
{
    public override void OnPointerEnter(PointerEventData eventData)
    {
        // 슬롯에 있는 아이템에 마우스를 올리면 툴팁이 나옴
        if (item != null && InventoryManager.isEquippedItem == false)
        {
            UIManager.Instance.tooltip3D.ShowTooltip2D(item, transform.GetComponent<RectTransform>().position);
        }
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        // 슬롯에 있는 아이템에서 마우스를 떼면 툴팁 사라짐
        if (item != null)
        {
            UIManager.Instance.tooltip3D.HideTooltip2D();
        }
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        // 아이템 반투명 해져서 들어올려짐
        if (item != null)
        {
            DragSlot.instance.dragSlot = this;
            DragSlot.instance.DragSetImage(image);
            DragSlot.instance.SetColor(0.5f);
            DragSlot.instance.transform.position = eventData.position;
        }
    }
    public override void OnDrag(PointerEventData eventData)
    {
        // 아이템 옮겨지는 중
        if (item != null)
        {
            DragSlot.instance.transform.position = eventData.position;

            // 드래그 중에는 다른 툴팁이 뜨지 않도록
            UIManager.Instance.tooltip3D.HideTooltip2D();
        }
    }
    public override void OnEndDrag(PointerEventData eventData)
    {
        // 드래그 끝
        // 아이템이 인벤토리 바깥으로 넘어가게 될 경우 바닥에 버려짐.
        if (DragSlot.instance.transform.localPosition.x < baseRect.xMin ||
            DragSlot.instance.transform.localPosition.x > baseRect.xMax ||
            DragSlot.instance.transform.localPosition.y < baseRect.yMin ||
            DragSlot.instance.transform.localPosition.y > baseRect.yMax)
        {
            Vector3 itemPos = GameObject.Find("FpsController").transform.position;

            Instantiate(DragSlot.instance.dragSlot.item.itemPrefab,
                itemPos + new Vector3(0f, 0f, 2f), Quaternion.Euler(90f, 0, 0));
            DragSlot.instance.SetColor(0);
            DragSlot.instance.dragSlot = null;
        }
        else
        {
            DragSlot.instance.SetColor(0);
            DragSlot.instance.dragSlot = null;
        }
    }
    public override void OnDrop(PointerEventData eventData)
    {
        // 아이템 다른 슬롯으로 옮겨짐
        // 아이템을 EquipmentSlot에 놓았을 때 장착이 됨.
        if (DragSlot.instance.dragSlot != null)
        {
            if(DragSlot.instance.dragSlot.item.itemType == Item.ItemType.Equipment)
            {
                ChangeDraggedSlot();
                UIManager.Instance.inventoryManager.EquipItemFromInventory(item);
            }
        }
    }
}