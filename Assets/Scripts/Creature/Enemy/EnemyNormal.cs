using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyNormal : EnemyController
{
    [SerializeField] PatrolPath patrolPath;
    [SerializeField] private bool DebugMode = false;

    [SerializeField] private EnemyStates lastEnemyState;
    private EnemyStates currentEnemyState
    {
        set
        {
            if (lastEnemyState == value)
                return;

            lastEnemyState = value;
            Debug.Log(lastEnemyState);
            switch (value)
            {
                case EnemyStates.IDLE:
                    IdleAnimation();
                    IdleBehaviour();
                    break;
                case EnemyStates.PATROL:
                    PatrolAnimation();
                    PatrolBehaviour();
                    break;
                case EnemyStates.ATTACK:
                    AttackAnimation();
                    AttackBehaviour();
                    break;
                case EnemyStates.HIT:
                    HitAnimation();
                    break;
                case EnemyStates.CHASE:
                    ChaseAnimation();
                    ChaseBehaviour();
                    break;
                case EnemyStates.MISSTARGET:
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
    private float attackRange { get { return monsterInfo.attackRange; } }
    
    private float timeSinceArrivedPath = Mathf.Infinity;    
    private float timeSinceMissTarget = Mathf.Infinity;    

    private int currentWaypointIndex = 0;
    private bool isDetect;
    private Vector3 playerPos;

    protected override bool Init()
    {
        if (base.Init() == false)
            return false;
        
        StartCoroutine(EnemyActState());

        return true;
    }

    protected override void OnFixedUpdate()
    {
        if (lastEnemyState != EnemyStates.MISSTARGET) timeSinceMissTarget = 0;
        PatrolDetect();

        switch (lastEnemyState)
        {
            case EnemyStates.IDLE:
                IdleUpdateBehaviour();
                break;
            case EnemyStates.PATROL:
                PatrolUpdateBehaviour();
                break;
            case EnemyStates.ATTACK:
                AttackUpdateBehaviour();
                break;
            case EnemyStates.HIT:
                HitUpdateBehaviour();
                break;
            case EnemyStates.CHASE:
                ChaseUpdateBehaviour();
                break;
            case EnemyStates.MISSTARGET:
                MissTargetUpdateBehaviour();
                break;
        }

        UpdateTimers();
    }

    protected IEnumerator EnemyActState()
    {
        while (true)
        {
            if (isDetect && AtTarget())
            {
                currentEnemyState = EnemyStates.ATTACK;
                yield return Managers.COROUTINE.WaitForSeconds(0.1f);
                yield return Managers.COROUTINE.WaitForSeconds(_animator);                
            }
            else if (isDetect && AtTarget() == false)
            {
                currentEnemyState = EnemyStates.CHASE;
            }
            else if (timeSinceMissTarget < missTargetTime
                && lastEnemyState != EnemyStates.IDLE
                && lastEnemyState != EnemyStates.PATROL)
            {
                currentEnemyState = EnemyStates.MISSTARGET;
            }
            else
            {
                PatrolUpdateBehaviour();
            }

            yield return null;
        }
    }

    protected void PatrolDetect()
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
            if (targetAngle <= viewAngle * 0.5f && !Physics.Raycast(myPos, targetDir, viewRadius, obstacleMask))
            {
                isDetect = true;
                playerPos = targetPos;
            }
            else
            {
                isDetect = false;
                playerPos = Vector3.zero;                
            }
        }
    }

    #region Behaviour Group
    protected void IdleBehaviour() { }
    protected void PatrolBehaviour() { }
    protected void AttackBehaviour() { }
    protected void HitBehaviour() 
    {
        
    }
    protected void ChaseBehaviour() { }
    protected void MissTargetBehaviour() { }

    protected void IdleUpdateBehaviour() { }
    protected void PatrolUpdateBehaviour()
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
            currentEnemyState = EnemyStates.PATROL;
        }
        else
        {
            currentEnemyState = EnemyStates.IDLE;
        }
    }
    protected void AttackUpdateBehaviour()
    {

    }
    protected void HitUpdateBehaviour()
    {

    }
    protected void ChaseUpdateBehaviour()
    {
        TargetToMove(playerPos);
    }
    protected void MissTargetUpdateBehaviour() { }

    #endregion

    #region Animations
    private void IdleAnimation() { _animator.CrossFade("Idle", 0.1f); }
    private void PatrolAnimation() { _animator.CrossFade("RunForward", 0.1f); }
    private void AttackAnimation() { _animator.CrossFade("Sword", 0.1f); }
    private void HitAnimation() { _animator.CrossFade("HitBack", 0.1f); }
    private void ChaseAnimation() { _animator.CrossFade("RunForward", 0.1f); }
    private void MissTargetAnimation() { _animator.CrossFade("Idle", 0.1f); }
    #endregion

    protected Vector3 GetCurrentWaypoint() { return patrolPath.GetWaypoint(currentWaypointIndex); }
    protected void CycleWaypoint() { currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex); }
    protected bool AtWaypoint()
    {
        float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
        return distanceToWaypoint < waypointRange;
    }
    protected bool AtTarget()
    {
        float distanceToTarget = Vector3.Distance(transform.position, playerPos);
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(TargetDir(playerPos)), Time.fixedDeltaTime * 2f);
        return distanceToTarget < attackRange;
    }

    protected void TargetToMove(Vector3 target)
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(TargetDir(target)), Time.fixedDeltaTime * 2f);
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }

    private Vector3 TargetDir(Vector3 target) { return (target - transform.position).normalized; }

    protected void UpdateTimers()
    {
        timeSinceArrivedPath += Time.fixedDeltaTime;        
        timeSinceMissTarget += Time.fixedDeltaTime;
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
            if (targetAngle <= viewAngle * 0.5f && !Physics.Raycast(myPos, targetDir, viewRadius, obstacleMask))
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
