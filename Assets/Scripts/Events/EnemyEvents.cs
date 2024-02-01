using System;

public class EnemyEvents
{
    public event Action onEnemyIdle;
    public void EnemyIdle()
    {
        if (onEnemyIdle != null) 
        {
            onEnemyIdle();
        }
    }

    public event Action onEnemyPatrol;
    public void EnemyPatrol()
    {
        if (onEnemyPatrol != null) 
        {
            onEnemyPatrol();
        }
    }

    public event Action onEnemyAttack;
    public void EnemyAttack()
    {
        if (onEnemyAttack != null)
        {
            onEnemyAttack();
        }
    }

    public event Action onEnemyChase;
    public void EnemyChase()
    {
        if (onEnemyChase != null)
        {
            onEnemyChase();
        }
    }

    public event Action onEnemyAlert;
    public void EnemyAlert()
    {
        if (onEnemyAlert != null)
        {
            onEnemyAlert();
        }
    }
}
