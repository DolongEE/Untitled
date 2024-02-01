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
        cam = Camera.main;
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

    public void EquipItemFromInventory(EquippableItem _item)
    {
        if (_item is EquippableItem)
        {
            EquipItem(_item);
            Debug.Log("������ ����");
        }
    }

    public void UnEquipItemFromEquip(EquippableItem _item)
    {
        if (_item is EquippableItem)
        {
            UnEquipItem(_item);
            Debug.Log("������ ����");
        }
    }

    //������ ���� ����, ���� ���� ǥ��
    private void EquipItem(EquippableItem _item)
    {
        if (_item != null)
        {
            // ��� �������� ���
            GameObject arms = GameObject.Find("LeftArm");
            GameObject weapon = Instantiate(_item.itemPrefab, arms.transform.position, Quaternion.Euler(new Vector3(-30f, 90f, 0f)));
            weapon.transform.SetParent(arms.transform);

            UIManager.Instance.tooltip2D.HideTooltip2D();

            inventory.ReturnItem(_item);
            inventory.items.Remove(_item);

            equipment.equipItems.Add(_item);
            equipment.AcquireItem(_item);

            PlayerStatus.Instance.EquipItem(_item);

            isEquippedItem = true;
        }
    }

    private void UnEquipItem(EquippableItem _item)
    {
        if (_item != null)
        {
            // ������ ���� ������ �� ���� �� ������ �ٽ� ��������
            // ���â���� Ŭ������ ��쿡�� ������ �������� �̵��ǰ�
            GameObject arms = GameObject.Find("LeftArm");
            if (arms.transform.childCount > 0)
            {
                Destroy(arms.transform.GetChild(0).gameObject);
            }

            equipment.ReturnItem(_item);
            equipment.equipItems.Remove(_item);

            inventory.items.Add(_item);
            inventory.AcquireItem(_item);

            PlayerStatus.Instance.UnequipItem(_item);

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
