using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentSlot : ItemSlot
{
    public override void OnPointerEnter(PointerEventData eventData)
    {
        // ���Կ� �ִ� �����ۿ� ���콺�� �ø��� ������ ����
        if (item != null && InventoryManager.isEquippedItem == false)
        {
            UIManager.Instance.tooltip3D.ShowTooltip2D(item, transform.GetComponent<RectTransform>().position);
        }
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        // ���Կ� �ִ� �����ۿ��� ���콺�� ���� ���� �����
        if (item != null)
        {
            UIManager.Instance.tooltip3D.HideTooltip2D();
        }
    }

    public override void OnBeginDrag(PointerEventData eventData)
    {
        // ������ ������ ������ ���÷���
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
        // ������ �Ű����� ��
        if (item != null)
        {
            DragSlot.instance.transform.position = eventData.position;

            // �巡�� �߿��� �ٸ� ������ ���� �ʵ���
            UIManager.Instance.tooltip3D.HideTooltip2D();
        }
    }
    public override void OnEndDrag(PointerEventData eventData)
    {
        // �巡�� ��
        // �������� �κ��丮 �ٱ����� �Ѿ�� �� ��� �ٴڿ� ������.
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
        // ������ �ٸ� �������� �Ű���
        // �������� EquipmentSlot�� ������ �� ������ ��.
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