using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolTip2D : MonoBehaviour
{
    [Header("���� ������Ʈ")]
    [SerializeField] private GameObject tooltip;

    [SerializeField] private Image itemImage;
    [SerializeField] private Text itemName;
    [SerializeField] private Text itemDescription;

    public void ShowTooltip2D(Item _item, Vector3 _pos)
    {
        tooltip.SetActive(true);

        if(UIManager.inventoryActivated)
        {
            float width = tooltip.GetComponent<RectTransform>().rect.width;
            float height = tooltip.GetComponent<RectTransform>().rect.height;

            Canvas canvas = FindObjectOfType<Canvas>();

            // ĵ������ ���߾��� ���� ��ǥ�� ��ȯ
            Vector3 canvasCenter = canvas.transform.position;

            // ������� ��ġ ���
            Vector3 relativePos = _pos - canvasCenter;

            // ĵ������ 4����Ͽ� ��ġ ����
            if (relativePos.x > 0 && relativePos.y > 0)
            {
                _pos -= new Vector3(width * 0.5f, height * 0.5f, 0); // 1��и�
            }

            else if (relativePos.x < 0 && relativePos.y > 0)
            {
                _pos += new Vector3(width * 0.5f, height * 0.5f, 0); // 2��и�
            }

            else if (relativePos.x < 0 && relativePos.y < 0)
            {
                _pos += new Vector3(width * 0.5f, -height * 0.5f, 0); // 3��и�
            }

            else
            {
                _pos -= new Vector3(width * 0.5f, -height * 0.5f, 0); // 4��и�
            }

            tooltip.transform.position = _pos;
        }
        else
        {
            tooltip.transform.position = Input.mousePosition;
        }

        itemImage.sprite = _item.itemImage;
        itemName.text = _item.itemName;
        itemDescription.text = _item.itemDescription;
    }

    public void HideTooltip2D()
    {
        tooltip.SetActive(false);
    }
}
