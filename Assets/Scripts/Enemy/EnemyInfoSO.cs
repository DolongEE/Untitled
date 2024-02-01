using UnityEngine;

[CreateAssetMenu(fileName = "EnemyInfoSO", menuName = "ScriptableObjects/EnemyInfoSO", order = 2)]
public class EnemyInfoSO : ScriptableObject
{
    [Header("Monster Type")]
    public EnemyType type;

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
    public float alertDelay;

    public float missTargetTime;

    public float attackRange;

    private void OnValidate()
    {
#if UNITY_EDITOR
        UnityEditor.EditorUtility.SetDirty(this);
#endif
    }
}
