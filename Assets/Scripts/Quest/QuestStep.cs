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
            Managers.EVENT.questEvents.AdvanceQuest(_questId);
            Destroy(gameObject);
        }
    }
}
