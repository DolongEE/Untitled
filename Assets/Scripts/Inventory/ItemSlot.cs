using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler,
    IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    private Item _item;
    public Item Item
    {
        get
        {
            return _item;
        }
        set
        {
            _item = value;
        }
    }
    public Image image;

    void Awake()
    {
        image = GetComponent<Image>();
    }

    public void SetColor(float _alpha)
    {
        Color color = image.color;
        color.a = _alpha;
        image.color = color;
    }

    public void AddItem(Item newItem)
    {
        _item = newItem;
        image.sprite = newItem.itemImage;
        SetColor(1);
    }

    public void RemoveItem()
    {
        _item = null;
        image.sprite = null;
        SetColor(0);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // ���Կ� �ִ� �����ۿ� ���콺�� �ø��� ������ ����
        if (_item != null && Managers.INVENTORY.isEquippedItem == false)
        {
            UIManager.Instance.tooltip2D.ShowTooltip2D(_item, transform.GetComponent<RectTransform>().position);
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        // ���Կ� �ִ� �����ۿ��� ���콺�� ���� ���� �����
        if (_item != null)
        {
            UIManager.Instance.tooltip2D.HideTooltip2D();
        }
    }
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        // �κ��丮���� ��Ŭ�� �� �Һ� ������ ���
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (_item != null && _item.itemType == Item.ItemType.Consumable)
            {
                Managers.INVENTORY.UseItem((UsableItem)_item);
            }
        }
        // �κ��丮���� ��Ŭ�� �� ������ ����
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (_item != null && _item.itemType == Item.ItemType.Equipment)
            {
                if (Managers.INVENTORY.isEquippedItem == false)
                    Managers.INVENTORY.EquipItemFromInventory((EquippableItem)_item);
            }
        }
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_item != null)
        {
            // ������ ������ ������ ���÷���
            DragSlot.instance.SetDraggedItem(this);
            DragSlot.instance.SetColor(0.5f);
            DragSlot.instance.transform.position = eventData.position;
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (_item != null)
        {
            // ������ �Ű����� ��
            DragSlot.instance.transform.position = eventData.position;
            // �巡�� �߿��� �ٸ� ������ ���� �ʵ���
            UIManager.Instance.tooltip2D.HideTooltip2D();
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        // �巡�� ��
        // �������� �κ��丮 �ٱ����� �Ѿ�� �� ��� �ٴڿ� ������.
        //UIManager.Instance.inventoryManager.DropItem(_item);
        DragSlot.instance.SetColor(0);
        DragSlot.instance.dragSlot = null;
    }
    // EquipmentSlot�� ItemSlot�� Drop�� ��� UnequipItem�� �ؾ� �ϰ�,
    // ItemSlot�� Drop�� ��� Item Swap�� �ؾ� �Ѵ�.
    public virtual void OnDrop(PointerEventData eventData)
    {
        // ������ �ٸ� �������� �Ű���
        if (DragSlot.instance.GetDraggedItem() != null)
        {
            // �� ���Կ��� �������� ���� �� �ְ� �ϱ� ����.
            if (_item == null)
            {
                if (Managers.INVENTORY.isEquippedItem == true)
                {
                    if (DragSlot.instance.GetDraggedItem().itemType == Item.ItemType.Equipment)
                    {
                        Managers.INVENTORY.UnEquipItemFromEquip((EquippableItem)DragSlot.instance.GetDraggedItem());
                        DragSlot.instance.ResetDraggedSlot();
                    }
                }
                ChangeDraggedSlot();
            }
            else
            {
                if (Managers.INVENTORY.isEquippedItem == true)
                {
                    if (DragSlot.instance.GetDraggedItem().itemType == Item.ItemType.Equipment)
                    {
                        Debug.Log(DragSlot.instance.GetDraggedItem());
                        Managers.INVENTORY.UnEquipItemFromEquip((EquippableItem)DragSlot.instance.GetDraggedItem());
                        DragSlot.instance.ResetDraggedSlot();
                    }
                    else
                    {
                        ChangeDraggedSlot();
                        return;
                    }
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
        Item tempItem = _item;

        AddItem(DragSlot.instance.dragSlot._item);

        if (tempItem != null)
        {
            DragSlot.instance.dragSlot.AddItem(tempItem);
        }
        else
        {
            DragSlot.instance.dragSlot.RemoveItem();
        }

    }
}