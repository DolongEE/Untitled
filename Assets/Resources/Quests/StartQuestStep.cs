
using UnityEngine.InputSystem;

public class StartQuestStep : QuestStep
{
    private int pressed = 0;
    private int end = 5;

    private void OnEnable()
    {
        GameEventsManager.instance.inputEvents.onSubmitPressed += Submit;
    }

    private void OnDisable()
    {
        GameEventsManager.instance.inputEvents.onSubmitPressed -= Submit;
    }

    void Submit()
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

    void Press()
    {
        if(pressed < end)
        {
            pressed++;
        }

        if (pressed >= end) 
        {
            FinishedQuestStep();
        }
    }
}
