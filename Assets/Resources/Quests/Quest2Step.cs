using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quest2Step : QuestStep
{
    public int killMonsterCount = 0;
    public int endMonsterCount = 10;

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
        if (killMonsterCount < endMonsterCount)
        {
            
        }

        if (killMonsterCount == endMonsterCount)
        {
            FinishedQuestStep();
        }
    }
}
