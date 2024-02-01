using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    //IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    [SerializeField] private Item _item;
    public Item item
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
    protected Rect baseRect;

    void Start()
    {
        image = GetComponent<Image>();
        baseRect = transform.parent.parent.parent.GetComponent<RectTransform>().rect;
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

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        // 슬롯에 있는 아이템에 마우스를 올리면 툴팁이 나옴
        if (_item != null)
        {
            UIManager.Instance.tooltip3D.ShowTooltip2D(_item, transform.GetComponent<RectTransform>().position);
        }
    }
    public virtual void OnPointerExit(PointerEventData eventData)
    {
        // 슬롯에 있는 아이템에서 마우스를 떼면 툴팁 사라짐
        if (_item != null)
        {
            UIManager.Instance.tooltip3D.HideTooltip2D();
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        // 인벤토리에서 우클릭 시 아이템 장착
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (_item != null && _item.itemType == Item.ItemType.Equipment)
            {
                if(InventoryManager.isEquippedItem == false)
                {
                    UIManager.Instance.inventoryManager.EquipItemFromInventory((EquippableItem)_item);
                }
                else
                {
                    UIManager.Instance.inventoryManager.UnEquipItemFromEquip((EquippableItem)_item);
                }
            }
        }
    }
    //public virtual void OnBeginDrag(PointerEventData eventData)
    //{
    //    // 아이템 반투명 해져서 들어올려짐
    //    if (_item != null)
    //    {
    //        DragSlot.instance.dragSlot = this;
    //        DragSlot.instance.DragSetImage(image);
    //        DragSlot.instance.SetColor(0.5f);
    //        DragSlot.instance.transform.position = eventData.position;
    //    }
    //}
    //public virtual void OnDrag(PointerEventData eventData)
    //{
    //    // 아이템 옮겨지는 중
    //    if (_item != null)
    //    {
    //        DragSlot.instance.transform.position = eventData.position;

    //        // 드래그 중에는 다른 툴팁이 뜨지 않도록
    //        UIManager.Instance.tooltip3D.HideTooltip2D();
    //    }
    //}
    //public virtual void OnDrop(PointerEventData eventData)
    //{
    //    // 아이템 다른 슬롯으로 옮겨짐
    //    if (DragSlot.instance.dragSlot != null)
    //    {
    //        ChangeDraggedSlot();
    //        if(_item.itemType == Item.ItemType.Equipment)
    //        {
    //            UIManager.Instance.inventoryManager.UnEquipItemFromEquip(_item);
    //        }
    //    }
    //}
    //public virtual void OnEndDrag(PointerEventData eventData)
    //{
    //    // 드래그 끝
    //    // 아이템이 인벤토리 바깥으로 넘어가게 될 경우 바닥에 버려짐.
    //    if (DragSlot.instance.transform.localPosition.x < baseRect.xMin ||
    //        DragSlot.instance.transform.localPosition.x > baseRect.xMax ||
    //        DragSlot.instance.transform.localPosition.y < baseRect.yMin ||
    //        DragSlot.instance.transform.localPosition.y > baseRect.yMax)
    //    {
    //        Vector3 itemPos = GameObject.Find("FpsController").transform.position;

    //        Instantiate(DragSlot.instance.dragSlot._item.itemPrefab,
    //            itemPos + new Vector3(0f, 0f, 2f), Quaternion.Euler(90f, 0, 0));
    //        DragSlot.instance.SetColor(0);
    //        DragSlot.instance.dragSlot = null;
    //    }
    //    else
    //    {
    //        DragSlot.instance.SetColor(0);
    //        DragSlot.instance.dragSlot = null;
    //    }
    //}
    //public void ChangeDraggedSlot()
    //{
    //    Item tempItem = _item;

    //    AddItem(DragSlot.instance.dragSlot._item);

    //    if (tempItem != null)
    //    {
    //        DragSlot.instance.dragSlot.AddItem(tempItem);
    //    }
    //    else
    //    {
    //        DragSlot.instance.dragSlot.RemoveItem();
    //    }
    //}

    #region Gizmos
    // 아이템 버리는 공간 보여주는 기즈모
    //private void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.blue;

    //    Vector3[] corners = new Vector3[4];
    //    transform.parent.parent.parent.GetComponent<RectTransform>().GetWorldCorners(corners);

    //    for (int i = 0; i < corners.Length; i++)
    //    {
    //        int nextIndex = (i + 1) % corners.Length;
    //        Gizmos.DrawLine(corners[i], corners[nextIndex]);
    //    }
    //}
    #endregion
}