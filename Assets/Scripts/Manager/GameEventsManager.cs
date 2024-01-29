
public class GameEventsManager
{
    public InputEvents inputEvents;
    public PlayerEvents playerEvents;
    public QuestEvents questEvents;

    public void Init()
    {
        inputEvents = new InputEvents();
        playerEvents = new PlayerEvents();
        questEvents = new QuestEvents();
    }
}
