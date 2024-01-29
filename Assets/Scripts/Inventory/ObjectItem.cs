using UnityEngine;
using UnityEngine.UI;

public interface IObjectItem
{
    Item ClickItem();
}

public class ObjectItem : MonoBehaviour, IObjectItem
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
        UIManager.Instance.inventoryManager.HideTooltip3D();
        return this.item;
    }
    private void OnMouseEnter()
    {
        UIManager.Instance.inventoryManager.ShowTooltip3D(itemName, itemTooltip, itemImage, transform.position + new Vector3(0f, 3f, 0f));
    }

    private void OnMouseExit()
    {
        UIManager.Instance.inventoryManager.HideTooltip3D();
    }
}