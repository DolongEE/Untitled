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
    [SerializeField] private bool isPlayerPickUP = false;

    private void Start()
    {
        itemName = item.itemName;
        itemTooltip = item.itemDescription;
        itemImage = item.itemImage;
    }

    private void OnEnable()
    {
        Managers.EVENT.inputEvents.onToggleGPressed += TogglePressed;
    }
    private void OnDisable()
    {
        Managers.EVENT.inputEvents.onToggleGPressed -= TogglePressed;
    }

    public void TogglePressed()
    {
        if (isPlayerNear == false)
            return;
        if (Managers.INVENTORY.inventory.IsInventoryFull() == true)
            return;
        if (isPlayerPickUP == true)
            return;

        if (Managers.INVENTORY.inventory.AcquireItem(item))
        {
            Managers.INVENTORY.toolTip.tooltip.SetActive(false);
            isPlayerPickUP = true;
            Destroy(this.gameObject, 0.25f);
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
        if (other.CompareTag("Player") && isPlayerPickUP == false)
        {
            isPlayerNear = true;
            Managers.INVENTORY.toolTip.tooltip.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && isPlayerPickUP == true)
        {
            isPlayerNear = false;
            Managers.INVENTORY.toolTip.tooltip.SetActive(false);
        }
    }
}