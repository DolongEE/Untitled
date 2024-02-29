using System.Collections.Generic;
using UnityEngine;

public class InventoryManager
{
    public Inventory inventory;
    [SerializeField] EquipmentInventory equipment;
    public InventoryToolTip toolTip;
    public Camera cam;
    public bool isEquippedWeapon;

    public void Init()
    {
        GameObject canvas = GameObject.Find("Canvas");
        inventory = canvas.GetComponentInChildren<Inventory>();
        equipment = canvas.GetComponentInChildren<EquipmentInventory>();
        toolTip = canvas.GetComponentInChildren<InventoryToolTip>();
        cam = Camera.main;
        isEquippedWeapon = false;
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
                GameObject weapon = Object.Instantiate(weaponPrefab, arms.transform.position, Quaternion.identity, arms.transform);

                weapon.name = _item.name;
                weapon.GetComponent<BoxCollider>().center = new Vector3(0f, 0.85f, 0f);
                weapon.GetComponent<BoxCollider>().size = new Vector3(0.2f, 2.3f, 0.2f);
                weapon.GetComponent<ItemInfo>().enabled = false;
                
                Managers.INVENTORY.toolTip.HideTooltip2D();
                PlayerStatus.Instance.EquipItem(_item);
                copy.Equip();
                
                isEquippedWeapon = true;
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
           
            isEquippedWeapon = false;
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