using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryToolTip : MonoBehaviour
{
    public static bool inventoryActivated = false;
    private GameObject noMovingZone;
    private GameObject itemBag;
    private GameObject equipmentBag;
    private GameObject statBag;
    private GameObject header;

    [Header("���� ������Ʈ")]
    [SerializeField] private GameObject slotTooltip;
    public GameObject tooltip;

    private Image itemImage;
    private Text itemName;
    private Text itemDescription;

    private void Awake()
    {
        noMovingZone = transform.Find("NoMovingZone").gameObject;
        itemBag = GameObject.Find("itemBag");
        equipmentBag = GameObject.Find("equipmentBag");
        statBag = GameObject.Find("statBag");
        tooltip = GameObject.Find("tooltip");
        header = GameObject.Find("MovableHeader");

        slotTooltip = GameObject.Find("SlotTooltip");
        itemImage = slotTooltip.transform.Find("imageBG").GetChild(0).GetComponentInChildren<Image>();
        itemName = slotTooltip.transform.Find("nameBG").GetComponentInChildren<Text>();
        itemDescription = slotTooltip.transform.Find("descBG").GetComponentInChildren<Text>();
    }

    private void Start()
    {
        noMovingZone.SetActive(false);
        tooltip.SetActive(false);
        slotTooltip.SetActive(false);
        itemBag.SetActive(false);
        equipmentBag.SetActive(false);
        statBag.SetActive(false);      
        header.SetActive(false);
    }

    private void Update()
    {
        InventoryUIOnOff();
    }

    private void InventoryUIOnOff()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryActivated = !inventoryActivated;

            noMovingZone.SetActive(inventoryActivated);
            itemBag.SetActive(inventoryActivated);
            equipmentBag.SetActive(inventoryActivated);
            statBag.SetActive(inventoryActivated);
            header.SetActive(inventoryActivated);

            if (inventoryActivated == false)
            {
                HideTooltip2D();
            }
        }
    }

    public void ShowTooltip2D(Item _item, Vector3 _pos)
    {
        slotTooltip.SetActive(true);

        if (inventoryActivated)
        {
            float width = slotTooltip.GetComponent<RectTransform>().rect.width;
            float height = slotTooltip.GetComponent<RectTransform>().rect.height;

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

            slotTooltip.transform.position = _pos;
        }
        else
        {
            slotTooltip.transform.position = Input.mousePosition;
        }

        itemImage.sprite = _item.itemImage;
        itemName.text = _item.itemName;
        itemDescription.text = _item.itemDescription;
    }

    public void HideTooltip2D()
    {
        slotTooltip.SetActive(false);
    }
}