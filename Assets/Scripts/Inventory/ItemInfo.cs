using UnityEngine;

//public interface IObjectItem
//{
//    Item ClickItem();
//}

public class ItemInfo : MonoBehaviour//, IObjectItem
{
    public Item item;
    public Sprite itemImage;
    public string itemName;
    public string itemTooltip;

    private bool isPlayerNear = false;

    void Start()
    {
        itemName = item.itemName;
        itemTooltip = item.itemDescription;
        itemImage = item.itemImage;
    }

    private void OnEnable()
    {
        Managers.EVENT.inputEvents.onQuestLogTogglePressed += TogglePressed;
    }
    private void OnDisable()
    {
        Managers.EVENT.inputEvents.onQuestLogTogglePressed -= TogglePressed;
    }

    public void TogglePressed()
    {
        if (isPlayerNear == false)
            return;
        if (Managers.INVENTORY.inventory.IsInventoryFull() == true)
            return;

        if (Managers.INVENTORY.inventory.AcquireItem(item))
        {
            Managers.INVENTORY.toolTip.tooltip.SetActive(false);
            Destroy(this.gameObject);
        }
    }
    //private void OnMouseEnter()
    //{
    //    Managers.INVENTORY.toolTip.ShowTooltip2D(item, Input.mousePosition);
    //}

    //private void OnMouseExit()
    //{
    //    Managers.INVENTORY.toolTip.HideTooltip2D();
    //}
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && Managers.INVENTORY.isEquippedItem == false)
        {
            isPlayerNear = true;
            Managers.INVENTORY.toolTip.tooltip.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && Managers.INVENTORY.isEquippedItem == false)
        {
            isPlayerNear = false;
            Managers.INVENTORY.toolTip.tooltip.SetActive(false);
        }
    }
}