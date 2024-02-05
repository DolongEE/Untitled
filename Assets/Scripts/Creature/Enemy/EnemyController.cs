
using System.Collections;
using UnityEngine;
/*
-일반
정찰 공격 
-보스
공격1, 공격2, ....
 */

public class EnemyController : Creature
{
    [SerializeField] protected EnemyInfoSO monsterInfo;       
    [SerializeField] protected GameObject player;

    protected float attackDamage { get { return monsterInfo.attackDamage; } }
    protected float moveSpeed { get { return monsterInfo.moveSpeed; } }

    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        CreatureType = CreatureType.Enemy;
        Managers.EVENT.creatureEvents.CreatureCreate(this);

        player = GameObject.FindWithTag("Player");

        _health.SetHealth(monsterInfo.health);

        return true;
    }    

    protected override void OnFixedUpdate()
    {
        
    }
}
