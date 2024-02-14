using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ToolTip2D : MonoBehaviour
{
    [Header("툴팁 오브젝트")]
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

            // 캔버스의 정중앙을 월드 좌표로 변환
            Vector3 canvasCenter = canvas.transform.position;

            // 상대적인 위치 계산
            Vector3 relativePos = _pos - canvasCenter;

            // 캔버스를 4등분하여 위치 조절
            if (relativePos.x > 0 && relativePos.y > 0)
            {
                _pos -= new Vector3(width * 0.5f, height * 0.5f, 0); // 1사분면
            }

            else if (relativePos.x < 0 && relativePos.y > 0)
            {
                _pos += new Vector3(width * 0.5f, height * 0.5f, 0); // 2사분면
            }

            else if (relativePos.x < 0 && relativePos.y < 0)
            {
                _pos += new Vector3(width * 0.5f, -height * 0.5f, 0); // 3사분면
            }

            else
            {
                _pos -= new Vector3(width * 0.5f, -height * 0.5f, 0); // 4사분면
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
