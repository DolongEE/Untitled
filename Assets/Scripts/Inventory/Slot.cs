using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IDropHandler
{
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
            
            // �������� ������ ���
            if(_item != null)
            {
                image.sprite = item.itemImage;
                image.color = new Color(1, 1, 1, 1);
            }

            else
            {
                image.color = new Color(1, 1, 1, 0);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        RectTransform rect = gameObject.GetComponent<RectTransform>();

        ItemManager.instance.ShowTooltip(item.itemName, item.itemDescription, item.itemImage, rect.position);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Left)
        {
            if(item != null)
            {
                // ������ ����
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // ������ ������ ������ ���÷���
        throw new System.NotImplementedException();
    }

    public void OnDrag(PointerEventData eventData)
    {
        // ������ ������ ������ �Ű����� ��
        throw new System.NotImplementedException();
    }

    public void OnDrop(PointerEventData eventData)
    {
        // ������ �ٸ� �������� �Ű���
        throw new System.NotImplementedException();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
}
