using UnityEngine;

[CreateAssetMenu(fileName = "NPCInfoSO", menuName = "ScriptableObjects/NPCInfoSO", order = 2)]
public class NPCInfoSO : ScriptableObject
{
    [field: SerializeField] public string id { get; private set; }

    [Header("Quest Init")]
    public string[] init;

    [Header("Quest Progress")]
    public string[] progress;

    [Header("Quest Rewards")]
    public string[] reward;

    [Header("Quest End")]
    public string[] end;

    private void OnValidate()
    {
#if UNITY_EDITOR
        id = this.name;
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }
}
