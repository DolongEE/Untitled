using UnityEngine;

[CreateAssetMenu(menuName = "Items/Item")]
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

    // 스탯 정보

#if UNITY_EDITOR
    private void OnValidate()
    {
        //정보 유지
        UnityEditor.EditorUtility.SetDirty(this);
    }
#endif
}