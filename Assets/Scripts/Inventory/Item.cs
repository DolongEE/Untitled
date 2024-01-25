using UnityEngine;

[CreateAssetMenu]
public class Item : ScriptableObject
{
    [Header("아이템 정보")]
    public string itemName;
    public Sprite itemImage;
    public string itemDescription;

    // 스탯 정보

#if UNITY_EDITOR
    private void OnValidate()
    {
        //정보 유지
        UnityEditor.EditorUtility.SetDirty(this);
    }
#endif
}