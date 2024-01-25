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
            
            // 아이템이 존재할 경우
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
                // 아이템 장착
            }
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // 아이템 반투명 해져서 들어올려짐
        throw new System.NotImplementedException();
    }

    public void OnDrag(PointerEventData eventData)
    {
        // 아이템 반투명 해져서 옮겨지는 중
        throw new System.NotImplementedException();
    }

    public void OnDrop(PointerEventData eventData)
    {
        // 아이템 다른 슬롯으로 옮겨짐
        throw new System.NotImplementedException();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
}
