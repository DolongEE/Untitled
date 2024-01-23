using Palmmedia.ReportGenerator.Core.Reporting.Builders;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    [Header("인벤토리")]
    public Inventory inventory;
    [Header("가방")]
    public GameObject bag;

    void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if(hit.collider != null)
            {
                HitCheckObject(hit);
            }
        }

        if(Input.GetKeyDown(KeyCode.I))
        {
            bag.SetActive(!bag.activeSelf);
        }
    }

    void HitCheckObject(RaycastHit2D hit)
    {
        IObjectItem clickInterface = hit.transform.gameObject.GetComponent<IObjectItem>();

        if(clickInterface != null)
        {
            Item item = clickInterface.ClickItem();
            Debug.Log($"{item.itemName}");
            inventory.AddItem(item);

            hit.transform.gameObject.SetActive(false);
        }
    }

    #region 마우스가 가리키는 곳
    //private void OnDrawGizmos()
    //{
    //    Vector3 mousePosition = Input.mousePosition;
    //    mousePosition.z = Camera.main.nearClipPlane;
    //    Vector3 worldMousePosition = Camera.main.ScreenToWorldPoint(mousePosition);

    //    Gizmos.color = Color.red;

    //    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
    //    Gizmos.DrawLine(ray.origin, ray.origin + ray.direction * 10f);
    //}
    #endregion
}
