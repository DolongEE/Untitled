using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;
    #region Property
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
    #endregion
    public static bool inventoryActivated = false;

    //public InventoryPanel inventory;
    //public EquipmentSlot equipSlot;
    //public InventoryManager inventoryManager;
    public GameObject itemBag;
    public GameObject equipmentBag;
    public GameObject statBag;

    public GameObject tooltipPrefab;
    public ToolTip2D tooltip2D;
    public ToolTip3D tooltip3D;

    private void Awake()
    {
        if (_instance != null)
        {
            Debug.LogError("Found more than one Game Events Manager in the scene.");
        }
        _instance = this;

        // initialize all events
        //inventoryManager = FindObjectOfType<InventoryManager>();
        itemBag = GameObject.Find("itemBag");
        equipmentBag = GameObject.Find("equipmentBag");
        statBag = GameObject.Find("statBag");
    }

    private void Start()
    {
        itemBag.SetActive(false);
        equipmentBag.SetActive(false);
        statBag.SetActive(false);
    }

    private void Update()
    {
        InventoryUIOnOff();
    }

    private void InventoryUIOnOff()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            inventoryActivated = !inventoryActivated;

            if (inventoryActivated)
            {
                itemBag.SetActive(true);
                equipmentBag.SetActive(true);
                statBag.SetActive(true);
            }
            else
            {
                itemBag.SetActive(false);
                equipmentBag.SetActive(false);
                statBag.SetActive(false);
            }
        }
    }
}