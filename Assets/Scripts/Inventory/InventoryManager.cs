using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryManager : MonoBehaviour
{
    [SerializeField] Inventory inventory;
    [SerializeField] EquipmentWindow equipment;
    public Camera cam;
    public static bool isEquippedItem = false;

    private void Start()
    {
        cam = GameObject.Find("FpsController").GetComponentInChildren<Camera>();
    }

    void Update()
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

            inventory.items.Add(item);
            inventory.AcquireItem(item);

            Destroy(hit.transform.gameObject);
        }
    }

    public void EquipItemFromInventory(Item _item)
    {
        if (_item is Item)
        {
            EquipItem(_item);
        }
    }

    public void UnEquipItemFromEquip(Item _item)
    {
        if (_item is Item)
        {
            UnEquipItem(_item);
        }
    }

    //아이템 장착 해제, 툴팁 정보 표시
    private void EquipItem(Item _item)
    {
        if (_item != null)
        {
            // 장비 아이템일 경우
            if (_item.itemType == Item.ItemType.Equipment)
            {
                GameObject arms = GameObject.Find("LeftArm");
                GameObject weapon = Instantiate(_item.itemPrefab, arms.transform.position, Quaternion.Euler(new Vector3(-30f, 90f, 0f)));
                weapon.transform.SetParent(arms.transform);

                UIManager.Instance.tooltip2D.HideTooltip2D();

                equipment.equipItems.Add(_item);
                equipment.AcquireItem(_item);

                inventory.items.Remove(_item);
                inventory.ReturnItem(_item);

                isEquippedItem = true;
            }
            else
            {
                Debug.Log("장착할 수 없는 타입의 무기");
            }
        }
    }

    private void UnEquipItem(Item _item)
    {
        if (_item != null)
        {
            // 아이템 장착 해제시 모델 삭제 및 아이템 다시 슬롯으로
            // 장비창에서 클릭했을 경우에도 아이템 슬롯으로 이동되게
            GameObject arms = GameObject.Find("LeftArm");
            Destroy(arms.transform.GetChild(0).gameObject);

            inventory.items.Add(_item);
            inventory.AcquireItem(_item);

            equipment.equipItems.Remove(_item);
            equipment.ReturnItem(_item);

            isEquippedItem = false;
        }
    }

    //private void EquipItem(Item _item)
    //{
    //    // 인벤토리에서 항목 제거 후 장비창에 추가
    //    if(inventory.RemoveItem(_item))
    //    {
    //        Item oldItem;
    //        if(equipment.AddItem(_item, out oldItem))
    //        {
    //            if (oldItem != null)
    //            {
    //                inventory.AddItem(oldItem);
    //            }
    //        }
    //        else
    //        {
    //            inventory.AddItem(_item);
    //        }
    //    }
    //}

    //private void UnEquipItem(Item _item)
    //{
    //    // 인벤토리 IsFull 확인
    //    // 장비창에서 제거 후 인벤토리에 추가
    //    if(!inventory.IsFull() && equipment.RemoveItem(_item))
    //    {
    //        inventory.AddItem(_item);
    //    }
    //}
}
