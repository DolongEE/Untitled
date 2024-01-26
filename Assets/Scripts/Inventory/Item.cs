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

    [Header("������ �̸�")]
    public string itemName;
    [Header("������ �̹���")]
    public Sprite itemImage;
    [Header("������ ����")]
    public string itemDescription;
    [Header("������ Ÿ��")]
    public ItemType itemType;
    [Header("������ ������")]
    public GameObject itemPrefab;

    // ���� ����

#if UNITY_EDITOR
    private void OnValidate()
    {
        //���� ����
        UnityEditor.EditorUtility.SetDirty(this);
    }
#endif
}