
public class GameEventsManager
{
    public CreatureEvents creatureEvents;
    public InputEvents inputEvents;
    public EnemyEvents enemyEvents;
    public QuestEvents questEvents;
    

    public void Init()
    {
        creatureEvents = new CreatureEvents();
        inputEvents = new InputEvents();
        enemyEvents = new EnemyEvents();
        questEvents = new QuestEvents();
    }
}
