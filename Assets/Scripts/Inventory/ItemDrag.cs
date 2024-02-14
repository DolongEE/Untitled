using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEditorInternal.Profiling.Memory.Experimental;

public class ItemDrag : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Item _item;
    public Item Item { get { return _item; } set { _item = value; } }
    public Image image;

    private void Awake()
    {
        image = transform.Find("Image").GetComponent<Image>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // 슬롯에 있는 아이템에 마우스를 올리면 툴팁이 나옴
        if (_item != null && Managers.INVENTORY.isEquippedItem == false)
        {
            Managers.INVENTORY.toolTip.ShowTooltip2D(_item, transform.GetComponent<RectTransform>().position);
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        // 슬롯에 있는 아이템에서 마우스를 떼면 툴팁 사라짐
        if (_item != null)
        {
            Managers.INVENTORY.toolTip.HideTooltip2D();
        }
    }
    public virtual void OnPointerClick(PointerEventData eventData)
    {        
        // 인벤토리에서 좌클릭 시 소비 아이템 사용
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (_item != null && _item.itemType == Item.ItemType.Consumable)
            {
                Managers.INVENTORY.UseItem((UsableItem)_item);
            }
        }
        // 인벤토리에서 우클릭 시 아이템 장착
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (_item != null && _item.itemType == Item.ItemType.Equipment)
            {
                if (Managers.INVENTORY.isEquippedItem == false)
                    Managers.INVENTORY.EquipItemFromInventory((EquippableItem)_item);
            }
        }
    }

    #region 이미지
    public void SetColor(float _alpha)
    {
        Color color = image.color;
        color.a = _alpha;
        image.color = color;
    }

    public void SetItemImage(Item newItem)
    {
        _item = newItem;
        image.sprite = newItem.itemImage;
        SetColor(1);
    }

    public void RemoveItemImage()
    {
        _item = null;
        image.sprite = null;
        SetColor(0);
    }
    #endregion
}
