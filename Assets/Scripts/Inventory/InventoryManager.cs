using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    private static InventoryManager _instance;
    public static InventoryManager instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<InventoryManager>();

            return _instance;
        }
    }

    public GameObject tooltipPrefab;
    private GameObject tooltipInstance;
    public slotTooltip tooltip;

    public void ShowTooltip2D(Item _item, Vector3 _pos)
    {
        tooltip.ShowTooltip(_item, _pos);
    }
    public void HideTooltip2D()
    {
        tooltip.HideTooltip();
    }
    public void ShowTooltip3D(string itemName, string itemTooltip, Sprite itemImage, Vector3 position)
    {
        if (tooltipPrefab != null)
        {
            tooltipInstance = Instantiate(tooltipPrefab, position, Quaternion.Euler(30f, 0f, 0f));

            Image[] tooltipImage = tooltipInstance.GetComponentsInChildren<Image>();
            Text[] tooltipText = tooltipInstance.GetComponentsInChildren<Text>();

            if (tooltipInstance != null)
            {
                tooltipImage[1].sprite = itemImage;
                tooltipText[0].text = itemName;
                tooltipText[1].text = itemTooltip;
            }
        }
    }
    public void HideTooltip3D()
    {
        if (tooltipInstance != null)
        {
            Destroy(tooltipInstance);
            tooltipInstance = null;
        }
    }
}