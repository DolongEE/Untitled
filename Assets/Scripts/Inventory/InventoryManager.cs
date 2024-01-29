using UnityEngine;
using UnityEngine.UI;

public class InventoryManager
{
    private GameObject tooltipInstance;

    public void HitCheckObject(RaycastHit hit)
    {
        IObjectItem clickInterface = hit.transform.gameObject.GetComponent<IObjectItem>();

        if (clickInterface != null)
        {
            Item item = clickInterface.ClickItem();

            if (item.IsAcquire == false)
            {
                UIManager.Instance.inventory.items.Add(item);
                UIManager.Instance.inventory.AcquireItem(item);
            }
        }
    }

    public void EquipItem(Item _item)
    {
        if (_item != null)
        {
            if (_item.itemType == Item.ItemType.Equipment)
            {
                // 아이템 장착
                GameObject arms = GameObject.Find("LeftArm");
                GameObject weapon = UIManager.Instance.MakeItem(_item, arms);
                weapon.transform.SetParent(arms.transform);

                HideTooltip2D();
            }
            else
            {
                Debug.Log("장비 아이템이 아닙니다.");
            }
        }
    }

    public void UnEquipItem()
    {

    }

    public void ShowTooltip2D(Item item, Vector3 position)
    {
        if(item.IsEquip == false)
        {
            UIManager.Instance.tooltip.ShowTooltip(item, position);
        }
    }

    public void HideTooltip2D()
    {
        UIManager.Instance.tooltip.HideTooltip();
    }

    public void ShowTooltip3D(string itemName, string itemTooltip, Sprite itemImage, Vector3 position)
    {
        if (UIManager.Instance.tooltipPrefab != null)
        {
            tooltipInstance = UIManager.Instance.MakeToolTip();
            tooltipInstance.transform.position = position;

            Image[] tooltipImage = tooltipInstance.GetComponentsInChildren<Image>();
            Text[] tooltipText = tooltipInstance.GetComponentsInChildren<Text>();

            if (tooltipInstance != null)
            {
                tooltipImage[1].sprite = itemImage;
                tooltipText[0].text = itemName;
                tooltipText[1].text = itemTooltip;
            }
        }
    }
    public void HideTooltip3D()
    {
        if (tooltipInstance != null)
        {
            UIManager.Instance.DestroyObject(tooltipInstance);
            tooltipInstance = null;
        }
    }
}


//public class InventoryManager : MonoBehaviour 
//{
//    //private static InventoryManager _instance;
//    //public static InventoryManager instance
//    //{
//    //    get
//    //    {
//    //        if (_instance == null)
//    //            _instance = FindObjectOfType<InventoryManager>();

//    //        return _instance;
//    //    }
//    //}
//    public Inventory inventory;
//    private GameObject tooltipInstance;

//    public void ShowTooltip2D(Item _item, Vector3 _pos)
//    {
//        UIManager.instance.tooltip.ShowTooltip(_item, _pos);
//    }
//    public void HideTooltip2D()
//    {
//        UIManager.instance.tooltip.HideTooltip();
//    }
//    public void ShowTooltip3D(string itemName, string itemTooltip, Sprite itemImage, Vector3 position)
//    {
//        if (UIManager.instance.tooltipPrefab != null)
//        {
//            tooltipInstance = Instantiate(UIManager.instance.tooltipPrefab, position, Quaternion.Euler(30f, 0f, 0f));

//            Image[] tooltipImage = tooltipInstance.GetComponentsInChildren<Image>();
//            Text[] tooltipText = tooltipInstance.GetComponentsInChildren<Text>();

//            if (tooltipInstance != null)
//            {
//                tooltipImage[1].sprite = itemImage;
//                tooltipText[0].text = itemName;
//                tooltipText[1].text = itemTooltip;
//            }
//        }
//    }
//    public void HideTooltip3D()
//    {
//        if (tooltipInstance != null)
//        {
//            Destroy(tooltipInstance);
//            tooltipInstance = null;
//        }
//    }
//}