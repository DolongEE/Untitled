
using TMPro;
using UnityEngine;

public class QuestPoint : NPC
{
    [Header("Quest")]
    [SerializeField] private QuestInfoSO questInfoQuest;
    [SerializeField] private NPCInfoDialogSO npcInfoDialog;

    [Header("Config")]
    [SerializeField] private bool startPoint = true;
    [SerializeField] private bool finishPoint = true;

    private string questId;
    private QuestStates currentQuestState;

    private QuestIcon questIcon;
    private int logCount;

    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        questId = questInfoQuest.id;
        questIcon = GetComponentInChildren<QuestIcon>();

        return true;
    }

    private void OnEnable()
    {
        Managers.EVENT.questEvents.onQuestStateChange += QuestStateChange;
    }

    private void OnDisable()
    {
        Managers.EVENT.questEvents.onQuestStateChange -= QuestStateChange;
    }

    private void QuestStateChange(Quest quest)
    {
        if (quest.info.id.Equals(questId))
        {
            currentQuestState = quest.state;
            questIcon.SetState(currentQuestState, startPoint, finishPoint);
        }
    }

    protected override void TogglePressed()
    {
        if (playerIsNear == false)
            return;

        if (npcInfoDialog != null)
        {
            switch (currentQuestState)
            {
                case QuestStates.CAN_START:
                    Talking(npcInfoDialog.init);
                    break;
                case QuestStates.IN_PROGRESS:
                    Talking(npcInfoDialog.progress);
                    break;
                case QuestStates.CAN_FINISH:
                    Talking(npcInfoDialog.reward);
                    break;
                case QuestStates.FINISHED:
                    Talking(npcInfoDialog.end);
                    break;
            }
        }
    }

    protected override void Talking(string[] logs)
    {
        if (logCount < logs.Length)
        {
            panelLogBox.SetActive(true);
            txtLogBox.text = logs[logCount++];
        }
        else
        {
            if (currentQuestState.Equals(QuestStates.CAN_START) && startPoint)
            {
                Managers.EVENT.questEvents.StartQuest(questId);
            }
            else if (currentQuestState.Equals(QuestStates.CAN_FINISH) && finishPoint)
            {
                Managers.EVENT.questEvents.FinishQuest(questId);
            }
            logCount = 0;
            panelLogBox.SetActive(false);
        }        
    }
    protected override void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNear = false;
        }
    }
}
