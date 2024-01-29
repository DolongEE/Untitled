using UnityEngine;

public class MonsterController : MonoBehaviour
{
    [SerializeField] MonsterInfoSO monsterInfo;
    // 몬스터 정찰
    [SerializeField] bool DebugMode = false;
    [SerializeField] float ViewAngle { get { return monsterInfo.viewAngle; } }
    [SerializeField] float ViewRadius { get { return monsterInfo.viewRadious; } }
    [SerializeField] LayerMask TargetMask { get { return monsterInfo.targetMask; } }
    [SerializeField] LayerMask ObstacleMask { get { return monsterInfo.obstacleMask; } }
    
    [SerializeField] float waypointRange { get { return monsterInfo.waypointRange; } }
    [SerializeField] float waypointDelay { get { return monsterInfo.waypointDelay; } }
    private int currentWaypointIndex = 0;

    [SerializeField] PatrolPath patrolPath;

    // 경계
    [SerializeField] float alertTime { get { return monsterInfo.alertTime; } }
    private bool isDetect;

    private float timeSinceLastSawPlayer = Mathf.Infinity;
    private float timeSinceArrivedPath = Mathf.Infinity;

    // TODO - 스탯으로 통합
    private float attackDamage;
    private float speed;

    Health health;

    private void Init()
    {
        health = GetComponent<Health>();
        health.SetHealth(monsterInfo.health);

        attackDamage = monsterInfo.attackDamage;
        speed = monsterInfo.speed;
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
        Detecting();

        // 공격
        if (isDetect)
        {
            // TODO - Attack
            AttackBehaviour();
        }
        // 경계
        else if (timeSinceLastSawPlayer < alertTime)
        {
            isDetect = false;
        }
        // 순찰
        else
        {
            PatrolBehaviour();
        }

        UpdateTimers();
    }

    private void AttackBehaviour()
    {


    }

    private void Detecting()
    {
        Vector3 myPos = transform.position + Vector3.up * 0.5f;
        float lookingAngle = transform.eulerAngles.y;
        Vector3 lookDir = AngleToDir(lookingAngle);
        
        Collider[] Targets = Physics.OverlapSphere(myPos, ViewRadius, TargetMask);

        if (Targets.Length == 0) return;        
        foreach (Collider EnemyColli in Targets)
        {
            Vector3 targetPos = EnemyColli.transform.position;
            Vector3 targetDir = (targetPos - myPos).normalized;
            float targetAngle = Mathf.Acos(Vector3.Dot(lookDir, targetDir)) * Mathf.Rad2Deg;
            if (targetAngle <= ViewAngle * 0.5f && !Physics.Raycast(myPos, targetDir, ViewRadius, ObstacleMask))
            {
                isDetect = true;
                timeSinceLastSawPlayer = 0;
                TargetToMove(targetPos, targetDir);
            }
        }
    }

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
        }
    }

    private void TargetToMove(Vector3 target, Vector3 dir)
    {
        float lookSpeed = 0;
        if (isDetect) lookSpeed = 1.2f;
        else          lookSpeed = 2f;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * lookSpeed);        
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }    

    //현재 몬스터가 정찰 경로를 가지고 있다면, 어느 포인트에 있는지 반환
    private Vector3 GetCurrentWaypoint()
    {
        return patrolPath.GetWaypoint(currentWaypointIndex);
    }

    //다음 웨이포인트로 경로 지정
    private void CycleWaypoint()
    {
        currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
    }

    //목적지 웨이포인트와 몬스터와의 거리 계산
    private bool AtWaypoint()
    {
        float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
        return distanceToWaypoint < waypointRange;
    }

    // 몬스터 가시 거리
    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        if (!DebugMode) return;
        Vector3 myPos = transform.position + Vector3.up * 0.5f;
        Gizmos.DrawWireSphere(myPos, ViewRadius);

        float lookingAngle = transform.eulerAngles.y;  //캐릭터가 바라보는 방향의 각도
        Vector3 rightDir = AngleToDir(transform.eulerAngles.y + ViewAngle * 0.5f);
        Vector3 leftDir = AngleToDir(transform.eulerAngles.y - ViewAngle * 0.5f);
        Vector3 lookDir = AngleToDir(lookingAngle);

        Debug.DrawRay(myPos, rightDir * ViewRadius, Color.blue);
        Debug.DrawRay(myPos, leftDir * ViewRadius, Color.blue);
        Debug.DrawRay(myPos, lookDir * ViewRadius, Color.cyan);

        Collider[] Targets = Physics.OverlapSphere(myPos, ViewRadius, TargetMask);

        if (Targets.Length == 0) return;
        foreach (Collider EnemyColli in Targets)
        {
            Vector3 targetPos = EnemyColli.transform.position;
            Vector3 targetDir = (targetPos - myPos).normalized;
            float targetAngle = Mathf.Acos(Vector3.Dot(lookDir, targetDir)) * Mathf.Rad2Deg;
            if (targetAngle <= ViewAngle * 0.5f && !Physics.Raycast(myPos, targetDir, ViewRadius, ObstacleMask))
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
