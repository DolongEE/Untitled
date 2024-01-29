using UnityEngine;

[CreateAssetMenu(fileName = "NPCInfoDiologSO", menuName = "ScriptableObjects/NPCInfoDiologSO", order = 1)]
public class NPCInfoDiologSO : ScriptableObject
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
