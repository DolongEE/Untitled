using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DragSlot : MonoBehaviour
{
    public static DragSlot instance;
    public ItemSlot dragSlot;
    public Item draggedItem;

    [SerializeField] private Image itemImage;

    private void Start()
    {
        instance = this;
    }
    public Item SetDraggedItem(ItemSlot sourceSlot)
    {
        if (sourceSlot.Item != null)
        {
            dragSlot = sourceSlot;
            draggedItem = sourceSlot.Item;

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
        SetColor(0);
    }

    public void SetColor(float _alpha)
    {
        Color color = itemImage.color;
        color.a = _alpha;
        itemImage.color = color;
    }
}