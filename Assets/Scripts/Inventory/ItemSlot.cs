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
        // ���Կ� �ִ� �����ۿ� ���콺�� �ø��� ������ ����
        if (_item != null)
        {
            UIManager.Instance.tooltip3D.ShowTooltip2D(_item, transform.GetComponent<RectTransform>().position);
        }
    }
    public virtual void OnPointerExit(PointerEventData eventData)
    {
        // ���Կ� �ִ� �����ۿ��� ���콺�� ���� ���� �����
        if (_item != null)
        {
            UIManager.Instance.tooltip3D.HideTooltip2D();
        }
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        // �κ��丮���� ��Ŭ�� �� ������ ����
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
    //    // ������ ������ ������ ���÷���
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
    //    // ������ �Ű����� ��
    //    if (_item != null)
    //    {
    //        DragSlot.instance.transform.position = eventData.position;

    //        // �巡�� �߿��� �ٸ� ������ ���� �ʵ���
    //        UIManager.Instance.tooltip3D.HideTooltip2D();
    //    }
    //}
    //public virtual void OnDrop(PointerEventData eventData)
    //{
    //    // ������ �ٸ� �������� �Ű���
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
    //    // �巡�� ��
    //    // �������� �κ��丮 �ٱ����� �Ѿ�� �� ��� �ٴڿ� ������.
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
    // ������ ������ ���� �����ִ� �����
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