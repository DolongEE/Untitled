using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class TestPlayer : MonoBehaviour
{
    [Header("인벤토리")]
    public Inventory inventory;
    public Camera cam;

    private void Start()
    {
        inventory = FindObjectOfType<Inventory>();
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

            if (Physics.Raycast(ray, out hitInfo))
            {
                HitCheckObject(hitInfo);
            }
        }
    }

    void HitCheckObject(RaycastHit hit)
    {
        IObjectItem clickInterface = hit.transform.gameObject.GetComponent<IObjectItem>();

        if(clickInterface != null)
        {
            Item item = clickInterface.ClickItem();
            Debug.Log($"{item.itemName}");
            inventory.items.Add(item);
            inventory.AcquireItem(item);

            Destroy(hit.transform.gameObject);
        }
    }
}