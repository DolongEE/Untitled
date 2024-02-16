using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ItemDrag : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private Item _item;
    public Item Item { get { return _item; } set { _item = value; } }
    public Image image;
    public TextMeshProUGUI tmPro;

    private void Awake()
    {
        image = transform.Find("Image").GetComponent<Image>();
        tmPro = transform.Find("Amount").GetComponent<TextMeshProUGUI>();
    }
    private void Start()
    {
        tmPro.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // ���Կ� �ִ� �����ۿ� ���콺�� �ø��� ������ ����
        if (_item != null && Managers.INVENTORY.isEquippedItem == false)
        {
            Managers.INVENTORY.toolTip.ShowTooltip2D(_item, transform.GetComponent<RectTransform>().position);
        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        // ���Կ� �ִ� �����ۿ��� ���콺�� ���� ���� �����
        if (_item != null)
        {
            Managers.INVENTORY.toolTip.HideTooltip2D();
        }
    }
    public virtual void OnPointerClick(PointerEventData eventData)
    {
        // �κ��丮���� ��Ŭ�� �� ������ ����
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (_item == null)
                return;

            if (_item.itemType == Item.ItemType.Equipment)
            {
                if (Managers.INVENTORY.isEquippedItem == false)
                    Managers.INVENTORY.EquipItemFromInventory((EquippableItem)_item);
            }

            else if (_item.itemType == Item.ItemType.Consumable)
            {
                Managers.INVENTORY.UseItem((UsableItem)_item);
            }
        }
    }

    #region �̹���
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
        if(_item.itemType == Item.ItemType.Equipment)
        {
            tmPro.gameObject.SetActive(false);
        }
        else
        {
            tmPro.gameObject.SetActive(true);
            tmPro.text = _item.maxItemStacks.ToString();
        }
        SetColor(1);
    }

    public void RemoveItemImage()
    {
        _item = null;
        image.sprite = null;
        tmPro.text = null;
        SetColor(0);
    }
    #endregion
}
