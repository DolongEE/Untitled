using UnityEditor;
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

    [Header("Item ID")]
    [SerializeField] string id;
    public string ID { get { return id; } }
    [Header("Item Name")]       public string itemName;
    [Header("Item Image")]      public Sprite itemImage;
    [Header("Item Descript")]   public string itemDescription;
    [Header("Item Type")]       public ItemType itemType;
    [Header("Item Prefab")]     public GameObject itemPrefab;
    [Header("Item Stackble")]   public bool isItemStackable;

#if UNITY_EDITOR
    protected virtual void OnValidate()
    {
        // ������ ��θ� ������
        string path = AssetDatabase.GetAssetPath(this);
        // ������ ��θ� GUID�� ��ȯ��. (���� �ĺ��� ����)
        id = AssetDatabase.AssetPathToGUID(path);
        //���� ����
        EditorUtility.SetDirty(this);
    }
#endif
}