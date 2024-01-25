
using UnityEngine.InputSystem;

public class StartQuestStep : QuestStep
{
    public int pressed = 0;
    public int end = 5;

    private void OnEnable()
    {
        GameEventsManager.instance.inputEvents.onSubmitPressed += StartQuest;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.inputEvents.onSubmitPressed -= StartQuest;
    }

    void StartQuest()
    {
        if (pressed < end)
        {
            pressed++;
        }

        if (pressed >= end)
        {
            FinishedQuestStep();
        }
    }
}
