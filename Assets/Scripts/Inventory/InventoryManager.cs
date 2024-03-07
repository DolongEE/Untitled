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

    }

    public void EquipItemFromInventory(EquippableItem _item)
    {
        EquippableItem copy = _item.GetCopy() as EquippableItem;
        if (inventory.ReturnItem(_item) && equipment.AcquireItem(copy))
        {
            GameObject weaponPrefab = _item.itemPrefab;

            if (weaponPrefab != null)
            {
                // ��� �������� ������ ���
                if(_item.wItemType == EItemType.Weapon)
                {
                    GameObject hand = GameObject.Find("Left Weapon Hand");
                    GameObject weapon = Object.Instantiate(weaponPrefab, hand.transform.position, hand.transform.rotation, hand.transform);

                    weapon.name = _item.name;
                    weapon.GetComponent<ItemInfo>().enabled = false;
                    weapon.GetComponentInChildren<CapsuleCollider>().enabled = false;

                    Managers.INVENTORY.toolTip.HideTooltip2D();
                    PlayerStatus.Instance.EquipItem(_item);
                    copy.Equip();

                    isEquippedWeapon = true;
                    Debug.Log("���� ������ ���� ���� : " + copy.IsEquipped);
                }
                // ��� �������� ����� ���
                else if(_item.wItemType == EItemType.Helmet)
                {
                    GameObject head = GameObject.Find("Head_end");
                    GameObject helmet = Object.Instantiate(weaponPrefab, head.transform.position, Quaternion.identity, head.transform);

                    helmet.name = _item.name;
                    helmet.GetComponent<ItemInfo>().enabled = false;
                    helmet.transform.localScale = new Vector3(0.05f, 0.05f, 0.05f);

                    Managers.INVENTORY.toolTip.HideTooltip2D();
                    PlayerStatus.Instance.EquipItem(_item);
                    copy.Equip();

                    Debug.Log("��� ������ ���� ���� : " + copy.IsEquipped);
                }
                // ��� �������� ������ ���
                else if (_item.wItemType == EItemType.Armor)
                {
                    GameObject spine = GameObject.Find("Spine 2");
                    GameObject armor = Object.Instantiate(weaponPrefab, spine.transform.position, Quaternion.identity, spine.transform);

                    armor.name = _item.name;
                    armor.GetComponent<ItemInfo>().enabled = false;
                    armor.transform.localScale = new Vector3(0.08f, 0.1f, 0.1f);

                    Managers.INVENTORY.toolTip.HideTooltip2D();
                    PlayerStatus.Instance.EquipItem(_item);
                    copy.Equip();

                    Debug.Log("���� ������ ���� ���� : " + copy.IsEquipped);
                }
                else if(_item.wItemType == EItemType.Tools)
                {
                    GameObject hand = GameObject.Find("Right Tool Hand");
                    GameObject tool = Object.Instantiate(weaponPrefab, hand.transform.position, hand.transform.rotation, hand.transform);

                    tool.name = _item.name;
                    tool.GetComponent<ItemInfo>().enabled = false;

                    Managers.INVENTORY.toolTip.HideTooltip2D();
                    PlayerStatus.Instance.EquipItem(_item);
                    copy.Equip();

                    Debug.Log("���� ������ ���� ���� : " + copy.IsEquipped);
                }
                else
                {
                    Debug.Log("��� �������� Ÿ���� �ٸ��ϴ�.");
                }
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
            if(_item.wItemType == EItemType.Weapon || _item.wItemType == EItemType.Tools)
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
            else if(_item.wItemType == EItemType.Helmet)
            {
                GameObject head = GameObject.Find("Head_end");

                if (head.transform.childCount > 0)
                {
                    Object.Destroy(head.transform.GetChild(0).gameObject);
                }

                PlayerStatus.Instance.UnequipItem(_item);
                _item.UnEquip();

                isEquippedWeapon = false;
                Debug.Log("������ ���� ���� : " + _item.IsEquipped);
            }
            else if (_item.wItemType == EItemType.Armor)
            {
                GameObject spine = GameObject.Find("Spine 2");

                if (spine.transform.childCount > 0)
                {
                    Object.Destroy(GameObject.Find("Armor"));
                }

                PlayerStatus.Instance.UnequipItem(_item);
                _item.UnEquip();

                isEquippedWeapon = false;
                Debug.Log("������ ���� ���� : " + _item.IsEquipped);
            }
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