using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class MonsterController : MonoBehaviour
{
    // ���� ����
    [SerializeField] bool DebugMode = false;
    [Range(0f, 360f)][SerializeField] float ViewAngle = 0f;
    [SerializeField] float ViewRadius = 6f;
    [SerializeField] LayerMask TargetMask;
    [SerializeField] LayerMask ObstacleMask;

    [SerializeField] PatrolPath patrolPath;
    [SerializeField] float waypointRange = 1f;
    [SerializeField] float waypointDelay = 3f;
    private int currentWaypointIndex = 0;

    // ���
    [SerializeField] float alertTime = 3f;
    private bool isDetect;
    private float targetDistance;

    private float timeSinceLastSawPlayer = Mathf.Infinity;
    private float timeSinceArrivedPath = Mathf.Infinity;

    [SerializeField] private float speed = 0.5f;
    
    // TODO - �������� ����
    private float hp = 100;

    private void Awake()
    {

    }

    private void Start()
    {

    }

    private void Update()
    {
        if (Die())
            return;



    }

    private void UpdateTimers()
    {
        timeSinceLastSawPlayer += Time.deltaTime;
        timeSinceArrivedPath += Time.deltaTime;
    }

    private void FixedUpdate()
    {
        if (Die())
            return;

        Detecting();

        // ����
        if (isDetect && CanAttack())
        {
            Debug.Log("�÷��̾� ����");
            AttackBehaviour();
        }
        // ���
        else if (timeSinceLastSawPlayer < alertTime)
        {
            isDetect = false;
        }
        // ����
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

    private void Damaged()
    {

    }

    private bool Die()
    {
        if (hp <= 0)
            return true;

        return false;
    }

    private bool CanAttack()
    {
        return false;
    }

    private void TargetToMove(Vector3 target, Vector3 dir)
    {
        float lookSpeed = 0;
        if (isDetect) lookSpeed = 1.2f;
        else          lookSpeed = 2f;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(dir), Time.deltaTime * lookSpeed);        
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }    

    //���� ���Ͱ� ���� ��θ� ������ �ִٸ�, ��� ����Ʈ�� �ִ��� ��ȯ
    private Vector3 GetCurrentWaypoint()
    {
        return patrolPath.GetWaypoint(currentWaypointIndex);
    }

    //���� ��������Ʈ�� ��� ����
    private void CycleWaypoint()
    {
        currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
    }

    //������ ��������Ʈ�� ���Ϳ��� �Ÿ� ���
    private bool AtWaypoint()
    {
        float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
        return distanceToWaypoint < waypointRange;
    }

    // ���� ���� �Ÿ�
    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        if (!DebugMode) return;
        Vector3 myPos = transform.position + Vector3.up * 0.5f;
        Gizmos.DrawWireSphere(myPos, ViewRadius);

        float lookingAngle = transform.eulerAngles.y;  //ĳ���Ͱ� �ٶ󺸴� ������ ����
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
