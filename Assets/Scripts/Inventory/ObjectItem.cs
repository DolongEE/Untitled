using UnityEngine;
using UnityEngine.UI;

public interface IObjectItem
{
    Item ClickItem();
}

public class ObjectItem : MonoBehaviour, IObjectItem
{
    [Header("������")]
    public Item item;

    [Header("������ �̹���")]
    public Sprite itemImage;
    [Header("������ �̸�")]
    public string itemName;
    [Header("������ ����")]
    public string itemTooltip;

    void Start()
    {
        itemName = item.itemName;
        itemTooltip = item.itemDescription;
        itemImage = item.itemImage;
    }
    public Item ClickItem()
    {
        ItemManager.instance.HideTooltip3D();
        return this.item;
    }
    private void OnMouseEnter()
    {
        ItemManager.instance.ShowTooltip3D(itemName, itemTooltip, itemImage, transform.position + new Vector3(0f, 3f, 0f));
    }

    private void OnMouseExit()
    {
        ItemManager.instance.HideTooltip3D();
    }
}