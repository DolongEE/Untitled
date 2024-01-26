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
        // 슬롯에 있는 아이템에 마우스를 올리면 툴팁이 나옴
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        // 슬롯에 있는 아이템에서 마우스를 떼면 툴팁 사라짐
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (_item != null)
            {
                // 아이템 장착
            }
        }
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        // 아이템 반투명 해져서 들어올려짐
        if (_item != null)
        {
            DragSlot.instance.dragSlot = this;
            DragSlot.instance.DragSetImage(image);
            DragSlot.instance.transform.position = eventData.position;
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        // 아이템 반투명 해져서 옮겨지는 중
        if (_item != null)
        {
            DragSlot.instance.transform.position = eventData.position;
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        // 드래그 끝
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
        // 아이템 다른 슬롯으로 옮겨짐
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