using System;

public class EnemyEvents
{
    public event Action onEnemyAttackRush;
    public void EnemyAttackRush()
    {
        if (onEnemyAttackRush != null) 
        {
            onEnemyAttackRush();
        }
    }

}
