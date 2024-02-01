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

}
