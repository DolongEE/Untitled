using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.HID;

public class TestPlayer : MonoBehaviour
{
    [Header("�κ��丮")]
    public Inventory inventory;
    public Camera cam;

    private void Start()
    {
        inventory = FindObjectOfType<Inventory>();
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
                UIManager.Instance.inventoryManager.HitCheckObject(hitInfo);
                Destroy(hitInfo.transform.gameObject);
            }
        }
    }

    //void HitCheckObject(RaycastHit hit)
    //{
    //    IObjectItem clickInterface = hit.transform.gameObject.GetComponent<IObjectItem>();

    //    if(clickInterface != null)
    //    {
    //        Item item = clickInterface.ClickItem();

    //        if(item.IsAcquire == false)
    //        {
    //            inventory.items.Add(item);
    //            inventory.AcquireItem(item);
    //        }

    //        Destroy(hit.transform.gameObject);
    //    }
    //}
}