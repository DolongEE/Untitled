using UnityEngine;

public class QuestPoint : MonoBehaviour
{
    [Header("Quest")]
    [SerializeField] private QuestInfoSO questInfoQuest;
    [SerializeField] private NPCInfoDialogSO npcInfoDialog;

    [Header("Config")]
    [SerializeField] private bool startPoint = true;
    [SerializeField] private bool finishPoint = true;

    private NPC npc;
    private QuestIcon questIcon;
    private string questId;
    [HideInInspector] public QuestStates currentQuestState;
    [HideInInspector] public bool isQuestTalk;

    private int logCount;
    
    private void Awake()
    {
        npc = GetComponent<NPC>();
        questId = questInfoQuest.id;
        questIcon = npc.questIcon.GetComponent<QuestIcon>();
    }

    private void OnEnable()
    {
        Managers.EVENT.questEvents.onQuestStateChange += QuestStateChange;
        Managers.EVENT.inputEvents.onToggleGPressed += ToggleGPressed;
    }

    private void OnDisable()
    {
        Managers.EVENT.questEvents.onQuestStateChange -= QuestStateChange;
        Managers.EVENT.inputEvents.onToggleGPressed -= ToggleGPressed;
    }

    private void QuestStateChange(Quest quest)
    {
        if (quest.info.id.Equals(questId))
        {
            currentQuestState = quest.state;
            questIcon.SetState(currentQuestState, startPoint, finishPoint);
        }
    }

    private void ToggleGPressed()
    {
        if (npc.playerIsNear == false || isQuestTalk == false)
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

    public void Talking(string[] logs)
    {
        if (logCount < logs.Length)
        {
            npc.panelLogBox.SetActive(true);
            npc.txtLogBox.text = logs[logCount++];
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
            isQuestTalk = false;
            npc.PlayerOtherAction = false;
            npc.panelLogBoxButtons.SetActive(true);
            npc.panelLogBox.SetActive(false);
            npc.TalkToggleAdd();
        }
    }
}
