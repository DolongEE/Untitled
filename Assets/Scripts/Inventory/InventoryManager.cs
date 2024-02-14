using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryManager
{
    [SerializeField] Inventory inventory;
    [SerializeField] EquipmentInventory equipment;
    public InventoryToolTip toolTip;
    public Camera cam;
    public bool isEquippedItem;

    public void Init()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        inventory = player.GetComponentInChildren<Inventory>();
        equipment = player.GetComponentInChildren<EquipmentInventory>();
        toolTip = player.GetComponentInChildren<InventoryToolTip>();
        cam = Camera.main;
        isEquippedItem = false;
    }

    public void Update()
    {
        // UI �̺�Ʈ�� �߻��� ��� ó������ ����
        if (EventSystem.current.IsPointerOverGameObject() == true)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo) && hitInfo.transform.CompareTag("Item"))
            {
                HitCheckObject(hitInfo);
            }
        }
    }

    void HitCheckObject(RaycastHit hit)
    {
        IObjectItem clickInterface = hit.transform.gameObject.GetComponent<IObjectItem>();

        if (clickInterface != null)
        {
            Item item = clickInterface.ClickItem();
            inventory.AcquireItem(item);
            Object.Destroy(hit.transform.gameObject);
        }
    }

    public void EquipItemFromInventory(EquippableItem _item)
    {
        inventory.ReturnItem(_item);
        equipment.AcquireItem(_item);

        if (_item is EquippableItem)
        {
            GameObject weaponPrefab = _item.itemPrefab;

            if(weaponPrefab != null)
            {
                GameObject arms = GameObject.Find("Left Weapon Arm");
                GameObject weapon = Object.Instantiate(weaponPrefab, arms.transform.position, Quaternion.Euler(new Vector3(-30f, 90f, 0f)));
                
                weapon.transform.SetParent(arms.transform);

                Managers.INVENTORY.toolTip.HideTooltip2D();
                PlayerStatus.Instance.EquipItem(_item);

                isEquippedItem = true;
                Debug.Log("������ ���� ���� : " + isEquippedItem);
            }
            else
            {
                Debug.Log("��� �������� �������� �����Ǿ� ���� �ʽ��ϴ�.");
            }
        }
        else
        {
            Debug.Log("��� �ƴ϶� ������ �� �����ϴ�.");
        }
    }

    public void UnEquipItemFromEquip(EquippableItem _item)
    {
        equipment.ReturnItem(_item);
        inventory.AcquireItem(_item);
        // ������ ���� ������ �� ���� �� ������ �ٽ� ��������
        // ���â���� Ŭ������ ��쿡�� ������ �������� �̵��ǰ�
        if (_item is EquippableItem)
        {
            GameObject arms = GameObject.Find("Left Weapon Arm");
            if (arms.transform.childCount > 0)
            {
                Object.Destroy(arms.transform.GetChild(0).gameObject);
            }

            PlayerStatus.Instance.UnequipItem(_item);

            isEquippedItem = false;
            Debug.Log("������ ���� ���� : " + isEquippedItem);
        }
        else
        {
            Debug.Log("���.. ��� �ƴѵ� ���� ���Կ� ����..");
        }
    }

    public void UseItem(UsableItem _item)
    {
        if (_item != null)
        {
            PlayerStatus.Instance.UseItem(_item);
            inventory.ReturnItem(_item);
            Managers.INVENTORY.toolTip.HideTooltip2D();
        }
    }

    public List<Item> GetItemInfo()
    {
        return inventory.items;
    }

    //public void DropItem(Item _item)
    //{
    //    // ������ ������ ��� ���� ������ ����
    //    //if (DragSlot.instance.transform.localPosition.x < baseRect.xMin ||
    //    //    DragSlot.instance.transform.localPosition.x > baseRect.xMax ||
    //    //    DragSlot.instance.transform.localPosition.y < baseRect.yMin ||
    //    //    DragSlot.instance.transform.localPosition.y > baseRect.yMax)
    //    //{
    //    //    Vector3 itemPos = GameObject.Find("Player").transform.position;

    //    //    Instantiate(DragSlot.instance.dragSlot.item.itemPrefab,
    //    //        itemPos + new Vector3(0f, 0f, 2f), Quaternion.Euler(90f, 0, 0));

    //    //    inventory.ReturnItem(_item);
    //    //}

    //    DragSlot.instance.SetColor(0);
    //    DragSlot.instance.dragSlot = null;
    //    inventory.ReturnItem(_item);
    //}
}