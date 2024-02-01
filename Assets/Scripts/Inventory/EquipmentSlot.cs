using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentSlot : ItemSlot
{
    [SerializeField] private EquippableItem _weaponitem;
    public EquippableItem weaponItem
    {
        get
        {
            return _weaponitem;
        }
        set
        {
            _weaponitem = value;
        }
    }
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

    //public override void OnBeginDrag(PointerEventData eventData)
    //{
    //    // ������ ������ ������ ���÷���
    //    if (item != null)
    //    {
    //        DragSlot.instance.dragSlot = this;
    //        DragSlot.instance.DragSetImage(image);
    //        DragSlot.instance.SetColor(0.5f);
    //        DragSlot.instance.transform.position = eventData.position;
    //    }
    //}
    //public override void OnDrag(PointerEventData eventData)
    //{
    //    // ������ �Ű����� ��
    //    if (item != null)
    //    {
    //        DragSlot.instance.transform.position = eventData.position;

    //        // �巡�� �߿��� �ٸ� ������ ���� �ʵ���
    //        UIManager.Instance.tooltip3D.HideTooltip2D();
    //    }
    //}
    //public override void OnEndDrag(PointerEventData eventData)
    //{
    //    // �巡�� ��
    //    DragSlot.instance.SetColor(0);
    //    DragSlot.instance.dragSlot = null;
    //}
    //public override void OnDrop(PointerEventData eventData)
    //{
    //    // ������ �ٸ� �������� �Ű���
    //    // �������� EquipmentSlot�� ������ �� ������ ��.
    //    if (DragSlot.instance.dragSlot != null)
    //    {
    //        if (DragSlot.instance.dragSlot.item.itemType == Item.ItemType.Equipment)
    //        {
    //            ChangeDraggedSlot();
    //            UIManager.Instance.inventoryManager.EquipItemFromInventory(item);
    //        }
    //    }
    //}
}