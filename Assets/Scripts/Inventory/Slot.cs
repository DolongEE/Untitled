using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler,
    IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
    public Image image;
    public Button button;
    private Rect baseRect;

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
        baseRect = transform.parent.parent.parent.GetComponent<RectTransform>().rect;
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

    private void RemoveItem()
    {
        _item = null;
        image.sprite = null;
        SetColor(0);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // ���Կ� �ִ� �����ۿ� ���콺�� �ø��� ������ ����
        if (_item != null)
        {
            InventoryManager.instance.ShowTooltip2D(_item, transform.GetComponent<RectTransform>().position);
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        // ���Կ� �ִ� �����ۿ��� ���콺�� ���� ���� �����
        InventoryManager.instance.HideTooltip2D();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (_item != null)
            {
                if(_item.itemType == Item.ItemType.Equipment)
                {
                    // ������ ����
                    GameObject arms = GameObject.Find("LeftArm");
                    GameObject weapon = Instantiate(_item.itemPrefab, arms.transform.position, Quaternion.Euler(-20f, 90f, 45f));
                    weapon.transform.SetParent(arms.transform);
                    Destroy(weapon.GetComponent<Rigidbody>());

                    RemoveItem();
                }
                else
                {
                    Debug.Log("��� �������� �ƴմϴ�.");
                }
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
            DragSlot.instance.SetColor(0.5f);
            DragSlot.instance.transform.position = eventData.position;
        }
    }
    public void OnDrag(PointerEventData eventData)
    {
        // ������ �Ű����� ��
        if (_item != null)
        {
            DragSlot.instance.transform.position = eventData.position;

            // �巡�� �߿��� �ٸ� ������ ���� �ʵ���
            InventoryManager.instance.HideTooltip2D();
        }
    }
    public void OnDrop(PointerEventData eventData)
    {
        // ������ �ٸ� �������� �Ű���
        if (DragSlot.instance.dragSlot != null)
        {
            ChangeSlot();
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        // �������� �κ��丮 �ٱ����� �Ѿ�� �� ��� �ٴڿ� ������.
        if (DragSlot.instance.transform.localPosition.x < baseRect.xMin ||
            DragSlot.instance.transform.localPosition.x > baseRect.xMax ||
            DragSlot.instance.transform.localPosition.y < baseRect.yMin ||
            DragSlot.instance.transform.localPosition.y > baseRect.yMax)
        {
            Vector3 itemPos = GameObject.Find("FpsController").transform.position;

            Instantiate(DragSlot.instance.dragSlot._item.itemPrefab,
                itemPos + new Vector3(0f, 0f, 2f), Quaternion.Euler(90f, 0, 0));
            DragSlot.instance.dragSlot.RemoveItem();
        }
        // �巡�� ��
        DragSlot.instance.SetColor(0);
        DragSlot.instance.dragSlot = null;
    }
    private void ChangeSlot()
    {
        Item tempItem = _item;

        AddItem(DragSlot.instance.dragSlot._item);

        if (tempItem != null)
            DragSlot.instance.dragSlot.AddItem(tempItem);
        else
            DragSlot.instance.dragSlot.RemoveItem();
    }
}