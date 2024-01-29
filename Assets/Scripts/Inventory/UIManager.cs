using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Progress;

//public class UIManager : MonoBehaviour
//{
//    //public static UIManager instance { get; private set; }


//    private void Awake()
//    {
//        if (instance != null)
//        {
//            Debug.LogError("Found more than one Game Events Manager in the scene.");
//        }
//        instance = this;

//        // initialize all events
//    }
//}
public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<UIManager>();
                if (_instance == null)
                {
                    GameObject go = new GameObject("UIManager");
                    _instance = go.AddComponent<UIManager>();
                }
            }
            return _instance;
        }
    }

    public InventoryManager inventoryManager;
    public Inventory inventory;
    public GameObject tooltipPrefab;
    public slotTooltip tooltip;

    private void Awake()
    {
        if (_instance != null)
        {
            Debug.LogError("Found more than one Game Events Manager in the scene.");
        }
        _instance = this;

        // initialize all events
        inventoryManager = new InventoryManager();
    }

    public GameObject MakeToolTip()
    {
        GameObject tooltipInstance = Instantiate(tooltipPrefab, Vector3.zero, Quaternion.identity);

        return tooltipInstance;
    }

    public GameObject MakeItem(Item _item, GameObject arm)
    {
        GameObject equipmentItem = Instantiate(_item.itemPrefab, arm.transform.position, Quaternion.Euler(-20f, 90f, 45f));

        return equipmentItem;
    }

    public void DestroyObject(GameObject go)
    {
        Destroy(go);
    }
}