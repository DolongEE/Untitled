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

    [Header("아이템 이름")]
    public string itemName;
    [Header("아이템 이미지")]
    public Sprite itemImage;
    [Header("아이템 설명")]
    public string itemDescription;
    [Header("아이템 타입")]
    public ItemType itemType;
    [Header("아이템 프리팹")]
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