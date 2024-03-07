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
                // 장비 아이템이 무기일 경우
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
                    Debug.Log("무기 아이템 장착 상태 : " + copy.IsEquipped);
                }
                // 장비 아이템이 헬멧일 경우
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

                    Debug.Log("헬멧 아이템 장착 상태 : " + copy.IsEquipped);
                }
                // 장비 아이템이 갑옷일 경우
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

                    Debug.Log("갑옷 아이템 장착 상태 : " + copy.IsEquipped);
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

                    Debug.Log("갑옷 아이템 장착 상태 : " + copy.IsEquipped);
                }
                else
                {
                    Debug.Log("장비 아이템의 타입이 다릅니다.");
                }
            }
            else
            {
                Debug.Log("장비 아이템의 프리팹이 설정되어 있지 않습니다.");
            }
        }
        else
        {
            inventory.AcquireItem(_item);
            Debug.Log("인벤토리에서 아이템이 삭제되지 않았거나, 장비창에 추가되지 않음.");
        }
    }

    public void UnEquipItemFromEquip(EquippableItem _item)
    {
        // 아이템 장착 해제시 모델 삭제 및 아이템 다시 슬롯으로
        // 장비창에서 클릭했을 경우에도 아이템 슬롯으로 이동되게
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
                Debug.Log("아이템 장착 상태 : " + _item.IsEquipped);
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
                Debug.Log("아이템 장착 상태 : " + _item.IsEquipped);
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
                Debug.Log("아이템 장착 상태 : " + _item.IsEquipped);
            }
        }
        else
        {
            equipment.AcquireItem(_item);
            Debug.Log("장비창에서 아이템이 삭제되지 않았거나, 인벤토리에 추가되지 않음.");
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