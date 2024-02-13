
public class GameEventsManager
{
    public CreatureEvents creatureEvents;
    public InputEvents inputEvents;
    public QuestEvents questEvents;    

    public void Init()
    {
        creatureEvents = new CreatureEvents();
        inputEvents = new InputEvents();
        questEvents = new QuestEvents();
    }
}
