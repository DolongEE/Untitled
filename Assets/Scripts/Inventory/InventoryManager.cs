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
        //// UI 이벤트가 발생한 경우 처리하지 않음
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
                Debug.Log("아이템 장착 상태 : " + copy.IsEquipped);
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