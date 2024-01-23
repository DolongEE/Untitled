using System.Collections;
using System.Collections.Generic;
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
    public SpriteRenderer itemImage;

    public string tooltipText = "아이템 툴팁";
    public GameObject tooltipPrefab;

    private GameObject tooltipInstance;

    // Start is called before the first frame update
    void Start()
    {
        itemImage.sprite = item.itemImage;
    }
    public Item ClickItem()
    {
        return this.item;
    }

    private void OnMouseEnter()
    {
        ShowTooltip();
    }

    private void OnMouseExit()
    {
        HideTooltip();
    }

    void ShowTooltip()
    {
        if(tooltipPrefab != null && tooltipInstance == null)
        {
            Vector3 tooltipPosition = transform.position + new Vector3(0f, 1.5f, 0f);
            tooltipInstance = Instantiate(tooltipPrefab, tooltipPosition, Quaternion.identity);

            Text tooltipTextComponent = tooltipInstance.GetComponentInChildren<Text>();
            if (tooltipTextComponent != null)
            {
                tooltipTextComponent.text = tooltipText;
            }
        }
    }

    void HideTooltip()
    {
        if (tooltipInstance != null)
        {
            Destroy(tooltipInstance);
            tooltipInstance = null;
        }
    }
}