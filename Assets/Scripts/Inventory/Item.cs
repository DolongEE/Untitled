using UnityEngine;

[CreateAssetMenu]
public class Item : ScriptableObject
{
    [Header("������ ����")]
    public string itemName;
    public Sprite itemImage;
    public string itemDescription;

    // ���� ����

#if UNITY_EDITOR
    private void OnValidate()
    {
        //���� ����
        UnityEditor.EditorUtility.SetDirty(this);
    }
#endif
}