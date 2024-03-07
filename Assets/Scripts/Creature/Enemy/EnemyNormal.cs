using System.Collections;
using UnityEngine;

public class EnemyNormal : EnemyController
{
    [SerializeField] PatrolPath patrolPath;
    [SerializeField] private bool DebugMode = false;

    [SerializeField] private EnemyNormalState lastEnemyState;
    private EnemyNormalState currentEnemyState
    {
        set
        {
            if (lastEnemyState == value)
                return;

            lastEnemyState = value;
            switch (value)
            {
                case EnemyNormalState.NONE:
                    NullAnimation();
                    break;
                case EnemyNormalState.IDLE:
                    IdleAnimation();
                    IdleBehaviour();
                    break;
                case EnemyNormalState.PATROL:
                    PatrolAnimation();
                    PatrolBehaviour();
                    break;
                case EnemyNormalState.ATTACK:
                    AttackAnimation();
                    AttackBehaviour();
                    break;
                case EnemyNormalState.HIT:
                    HitAnimation();
                    break;
                case EnemyNormalState.CHASE:
                    ChaseAnimation();
                    ChaseBehaviour();
                    break;
                case EnemyNormalState.MISSTARGET:
                    MissTargetAnimation();
                    MissTargetBehaviour();
                    break;
            }
        }
    }

    private float viewAngle { get { return monsterInfo.viewAngle; } }
    private float viewRadius { get { return monsterInfo.viewRadius; } }
    private LayerMask targetMask { get { return monsterInfo.targetMask; } }
    private LayerMask obstacleMask { get { return monsterInfo.obstacleMask; } }

    private float waypointRange { get { return monsterInfo.waypointRange; } }
    private float waypointDelay { get { return monsterInfo.waypointDelay; } }    
    private float missTargetTime { get {  return monsterInfo.missTargetTime; } }
    private float attackDelay { get { return monsterInfo.attackDelay; } }
        
    private float timeSinceArrivedPath = Mathf.Infinity;    
    private float timeSinceMissTarget = Mathf.Infinity;    
    private float timeSinceAttack = Mathf.Infinity;

    private int currentWaypointIndex = 0;
    private bool isDetect;

    protected override bool Init()
    {
        if (base.Init() == false)
            return false;
        
        StartCoroutine(EnemyActState());

        return true;
    }

    protected override void OnUpdate()
    {
        if (lastEnemyState != EnemyNormalState.MISSTARGET) timeSinceMissTarget = 0;
        PatrolDetect();

        switch (lastEnemyState)
        {
            case EnemyNormalState.IDLE:
                IdleUpdateBehaviour();
                break;
            case EnemyNormalState.PATROL:
                PatrolUpdateBehaviour();
                break;
            case EnemyNormalState.ATTACK:
                AttackUpdateBehaviour();
                break;
            case EnemyNormalState.HIT:
                HitUpdateBehaviour();
                break;
            case EnemyNormalState.CHASE:
                ChaseUpdateBehaviour();
                break;
            case EnemyNormalState.MISSTARGET:
                MissTargetUpdateBehaviour();
                break;
        }

        UpdateTimers();
    }

    private IEnumerator EnemyActState()
    {
        while (true)
        {

            if (isDetect && AtTarget()) 
            {
                if (timeSinceAttack > attackDelay)
                {
                    timeSinceAttack = 0;
                    currentEnemyState = EnemyNormalState.ATTACK;
                    yield return Managers.COROUTINE.WaitForSeconds(0.1f);
                    yield return Managers.COROUTINE.WaitForSeconds(_animator);
                }
                currentEnemyState = EnemyNormalState.NONE;
            }
            else if (isDetect && AtTarget() == false)
            {
                currentEnemyState = EnemyNormalState.CHASE;
            }
            else if (timeSinceMissTarget < missTargetTime
             && lastEnemyState != EnemyNormalState.IDLE
             && lastEnemyState != EnemyNormalState.PATROL)
            {                
                currentEnemyState = EnemyNormalState.MISSTARGET;
            }
            else
            {
                PatrolUpdateBehaviour();
            }

            yield return null;
        }
    }

    private void PatrolDetect()
    {
        Vector3 myPos = transform.position;
        float lookingAngle = transform.eulerAngles.y;
        Vector3 lookDir = AngleToDir(lookingAngle);

        Collider[] Targets = Physics.OverlapSphere(myPos, viewRadius, targetMask);

        if (Targets.Length == 0) return;
        foreach (Collider EnemyColli in Targets)
        {
            Vector3 targetPos = EnemyColli.transform.position;
            Vector3 targetDir = (targetPos - myPos).normalized;
            float targetAngle = Mathf.Acos(Vector3.Dot(lookDir, targetDir)) * Mathf.Rad2Deg;
            if (targetAngle <= viewAngle * 0.5f && Physics.Raycast(myPos, targetDir, viewRadius, obstacleMask) == false)
            {
                isDetect = true;
                playerTransform = EnemyColli.transform;                
            }
            else
            {
                isDetect = false;
                playerTransform = null;
            }
        }
    }

    #region Behaviour Group
    private void IdleBehaviour() { }
    private void PatrolBehaviour() { }
    private void AttackBehaviour() { }
    private void HitBehaviour() 
    {
        
    }
    private void ChaseBehaviour() { }
    private void MissTargetBehaviour() { }
    
    private void IdleUpdateBehaviour() { }
    private void PatrolUpdateBehaviour()
    {
        Vector3 nextPosition = transform.position;
        if (patrolPath != null)
        {
            if (AtWaypoint())
            {
                timeSinceArrivedPath = 0;
                CycleWaypoint();
            }
            nextPosition = GetCurrentWaypoint();
        }
        if (timeSinceArrivedPath > waypointDelay)
        {
            TargetToMove(nextPosition);
            currentEnemyState = EnemyNormalState.PATROL;
        }
        else
        {
            currentEnemyState = EnemyNormalState.IDLE;
        }
    }
    private void AttackUpdateBehaviour()
    {

    }
    private void HitUpdateBehaviour()
    {

    }
    private void ChaseUpdateBehaviour()
    {
        if (playerTransform == null)
            return;

        TargetToMove(playerTransform.position);
    }
    private void MissTargetUpdateBehaviour() { }

    #endregion

    #region Animations
    private void NullAnimation() { _animator.CrossFade("Idle", 0.1f); }
    private void IdleAnimation() { _animator.CrossFade("Idle", 0.1f); }
    private void PatrolAnimation() { _animator.CrossFade("RunForward", 0.1f); }
    private void AttackAnimation() { _animator.CrossFade("PunchLeft", 0.1f); }
    private void HitAnimation() { _animator.CrossFade("HitBack", 0.1f); }
    private void ChaseAnimation() { _animator.CrossFade("RunForward", 0.1f); }
    private void MissTargetAnimation() { _animator.CrossFade("Idle", 0.1f); }
    #endregion

    private Vector3 GetCurrentWaypoint() { return patrolPath.GetWaypoint(currentWaypointIndex); }
    private void CycleWaypoint() { currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex); }
    private bool AtWaypoint()
    {
        float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
        return distanceToWaypoint < waypointRange;
    }

    private void UpdateTimers()
    {
        timeSinceArrivedPath += Time.fixedDeltaTime;        
        timeSinceMissTarget += Time.fixedDeltaTime;
        timeSinceAttack += Time.fixedDeltaTime;
    }

    // 몬스터 가시 거리
    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        if (!DebugMode) return;
        Vector3 myPos = transform.position;
        Gizmos.DrawWireSphere(myPos, viewRadius);
        Gizmos.DrawWireSphere(myPos, attackRange);

        float lookingAngle = transform.eulerAngles.y;  //캐릭터가 바라보는 방향의 각도
        Vector3 rightDir = AngleToDir(transform.eulerAngles.y + viewAngle * 0.5f);
        Vector3 leftDir = AngleToDir(transform.eulerAngles.y - viewAngle * 0.5f);
        Vector3 lookDir = AngleToDir(lookingAngle);

        Debug.DrawRay(myPos, rightDir * viewRadius, Color.blue);
        Debug.DrawRay(myPos, leftDir * viewRadius, Color.blue);
        Debug.DrawRay(myPos, lookDir * viewRadius, Color.cyan);

        Collider[] Targets = Physics.OverlapSphere(myPos, viewRadius, targetMask);

        if (Targets.Length == 0) return;
        foreach (Collider EnemyColli in Targets)
        {
            Vector3 targetPos = EnemyColli.transform.position;
            Vector3 targetDir = (targetPos - myPos).normalized;
            float targetAngle = Mathf.Acos(Vector3.Dot(lookDir, targetDir)) * Mathf.Rad2Deg;
            if (targetAngle <= viewAngle * 0.5f && Physics.Raycast(myPos, targetDir, viewRadius, obstacleMask) == false)
            {
                if (DebugMode) Debug.DrawLine(myPos, targetPos, Color.red);
            }
        }
#endif
    }

    Vector3 AngleToDir(float angle)
    {
        float radian = angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(radian), 0f, Mathf.Cos(radian));
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
