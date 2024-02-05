using UnityEngine;

[CreateAssetMenu(fileName = "EnemyInfoSO", menuName = "ScriptableObjects/EnemyInfoSO", order = 2)]
public class EnemyInfoSO : ScriptableObject
{
    [Header("Stat")]
    public float health;
    public float attackDamage;
    public float moveSpeed;

    [Header("Detect Value")]
    [Range(0, 360)] public float viewAngle;
    public float viewRadius;
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
