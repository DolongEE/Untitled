using System.Collections.Generic;
using UnityEngine;

public class InventoryManager
{
    public Inventory inventory;
    [SerializeField] EquipmentInventory equipment;
    public InventoryToolTip toolTip;
    public Camera cam;

    public void Init()
    {
        GameObject canvas = GameObject.Find("Canvas");
        inventory = canvas.GetComponentInChildren<Inventory>();
        equipment = canvas.GetComponentInChildren<EquipmentInventory>();
        toolTip = canvas.GetComponentInChildren<InventoryToolTip>();
        cam = Camera.main;
    }

    public void Update()
    {
        //// UI �̺�Ʈ�� �߻��� ��� ó������ ����
        //if (EventSystem.current.IsPointerOverGameObject() == true)
        //    return;

        //if (Input.GetMouseButtonDown(0))
        //{
        //    Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        //    RaycastHit hitInfo;

        //    if (Physics.Raycast(ray, out hitInfo) && hitInfo.transform.CompareTag("Item"))
        //    {
        //        HitCheckObject(hitInfo);
        //    }
        //}
    }

    //void HitCheckObject(RaycastHit hit)
    //{
    //    IObjectItem clickInterface = hit.transform.gameObject.GetComponent<IObjectItem>();

    //    if (clickInterface != null)
    //    {
    //        Item item = clickInterface.ClickItem();
    //        inventory.AcquireItem(item);
    //        Object.Destroy(hit.transform.gameObject);
    //    }
    //}

    public void EquipItemFromInventory(EquippableItem _item)
    {
        EquippableItem copy = _item.GetCopy() as EquippableItem;
        if (inventory.ReturnItem(_item) && equipment.AcquireItem(copy))
        {
            GameObject weaponPrefab = _item.itemPrefab;

            if (weaponPrefab != null)
            {
                GameObject arms = GameObject.Find("Left Weapon Arm");
                GameObject weapon = Object.Instantiate(weaponPrefab, arms.transform.position, Quaternion.Euler(new Vector3(-30f, 90f, 0f)));

                weapon.transform.SetParent(arms.transform);

                Managers.INVENTORY.toolTip.HideTooltip2D();
                PlayerStatus.Instance.EquipItem(_item);

                copy.Equip();
                Debug.Log("������ ���� ���� : " + copy.IsEquipped);
            }
            else
            {
                Debug.Log("��� �������� �������� �����Ǿ� ���� �ʽ��ϴ�.");
            }
        }
        else
        {
            inventory.AcquireItem(_item);
            Debug.Log("�κ��丮���� �������� �������� �ʾҰų�, ���â�� �߰����� ����.");
        }
    }

    public void UnEquipItemFromEquip(EquippableItem _item)
    {
        // ������ ���� ������ �� ���� �� ������ �ٽ� ��������
        // ���â���� Ŭ������ ��쿡�� ������ �������� �̵��ǰ�
        if (equipment.ReturnItem(_item) && inventory.AcquireItem(_item))
        {
            GameObject arms = GameObject.Find("Left Weapon Arm");
           
            if (arms.transform.childCount > 0)
            {
                Object.Destroy(arms.transform.GetChild(0).gameObject);
            }

            PlayerStatus.Instance.UnequipItem(_item);
            _item.UnEquip();

            Debug.Log("������ ���� ���� : " + _item.IsEquipped);
        }
        else
        {
            equipment.AcquireItem(_item);
            Debug.Log("���â���� �������� �������� �ʾҰų�, �κ��丮�� �߰����� ����.");
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
}