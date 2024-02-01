
public class GameEventsManager
{
    public InputEvents inputEvents;
    public EnemyEvents EnemyEvents;
    public QuestEvents questEvents;

    public void Init()
    {
        inputEvents = new InputEvents();
        EnemyEvents = new EnemyEvents();
        questEvents = new QuestEvents();
    }
}
