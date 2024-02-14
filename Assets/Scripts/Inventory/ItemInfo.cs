using UnityEngine;

public interface IObjectItem
{
    Item ClickItem();
}

public class ItemInfo : MonoBehaviour, IObjectItem
{
    public Item item;
    public Sprite itemImage;
    public string itemName;
    public string itemTooltip;

    void Start()
    {
        itemName = item.itemName;
        itemTooltip = item.itemDescription;
        itemImage = item.itemImage;
    }
    public Item ClickItem()
    {
        Managers.INVENTORY.toolTip.HideTooltip2D();
        return this.item;
    }
    private void OnMouseEnter()
    {
        Managers.INVENTORY.toolTip.ShowTooltip2D(item, Input.mousePosition);
    }

    private void OnMouseExit()
    {
        Managers.INVENTORY.toolTip.HideTooltip2D();
    }
}