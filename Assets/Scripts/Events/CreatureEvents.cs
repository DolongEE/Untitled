using System;

public class CreatureEvents
{
    public event Action<Creature> onCreatureCreate;
    public void CreatureCreate(Creature creature)
    {
        if (onCreatureCreate != null)
        {
            onCreatureCreate(creature);
        }
    }
}
