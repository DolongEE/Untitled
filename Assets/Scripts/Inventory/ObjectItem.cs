using UnityEngine;
using UnityEngine.UI;

public interface IObjectItem
{
    Item ClickItem();
}

public class ObjectItem : MonoBehaviour, IObjectItem
{
    [Header("아이템")]
    public Item item;

    [Header("아이템 이미지")]
    public Sprite itemImage;
    [Header("아이템 이름")]
    public string itemName;
    [Header("아이템 설명")]
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