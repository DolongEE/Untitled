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
        // UI 이벤트가 발생한 경우 처리하지 않음
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
                Debug.Log("아이템 장착 상태 : " + isEquippedItem);
            }
            else
            {
                Debug.Log("장비 아이템의 프리팹이 설정되어 있지 않습니다.");
            }
        }
        else
        {
            Debug.Log("장비가 아니라 장착할 수 없습니다.");
        }
    }

    public void UnEquipItemFromEquip(EquippableItem _item)
    {
        equipment.ReturnItem(_item);
        inventory.AcquireItem(_item);
        // 아이템 장착 해제시 모델 삭제 및 아이템 다시 슬롯으로
        // 장비창에서 클릭했을 경우에도 아이템 슬롯으로 이동되게
        if (_item is EquippableItem)
        {
            GameObject arms = GameObject.Find("Left Weapon Arm");
            if (arms.transform.childCount > 0)
            {
                Object.Destroy(arms.transform.GetChild(0).gameObject);
            }

            PlayerStatus.Instance.UnequipItem(_item);

            isEquippedItem = false;
            Debug.Log("아이템 장착 상태 : " + isEquippedItem);
        }
        else
        {
            Debug.Log("어라.. 장비가 아닌데 장착 슬롯에 갔네..");
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
    //    // 아이템 버리기 기능 오류 문제로 보류
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