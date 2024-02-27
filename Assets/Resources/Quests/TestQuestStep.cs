
using UnityEngine.InputSystem;

public class TestQuestStep : QuestStep
{
    public int pressed = 0;
    public int end = 5;

    private void OnEnable()
    {
        Managers.EVENT.inputEvents.onSubmitPressed += StartQuest;
    }

    private void OnDisable()
    {
        Managers.EVENT.inputEvents.onSubmitPressed -= StartQuest;
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
