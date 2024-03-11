
using System.Collections;
using UnityEngine;

public class EnemyController : Creature
{
    [SerializeField] protected EnemyInfoSO monsterInfo;       
    protected GameObject player;
    protected Transform playerTransform;

    public float attackDamage { get { return monsterInfo.attackDamage; } }
    protected float moveSpeed { get { return monsterInfo.moveSpeed; } }
    protected float attackRange { get { return monsterInfo.attackRange; } }

    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        player = GameObject.FindWithTag("Player");
        _health.SetHealth(monsterInfo.health);

        return true;
    }    

    protected override void OnUpdate()
    {
        
    }

    protected Vector3 TargetDir(Vector3 target) { return (target - transform.position).normalized; }

    protected bool AtTarget()
    {
        float distanceToTarget = Vector3.Distance(transform.position, playerTransform.position);
        Quaternion targetRotation = Quaternion.LookRotation(TargetDir(playerTransform.position));
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.fixedDeltaTime * 2f);
        return distanceToTarget < attackRange;
    }

    protected void TargetToMove(Vector3 target)
    {
        Quaternion targetRotation = Quaternion.LookRotation(TargetDir(target));
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.fixedDeltaTime * 2f);
        transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
    }

    
}
