using System;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.PlayerSettings;
using static UnityEngine.GraphicsBuffer;

public class EnemyController : MonoBehaviour
{
    [SerializeField] EnemyInfoSO monsterInfo;
    [SerializeField] PatrolPath patrolPath;
    [SerializeField] private bool DebugMode = false;

    [SerializeField] private Animator animator;

    private float viewAngle { get { return monsterInfo.viewAngle; } }
    private float viewRadius { get { return monsterInfo.viewRadious; } }
    private LayerMask targetMask { get { return monsterInfo.targetMask; } }
    private LayerMask obstacleMask { get { return monsterInfo.obstacleMask; } }

    private float waypointRange { get { return monsterInfo.waypointRange; } }
    private float waypointDelay { get { return monsterInfo.waypointDelay; } }
    private float alertTime { get { return monsterInfo.alertTime; } }
    private float attackRange { get { return monsterInfo.attackRange; } }
    private float timeSinceLastSawPlayer = Mathf.Infinity;
    private float timeSinceArrivedPath = Mathf.Infinity;
    private int currentWaypointIndex = 0;     
    private bool isDetect;
    private Vector3 target;
    
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
            switch (value)
            {
                case EnemyStates.IDLE:
                    Managers.EVENT.EnemyEvents.EnemyIdle();
                    break;
                case EnemyStates.PATROL:
                    Managers.EVENT.EnemyEvents.EnemyPatrol();
                    break;
                case EnemyStates.ATTACK:
                    Managers.EVENT.EnemyEvents.EnemyAttack();
                    break;
                case EnemyStates.CHASE:
                    Managers.EVENT.EnemyEvents.EnemyChase();
                    break;
                case EnemyStates.ALERT:
                    Managers.EVENT.EnemyEvents.EnemyAlert();
                    break;
            }
        }
    }

    Health health;

    private void Init()
    {
        animator = GetComponentInChildren<Animator>();
        health = GetComponent<Health>();
        health.SetHealth(monsterInfo.health);

        attackDamage = monsterInfo.attackDamage;
        speed = monsterInfo.speed;

        Managers.EVENT.EnemyEvents.onEnemyIdle += IdleAnimation;
        Managers.EVENT.EnemyEvents.onEnemyPatrol += PatrolAnimation;
        Managers.EVENT.EnemyEvents.onEnemyAttack += AttackAnimation;
        Managers.EVENT.EnemyEvents.onEnemyChase += ChaseAnimation;
        Managers.EVENT.EnemyEvents.onEnemyAlert += AlertAnimation;
    }

    private void OnDisable()
    {
        Managers.EVENT.EnemyEvents.onEnemyIdle -= IdleAnimation;
        Managers.EVENT.EnemyEvents.onEnemyPatrol -= PatrolAnimation;
        Managers.EVENT.EnemyEvents.onEnemyAttack -= AttackAnimation;
        Managers.EVENT.EnemyEvents.onEnemyChase -= ChaseAnimation;
        Managers.EVENT.EnemyEvents.onEnemyAlert -= AlertAnimation;
    }

    private void Awake()
    {
        Init();
    }

    private void UpdateTimers()
    {
        timeSinceLastSawPlayer += Time.deltaTime;
        timeSinceArrivedPath += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (health.IsDead()) return;

        Detecting();

        if (isDetect && AtTarget())
        {
            currentEnemyState = EnemyStates.ATTACK;
            Debug.Log("aattakc");
            AttackBehaviour();
        }
        else if (isDetect && AtTarget() == false)
        {
            currentEnemyState = EnemyStates.CHASE;
            ChaseBehaviour();
        }        
        else if (timeSinceLastSawPlayer < alertTime)
        {
            //isDetect = false;
            currentEnemyState = EnemyStates.ALERT;
        }        
        else
        {            
            PatrolBehaviour();
        }
        UpdateTimers();
    }


    private void Detecting()
    {
        Vector3 myPos = transform.position;
        float lookingAngle = transform.eulerAngles.y;
        Vector3 lookDir = AngleToDir(lookingAngle);
        
        Collider[] Targets = Physics.OverlapSphere(myPos, viewRadius, targetMask);

        if (Targets.Length == 0)
        {
            isDetect = false;
            target = Vector3.zero;
            return;
        }
        foreach (Collider EnemyColli in Targets)
        {
            Vector3 targetPos = EnemyColli.transform.position;
            Vector3 targetDir = (targetPos - myPos).normalized;
            float targetAngle = Mathf.Acos(Vector3.Dot(lookDir, targetDir)) * Mathf.Rad2Deg;            
            if (targetAngle <= viewAngle * 0.5f && !Physics.Raycast(myPos, targetDir, viewRadius, obstacleMask))
            {
                isDetect = true;
                target = targetPos;
                timeSinceLastSawPlayer = 0;                
            }
            else
            {
                isDetect = false;
                target = Vector3.zero;
            }
        }
    }


    #region Behaviour Group
    private void PatrolBehaviour()
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

    protected virtual void AttackBehaviour()
    {

    }

    private void ChaseBehaviour()
    {
        Vector3 targetDir = (target - transform.position).normalized;
        TargetToMove(target, targetDir);
    }
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
        animator.CrossFade("Idle01", 0.1f);
    }
    #endregion

    #region Sub Function
    private void TargetToMove(Vector3 target, Vector3 dir)
    {
        float lookSpeed = 0;
        if (isDetect) lookSpeed = 1.2f;
        else lookSpeed = 2f;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * lookSpeed);
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
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * 2f);
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
