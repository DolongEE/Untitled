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
            image.sprite = (_item != null) ? _item.itemImage : null;
            SetColor((_item != null) ? 1 : 0);
        }
    }
    public Image image;

    void Start()
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
        // 슬롯에 있는 아이템에 마우스를 올리면 툴팁이 나옴
        if (_item != null && InventoryManager.isEquippedItem == false)
        {
            UIManager.Instance.tooltip3D.ShowTooltip2D(_item, transform.GetComponent<RectTransform>().position);
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        // 슬롯에 있는 아이템에서 마우스를 떼면 툴팁 사라짐
        if (_item != null)
        {
            UIManager.Instance.tooltip3D.HideTooltip2D();
        }
    }
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        // 인벤토리에서 좌클릭 시 소비 아이템 사용
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (_item != null && _item.itemType == Item.ItemType.Consumable)
            {
                UIManager.Instance.inventoryManager.UseItem((UsableItem)_item);
            }
        }
        // 인벤토리에서 우클릭 시 아이템 장착
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (_item != null && _item.itemType == Item.ItemType.Equipment)
            {
                if (InventoryManager.isEquippedItem == false)
                    UIManager.Instance.inventoryManager.EquipItemFromInventory((EquippableItem)_item);
            }
        }
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (_item != null)
        {
            // 아이템 반투명 해져서 들어올려짐
            DragSlot.instance.SetDraggedItem(this);
            DragSlot.instance.SetColor(0.5f);
            DragSlot.instance.transform.position = eventData.position;
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (_item != null)
        {
            // 아이템 옮겨지는 중
            DragSlot.instance.transform.position = eventData.position;
            // 드래그 중에는 다른 툴팁이 뜨지 않도록
            UIManager.Instance.tooltip3D.HideTooltip2D();
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        // 드래그 끝
        // 아이템이 인벤토리 바깥으로 넘어가게 될 경우 바닥에 버려짐.
        //UIManager.Instance.inventoryManager.DropItem(_item);
        DragSlot.instance.SetColor(0);
        DragSlot.instance.dragSlot = null;
    }
    // EquipmentSlot이 ItemSlot에 Drop될 경우 UnequipItem을 해야 하고,
    // ItemSlot에 Drop될 경우 Item Swap을 해야 한다.
    public virtual void OnDrop(PointerEventData eventData)
    {
        // 아이템 다른 슬롯으로 옮겨짐
        if (DragSlot.instance.GetDraggedItem() != null)
        {
            // 빈 슬롯에도 아이템을 놓을 수 있게 하기 위함.
            if (_item == null)
            {
                if (InventoryManager.isEquippedItem == true)
                {
                    if (DragSlot.instance.GetDraggedItem().itemType == Item.ItemType.Equipment)
                    {
                        UIManager.Instance.inventoryManager.UnEquipItemFromEquip((EquippableItem)DragSlot.instance.GetDraggedItem());
                        DragSlot.instance.ResetDraggedSlot();
                    }
                }
                ChangeDraggedSlot();
            }
            else
            {
                if (InventoryManager.isEquippedItem == true)
                {
                    if (DragSlot.instance.GetDraggedItem().itemType == Item.ItemType.Equipment)
                    {
                        Debug.Log(DragSlot.instance.GetDraggedItem());
                        UIManager.Instance.inventoryManager.UnEquipItemFromEquip((EquippableItem)DragSlot.instance.GetDraggedItem());
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