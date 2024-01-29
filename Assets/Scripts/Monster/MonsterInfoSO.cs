using UnityEngine;

[CreateAssetMenu(fileName = "MonsterInfoSO", menuName = "ScriptableObjects/MonsterInfoSO", order = 2)]
public class MonsterInfoSO : ScriptableObject
{
    [Header("Monster Type")]
    public MonsterType type;

    [Header("Stat")]
    public float health;
    public float attackDamage;
    public float speed;

    [Header("Detect Value")]    
    [Range(0, 360)] public float viewAngle;
    public float viewRadious;
    public LayerMask targetMask;
    public LayerMask obstacleMask;
    public float waypointRange;
    public float waypointDelay;
    public float alertTime;

    private void OnValidate()
    {
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }
}
