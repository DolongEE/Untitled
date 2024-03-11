using UnityEngine;
using UnityEngine.UI;

public class DragSlot : MonoBehaviour
{
    public static DragSlot instance;
    public ItemSlot dragSlot;
    public Item draggedItem;
    public int draggedItemAmount;

    private Image itemImage;

    private void Start()
    {
        instance = this;
        itemImage = GetComponent<Image>();
    }
    public Item SetDraggedItem(ItemSlot sourceSlot)
    {
        if (sourceSlot.Item != null)
        {
            dragSlot = sourceSlot;
            draggedItem = sourceSlot.Item;
            draggedItemAmount = sourceSlot.Amount;

            if (draggedItem != null)
            {
                itemImage.sprite = draggedItem.itemImage;
                SetColor(1);
            }
        }
        return draggedItem;
    }
    public Item GetDraggedItem()
    {
        return draggedItem;
    }
    public void ResetDraggedSlot()
    {        
        dragSlot = null;
        draggedItem = null;
        itemImage.sprite = null;
        draggedItemAmount = 0;
        SetColor(0);
    }

    public void SetColor(float _alpha)
    {
        Color color = itemImage.color;
        color.a = _alpha;
        itemImage.color = color;
    }
}