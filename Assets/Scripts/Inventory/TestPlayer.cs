using UnityEngine;
using UnityEngine.EventSystems;

public class TestPlayer : MonoBehaviour
{
    [Header("인벤토리")]
    public Inventory inventory;
    public Camera cam;

    void Update()
    {
        // UI 이벤트가 발생한 경우 처리하지 않음
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hitInfo;

            if (Physics.Raycast(ray, out hitInfo))
            {
                HitCheckObject(hitInfo);
            }
            else
            {
                Vector2 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

                if (hit.collider != null)
                {
                    HitCheckObject(hit);
                }
            }
        }
    }

    void HitCheckObject(RaycastHit hit)
    {
        IObjectItem clickInterface = hit.transform.gameObject.GetComponent<IObjectItem>();

        if (clickInterface != null)
        {
            Item item = clickInterface.ClickItem();
            Debug.Log($"{item.itemName}");
            inventory.AddItem(item);

            hit.transform.gameObject.SetActive(false);
        }
    }

    void HitCheckObject(RaycastHit2D hit)
    {
        IObjectItem clickInterface = hit.transform.gameObject.GetComponent<IObjectItem>();

        if (clickInterface != null)
        {
            Item item = clickInterface.ClickItem();
            Debug.Log($"{item.itemName}");
            inventory.AddItem(item);

            hit.transform.gameObject.SetActive(false);
        }
    }
}