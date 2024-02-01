using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolTip3D : MonoBehaviour
{
    private GameObject tooltipInstance;

    private void Awake()
    {
        tooltipInstance = FindObjectOfType<ToolTip3D>().gameObject;
    }

    public void ShowTooltip2D(Item _item, Vector3 _pos)
    {
        UIManager.Instance.tooltip2D.ShowTooltip2D(_item, _pos);
    }

    public void HideTooltip2D()
    {
        UIManager.Instance.tooltip2D.HideTooltip2D();
    }

    public void ShowTooltip3D(string _itemName, string _itemTip, Sprite _itemImage, Vector3 _pos)
    {
        tooltipInstance.SetActive(true);

        if (UIManager.Instance.tooltipPrefab != null)
        {
            tooltipInstance.transform.position = _pos;

            Image[] tooltipImage = tooltipInstance.GetComponentsInChildren<Image>();
            Text[] tooltipText = tooltipInstance.GetComponentsInChildren<Text>();

            if (tooltipInstance != null)
            {
                tooltipImage[1].sprite = _itemImage;
                tooltipText[0].text = _itemName;
                tooltipText[1].text = _itemTip;
            }
        }
    }
    public void HideTooltip3D()
    {
        tooltipInstance.SetActive(false);
    }
}
