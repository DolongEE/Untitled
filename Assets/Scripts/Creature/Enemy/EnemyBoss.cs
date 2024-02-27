using System.Collections;
using UnityEngine;

public class EnemyBoss : EnemyController
{
    private Vector3 startPos;
    private Vector3 targetPos;
    private float readyTime;

    private float attackDelay { get { return monsterInfo.attackDelay; } }
    private float timeSinceAttack = Mathf.Infinity;

    [SerializeField] private EnemyBossState lastEnemyState;
    private EnemyBossState currentEnemyState
    {
        set
        {
            if (lastEnemyState == value)
                return;

            BeforeAttack();
            lastEnemyState = value;
            Debug.Log(lastEnemyState);
            switch (value)
            {
                case EnemyBossState.NONE:
                    NullAnimation();
                    break;
                case EnemyBossState.IDLE:
                    IdleAnimation();
                    break;
                case EnemyBossState.ATTACKRUSH:
                    AttackRushAnimation();
                    break;
            }
        }
    }

    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        StartCoroutine(EnemyAttack());

        return true;
    }

    protected override void OnFixedUpdate()
    {
        switch (lastEnemyState)
        {
            case EnemyBossState.ATTACKRUSH:
                AttackUpdateRush();
                break;
        }

        timeSinceAttack += Time.fixedDeltaTime;
    }

    private IEnumerator EnemyAttack()
    {
        while (true)
        {
            if (timeSinceAttack > attackDelay)
            {
                timeSinceAttack = 0;
                currentEnemyState = EnemyBossState.ATTACKRUSH;
                yield return Managers.COROUTINE.WaitForSeconds(0.1f);
                yield return Managers.COROUTINE.WaitForSeconds(_animator);
            }
            else
            {
                currentEnemyState = EnemyBossState.NONE;
            }
            yield return null;
        }
    }

    private void BeforeAttack()
    {
        startPos = transform.position;
        targetPos = player.transform.position;
        transform.LookAt(targetPos);
    }

    private void AttackUpdateRush()
    {
        transform.position += (transform.forward * 10f * Time.fixedDeltaTime);
    }

    private void NullAnimation() { _animator.CrossFade("Idle", 0.1f); }
    private void IdleAnimation() { _animator.CrossFade("Idle", 0.1f); }
    private void AttackRushAnimation() { _animator.CrossFade("PunchLeft", 0.1f); }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
}
