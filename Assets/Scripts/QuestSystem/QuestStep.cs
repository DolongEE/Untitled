using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class QuestStep : MonoBehaviour
{
    private bool isFinished = false;

    private string _questId;

    public void InitializeQuestStep(string questId)
    {
        _questId = questId;
    }

    protected void FinishedQuestStep()
    {
        if (isFinished == false)
        {
            isFinished = true;
            GameEventsManager.instance.questEvents.AdvanceQuest(_questId);
            Destroy(gameObject);
        }
    }
}
