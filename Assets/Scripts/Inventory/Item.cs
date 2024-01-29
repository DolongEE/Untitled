using System.ComponentModel;
using UnityEngine;

[CreateAssetMenu]
public class Item : ScriptableObject
{
    public enum ItemType
    {
        Equipment,
        Consumable,
        ETC
    }

    [Header("Item Name")]
    public string itemName;
    [Header("Item Image")]
    public Sprite itemImage;
    [Header("Item Descript")]
    public string itemDescription;
    [Header("Item Type")]
    public ItemType itemType;
    [Header("Item Prefab")]
    public GameObject itemPrefab;
    [Header("Item Equipped Status")]
    [SerializeField]
    private bool isEquipped = false;
    [Header("Item Acquired")]
    [SerializeField]
    private bool isAcquired = false;

    // 스탯 정보

    public bool IsEquip
    {
        get
        {
            return isEquipped;
        }
        set
        {
            isEquipped = value;
        }
    }

    public bool IsAcquire
    {
        get
        {
            return isAcquired;
        }
        set
        {
            isAcquired = value;
        }
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        //정보 유지
        UnityEditor.EditorUtility.SetDirty(this);
    }
#endif
}