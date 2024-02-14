using UnityEngine;

public interface IObjectItem
{
    Item ClickItem();
}

public class PickupItem : MonoBehaviour, IObjectItem
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
        UIManager.Instance.tooltip2D.HideTooltip2D();
        return this.item;
    }
    private void OnMouseEnter()
    {
        UIManager.Instance.tooltip2D.ShowTooltip2D(item, Input.mousePosition);
    }

    private void OnMouseExit()
    {
        UIManager.Instance.tooltip2D.HideTooltip2D();
    }
}