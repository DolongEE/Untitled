using Unity.VisualScripting;
using UnityEngine;

public abstract class QuestStep : MonoBehaviour
{
    public QuestUI _questUI;
    [SerializeField] protected Door Door;
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
            transform.SetParent(Managers.QUEST.QuestEndRoot.transform);
        }
    }  

    public abstract string UpdateDescription();
    public abstract void OpenDoor();
}
