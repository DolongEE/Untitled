
public class GameEventsManager
{
    public InputEvents inputEvents;
    public QuestEvents questEvents;    

    public void Init()
    {
        inputEvents = new InputEvents();
        questEvents = new QuestEvents();

    }
}
