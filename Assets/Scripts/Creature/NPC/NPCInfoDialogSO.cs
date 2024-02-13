using UnityEngine;

[CreateAssetMenu(fileName = "NPCInfoDiologSO", menuName = "ScriptableObjects/NPCInfoDiologSO", order = 1)]
public class NPCInfoDialogSO : ScriptableObject
{
    [field: SerializeField] public string id { get; private set; }

    [Header("Dialogue Init")]
    public string[] init;

    [Header("Dialogue Progress")]
    public string[] progress;

    [Header("Dialogue Rewards")]
    public string[] reward;

    [Header("Dialogue End")]
    public string[] end;

    private void OnValidate()
    {
#if UNITY_EDITOR
        id = this.name;
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }
}
