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

    //������ ���� ����, ���� ���� ǥ��
    private void EquipItem(Item _item)
    {
        if (_item != null)
        {
            // ��� �������� ���
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
                Debug.Log("������ �� ���� Ÿ���� ����");
            }
        }
    }

    private void UnEquipItem(Item _item)
    {
        if (_item != null)
        {
            // ������ ���� ������ �� ���� �� ������ �ٽ� ��������
            // ���â���� Ŭ������ ��쿡�� ������ �������� �̵��ǰ�
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
    //    // �κ��丮���� �׸� ���� �� ���â�� �߰�
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
    //    // �κ��丮 IsFull Ȯ��
    //    // ���â���� ���� �� �κ��丮�� �߰�
    //    if(!inventory.IsFull() && equipment.RemoveItem(_item))
    //    {
    //        inventory.AddItem(_item);
    //    }
    //}
}
