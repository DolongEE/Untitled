using UnityEngine;

public class ItemInfo : MonoBehaviour
{
    public Item item;
    public Sprite itemImage;
    public string itemName;
    public string itemTooltip;

    [SerializeField] private bool isPlayerNear = false;
    [SerializeField] private BoxCollider box;

    private void OnValidate()
    {
        box = GetComponent<BoxCollider>();
        box.enabled = false;
    }
    private void Start()
    {
        itemName = item.itemName;
        itemTooltip = item.itemDescription;
        itemImage = item.itemImage;
        box.enabled = true;
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

        if (Managers.INVENTORY.inventory.AcquireItem(item))
        {
            Managers.INVENTORY.toolTip.tooltip.SetActive(false);
            Destroy(this.gameObject, 0.25f);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = true;
            other.GetComponentInChildren<PlayerAnimation>().isItemNear = true;
            Managers.INVENTORY.toolTip.tooltip.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            other.GetComponentInChildren<PlayerAnimation>().isItemNear = false;
            Managers.INVENTORY.toolTip.tooltip.SetActive(false);
        }
    }
}