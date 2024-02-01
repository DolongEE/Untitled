
using System.Collections;
using UnityEngine;

public abstract class EnemyController : MonoBehaviour
{
    [SerializeField] EnemyInfoSO monsterInfo;
    [SerializeField] PatrolPath patrolPath;
    [SerializeField] private bool DebugMode = false;

    protected Animator animator;
    protected Rigidbody rigid;

    private float viewAngle { get { return monsterInfo.viewAngle; } }
    private float viewRadius { get { return monsterInfo.viewRadious; } }
    private LayerMask targetMask { get { return monsterInfo.targetMask; } }
    private LayerMask obstacleMask { get { return monsterInfo.obstacleMask; } }

    private float waypointRange { get { return monsterInfo.waypointRange; } }
    private float waypointDelay { get { return monsterInfo.waypointDelay; } }
    private float alertDelay { get { return monsterInfo.alertDelay; } }
    private float missTargetTime { get { return monsterInfo.missTargetTime; } }

    private float attackRange { get { return monsterInfo.attackRange; } }
    private float timeSinceArrivedPath = Mathf.Infinity;
    private float timeSinceMissTarget = Mathf.Infinity;
    private int currentWaypointIndex = 0;
    private bool isDetect;
    protected Vector3 target;

    private float attackDamage;
    private float speed;

    [SerializeField] private EnemyStates lastEnemyState;
    private EnemyStates currentEnemyState
    {
        set
        {
            if (lastEnemyState == value)
                return;

            lastEnemyState = value;
            //Debug.Log(lastEnemyState);
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
                case EnemyStates.CHASE:
                    ChaseAnimation();
                    ChaseBehaviour();
                    break;
                case EnemyStates.ALERT:
                    AlertAnimation();
                    AlertBehaviour();
                    break;
                case EnemyStates.MISSTARGET:
                    MissTargetAnimation();
                    MissTargetBehaviour();
                    break;
            }
        }
    }

    Health health;

    private void Init()
    {
        animator = GetComponentInChildren<Animator>();
        rigid = GetComponentInChildren<Rigidbody>();
        health = GetComponent<Health>();
        health.SetHealth(monsterInfo.health);

        attackDamage = monsterInfo.attackDamage;
        speed = monsterInfo.speed;

        StartCoroutine(EnemyActState());
    }

    private void Awake()
    {
        Init();
    }
    private void OnDisable()
    {
        StopAllCoroutines();
    }

    IEnumerator EnemyActState()
    {
        while (true)
        {
            if (isDetect && AtTarget())
            {
                currentEnemyState = EnemyStates.ALERT;
                yield return Managers.COROUTINE.WaitForSeconds(0.1f);
                yield return Managers.COROUTINE.WaitForSeconds(alertDelay);

                currentEnemyState = EnemyStates.ATTACK;   
                yield return Managers.COROUTINE.WaitForSeconds(0.1f);
                yield return Managers.COROUTINE.WaitForSeconds(3f /*TODO - 공격 애니메이션 변경 */);                
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

    private void FixedUpdate()
    {
        if (health.IsDead()) return;

        PatrolDetect();

        switch(lastEnemyState)
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
            case EnemyStates.CHASE:
                ChaseUpdateBehaviour();
                break;
            case EnemyStates.ALERT:
                AlertUpdateBehaviour();
                break;
            case EnemyStates.MISSTARGET:
                MissTargetUpdateBehaviour();
                break;
        }

        UpdateTimers();
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
            if (targetAngle <= viewAngle * 0.5f && !Physics.Raycast(myPos, targetDir, viewRadius, obstacleMask))
            {
                isDetect = true;
                target = targetPos;
            }
            else
            {
                isDetect = false;
                target = Vector3.zero;
                timeSinceMissTarget = 0;
            }
        }
    }


    #region Behaviour Group
    private void IdleBehaviour() { }
    private void PatrolBehaviour() { }
    protected abstract void AttackBehaviour();
    private void ChaseBehaviour() { }
    private void AlertBehaviour() { }
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
        Vector3 targetDir = (nextPosition - transform.position).normalized;
        if (timeSinceArrivedPath > waypointDelay)
        {
            TargetToMove(nextPosition, targetDir);
            currentEnemyState = EnemyStates.PATROL;
        }
        else
        {
            currentEnemyState = EnemyStates.IDLE;
        }
    }
    protected abstract void AttackUpdateBehaviour();
    private void ChaseUpdateBehaviour()
    {
        Vector3 targetDir = (target - transform.position).normalized;
        TargetToMove(target, targetDir);
    }
    private void AlertUpdateBehaviour() 
    { 
        transform.RotateAround(target, Vector3.up, speed * Time.fixedDeltaTime); 
    }
    private void MissTargetUpdateBehaviour() { }

    #endregion

    #region Animations
    private void IdleAnimation()
    {
        animator.CrossFade("Idle01", 0.1f);
    }

    private void PatrolAnimation()
    {
        animator.CrossFade("Walk", 0.1f);
    }

    private void AttackAnimation()
    {
        animator.CrossFade("PunchLeft", 0.1f);
    }

    private void ChaseAnimation()
    {
        animator.CrossFade("Sprint", 0.1f);
    }

    private void AlertAnimation()
    {
        animator.CrossFade("Strafe01Left", 0.1f);
    }

    private void MissTargetAnimation()
    {
        animator.CrossFade("Idle01", 0.1f);
    }
    #endregion

    #region Sub Function
    private void UpdateTimers()
    {
        timeSinceArrivedPath += Time.fixedDeltaTime;
        timeSinceMissTarget += Time.fixedDeltaTime;
    }

    private void TargetToMove(Vector3 target, Vector3 dir)
    {
        float lookSpeed = 0;
        if (isDetect) lookSpeed = 1.2f;
        else lookSpeed = 2f;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.fixedDeltaTime * lookSpeed);
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }

    private Vector3 GetCurrentWaypoint()
    {
        return patrolPath.GetWaypoint(currentWaypointIndex);
    }

    private void CycleWaypoint()
    {
        currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
    }

    private bool AtWaypoint()
    {
        float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
        return distanceToWaypoint < waypointRange;
    }

    private bool AtTarget()
    {
        float distanceToTarget = Vector3.Distance(transform.position, target);
        Vector3 dir = (target - transform.position).normalized;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.fixedDeltaTime * 2f);
        return distanceToTarget < attackRange;
    }
    #endregion

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
}
