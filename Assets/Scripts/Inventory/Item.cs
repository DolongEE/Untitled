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
    [SerializeField]            private string id;
                                public string ID { get { return id; } }
    [Header("Item Name")]       public string itemName;
    [Header("Item Image")]      public Sprite itemImage;
    [Header("Item Descript")]   public string itemDescription;
    [Header("Item Type")]       public ItemType itemType;
    [Header("Item Prefab")]     public GameObject itemPrefab;
    [Header("Item Stackble")]   public bool isItemStackable;
    [SerializeField]            private bool isEquipped;
    public bool IsEquipped { get { return isEquipped; } set { isEquipped = value; } }

    public virtual Item GetCopy()
    {
        return this;
    }

#if UNITY_EDITOR
    protected virtual void OnValidate()
    {
        // 에셋의 경로를 가져옴
        string path = AssetDatabase.GetAssetPath(this);
        // 에셋의 경로를 GUID로 변환함. (고유 식별을 위함)
        id = AssetDatabase.AssetPathToGUID(path);
        //정보 유지
        EditorUtility.SetDirty(this);
    }

    private void OnEnable()
    {
        isEquipped = false;
    }
#endif
}