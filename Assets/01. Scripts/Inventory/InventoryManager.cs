using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InventoryManager
{
    public Inventory inventory;
    [SerializeField] public EquipmentInventory equipment;
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

    }

    public void EquipItemFromInventory(EquippableItem _item)
    {
        GameObject weaponPrefab = _item.itemPrefab;

        if (inventory.ReturnItem(_item) && equipment.AcquireItem(_item) && weaponPrefab != null)
        {
            // ��� �������� ������ ���
            if (_item.wItemType == EItemType.Weapon)
            {
                GameObject hand = GameObject.Find("Left Weapon Hand");
                GameObject weapon = Object.Instantiate(weaponPrefab, hand.transform.position, hand.transform.rotation, hand.transform);

                weapon.name = _item.name;
                weapon.GetComponent<ItemInfo>().enabled = false;
                weapon.GetComponentInChildren<CapsuleCollider>().enabled = false;

                Managers.INVENTORY.toolTip.HideTooltip2D();
                PlayerStatus.Instance.EquipItem(_item);
                _item.Equip();

                isEquippedWeapon = true;
            }
            // ��� �������� ����� ���
            else if (_item.wItemType == EItemType.Helmet)
            {
                GameObject head = GameObject.Find("Head_end");
                GameObject helmet = Object.Instantiate(weaponPrefab, head.transform.position, head.transform.rotation, head.transform);

                helmet.name = _item.name;
                helmet.GetComponent<ItemInfo>().enabled = false;

                Managers.INVENTORY.toolTip.HideTooltip2D();
                PlayerStatus.Instance.EquipItem(_item);
                _item.Equip();
            }
            // ��� �������� ������ ���
            else if (_item.wItemType == EItemType.Armor)
            {
                GameObject spine = GameObject.Find("Spine 2");
                GameObject armor = Object.Instantiate(weaponPrefab, spine.transform.position, Quaternion.Euler(90f, spine.transform.rotation.y, spine.transform.rotation.z), spine.transform);

                armor.name = _item.name;
                armor.GetComponent<ItemInfo>().enabled = false;

                Managers.INVENTORY.toolTip.HideTooltip2D();
                PlayerStatus.Instance.EquipItem(_item);
                _item.Equip();
            }
            else if (_item.wItemType == EItemType.Tools)
            {
                GameObject hand = GameObject.Find("Right Tool Hand");
                GameObject tool = Object.Instantiate(weaponPrefab, hand.transform.position, hand.transform.rotation, hand.transform);

                tool.name = _item.name;
                tool.GetComponent<ItemInfo>().enabled = false;
                tool.GetComponentInChildren<CapsuleCollider>().enabled = false;

                Managers.INVENTORY.toolTip.HideTooltip2D();
                PlayerStatus.Instance.EquipItem(_item);
                _item.Equip();
            }
        }
        else
        {
            inventory.AcquireItem(_item);
        }
    }

    public void UnEquipItemFromEquip(EquippableItem _item)
    {
        // ������ ���� ������ �� ���� �� ������ �ٽ� ��������
        // ���â���� Ŭ������ ��쿡�� ������ �������� �̵��ǰ�
        if (equipment.ReturnItem(_item) && inventory.AcquireItem(_item))
        {
            if (_item.wItemType == EItemType.Weapon)
            {
                GameObject lHand = GameObject.Find("Left Weapon Hand");

                if (lHand.transform.childCount > 0)
                {
                    Object.Destroy(lHand.transform.GetChild(0).gameObject);
                }

                PlayerStatus.Instance.UnequipItem(_item);
                _item.UnEquip();

                isEquippedWeapon = false;
            }
            else if (_item.wItemType == EItemType.Helmet)
            {
                GameObject head = GameObject.Find("Head_end");

                if (head.transform.childCount > 0)
                {
                    Object.Destroy(head.transform.GetChild(0).gameObject);
                }

                PlayerStatus.Instance.UnequipItem(_item);
                _item.UnEquip();

                isEquippedWeapon = false;
            }
            else if (_item.wItemType == EItemType.Armor)
            {
                GameObject spine = GameObject.Find("Spine 2");

                if (spine.transform.childCount > 0)
                {
                    Object.Destroy(spine.transform.GetChild(1).gameObject);
                }

                PlayerStatus.Instance.UnequipItem(_item);
                _item.UnEquip();

                isEquippedWeapon = false;
            }
            else if (_item.wItemType == EItemType.Tools)
            {
                GameObject rHand = GameObject.Find("Right Tool Hand");

                if (rHand.transform.childCount > 0)
                {
                    Object.Destroy(rHand.transform.GetChild(0).gameObject);
                }

                PlayerStatus.Instance.UnequipItem(_item);
                _item.UnEquip();

                isEquippedWeapon = false;
            }
        }
        else
        {
            equipment.AcquireItem(_item);
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