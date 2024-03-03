using Unity.VisualScripting;
using UnityEngine;

public abstract class QuestStep : MonoBehaviour
{
    public QuestUI _questUI;
    protected string description;

    private bool isFinished = false;
    private string _questId;   

    public void InitializeQuestStep(string questId, QuestUI questUI)
    {
        _questId = questId;
        _questUI = questUI;
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

    public abstract string UpdateDescription();    
}
