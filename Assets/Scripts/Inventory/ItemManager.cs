using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{
    private static ItemManager _instance;
    public static ItemManager instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<ItemManager>();

                if (_instance == null)
                {
                    GameObject itemManager = new GameObject("ItemManager");
                    _instance = itemManager.AddComponent<ItemManager>();
                }
            }

            return _instance;
        }
    }

    public GameObject tooltipPrefab;
    private GameObject tooltipInstance;
    [SerializeField]
    private slotTooltip slottoolTip;

    public void ShowTooltip2D(Item _item, Vector3 _pos)
    {
        slottoolTip.ShowTooltip(_item, _pos);
    }
    public void HideTooltip2D()
    {
        slottoolTip.HideTooltip();
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