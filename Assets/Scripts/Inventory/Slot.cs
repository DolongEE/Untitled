using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    private Rect rect;
    private ItemManager itemManager;

    public Image image;
    public Button button;

    private Item _item;
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

    void Start()
    {
        image = GetComponent<Image>();
        button = GetComponent<Button>();
        rect = transform.parent.parent.GetComponent<RectTransform>().rect;
        itemManager = FindObjectOfType<ItemManager>();
    }

    public void SetColor(float alpha)
    {
        Color color = image.color;
        color.a = alpha;
        image.color = color;
    }

    public void AddItem(Item item)
    {
        _item = item;
        image.sprite = item.itemImage;
        SetColor(1);
    }

    private void ClearSlot()
    {
        _item = null;
        image.sprite = null;
        SetColor(0);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // ���Կ� �ִ� �����ۿ� ���콺�� �ø��� ������ ����
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        // ���Կ� �ִ� �����ۿ��� ���콺�� ���� ���� �����
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (_item != null)
            {
                // ������ ����
            }
        }
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        // ������ ������ ������ ���÷���
        if (_item != null)
        {
            DragSlot.instance.dragSlot = this;
            DragSlot.instance.DragSetImage(image);
            DragSlot.instance.transform.position = eventData.position;
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        // ������ ������ ������ �Ű����� ��
        if (_item != null)
        {
            DragSlot.instance.transform.position = eventData.position;
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        // �巡�� ��
        //if (DragSlot.instance.transform.localPosition.x < rect.xMin ||
        //   DragSlot.instance.transform.localPosition.x > rect.xMax ||
        //   DragSlot.instance.transform.localPosition.y < rect.yMin ||
        //   DragSlot.instance.transform.localPosition.y > rect.yMax)
        //{
        //    Instantiate(DragSlot.instance.dragSlot._item.itemPrefab,
        //        itemManager.transform.position + itemManager.transform.forward, Quaternion.Euler(90, 0, 0));
        //    DragSlot.instance.dragSlot.ClearSlot();
        //}
        DragSlot.instance.SetColor(0);
        DragSlot.instance.dragSlot = null;
    }
    public void OnDrop(PointerEventData eventData)
    {
        // ������ �ٸ� �������� �Ű���
        if (DragSlot.instance.dragSlot != null)
        {
            ChangeSlot();
        }
    }
    private void ChangeSlot()
    {
        Item tempItem = _item;

        AddItem(DragSlot.instance.dragSlot._item);

        if (tempItem != null)
            DragSlot.instance.dragSlot.AddItem(tempItem);
        else
            DragSlot.instance.dragSlot.ClearSlot();
    }
}