
using TMPro;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class QuestPoint : MonoBehaviour
{
    [Header("Quest")]
    [SerializeField] private QuestInfoSO questInfoForPoint;
    [SerializeField] private NPCInfoDiologSO npcInfoForPoint;

    [Header("Object")]
    [SerializeField] private GameObject pnlLogBox;
    [SerializeField] private TextMeshProUGUI txtLogBox;

    [Header("Config")]
    [SerializeField] private bool startPoint = true;
    [SerializeField] private bool finishPoint = true;

    private bool playerIsNear = false;
    private string questId;
    private QuestState currentQuestState;

    private QuestIcon questIcon;
    private int logCount;

    private void Awake()
    {
        questId = questInfoForPoint.id;
        questIcon = GetComponentInChildren<QuestIcon>();
        pnlLogBox.SetActive(false);
    }

    private void OnEnable()
    {
        Managers.EVENT.questEvents.onQuestStateChange += QuestStateChange;
        Managers.EVENT.inputEvents.onSubmitPressed += SubmitPressed;
        Managers.EVENT.inputEvents.onQuestLogTogglePressed += TogglePressed;
    }

    private void OnDisable()
    {
        Managers.EVENT.questEvents.onQuestStateChange -= QuestStateChange;
        Managers.EVENT.inputEvents.onSubmitPressed -= SubmitPressed;
        Managers.EVENT.inputEvents.onQuestLogTogglePressed -= TogglePressed;
    }

    private void QuestStateChange(Quest quest)
    {
        if (quest.info.id.Equals(questId))
        {
            currentQuestState = quest.state;
            questIcon.SetState(currentQuestState, startPoint, finishPoint);
        }
    }

    private void SubmitPressed()
    {

    }

    private void TogglePressed()
    {
        if (playerIsNear == false)
            return;

        if (npcInfoForPoint != null)
        {
            switch (currentQuestState)
            {
                case QuestState.CAN_START:
                    Talking(npcInfoForPoint.init);
                    break;
                case QuestState.IN_PROGRESS:
                    Talking(npcInfoForPoint.progress);
                    break;
                case QuestState.CAN_FINISH:
                    Talking(npcInfoForPoint.reward);
                    break;
                case QuestState.FINISHED:
                    Talking(npcInfoForPoint.end);
                    break;
            }
        }

    }

    private void Talking(string[] logs)
    {
        if (logCount < logs.Length)
        {
            pnlLogBox.SetActive(true);
            txtLogBox.text = logs[logCount++];
        }
        else
        {
            if (currentQuestState.Equals(QuestState.CAN_START) && startPoint)
            {
                Managers.EVENT.questEvents.StartQuest(questId);
            }
            else if (currentQuestState.Equals(QuestState.CAN_FINISH) && finishPoint)
            {
                Managers.EVENT.questEvents.FinishQuest(questId);
            }
            logCount = 0;
            pnlLogBox.SetActive(false);
        }        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNear = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNear = false;
        }
    }
}
