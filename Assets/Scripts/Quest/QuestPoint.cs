
using TMPro;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class QuestPoint : MonoBehaviour
{
    [Header("Quest")]
    [SerializeField] private QuestInfoSO questInfoQuest;
    [SerializeField] private NPCInfoDiologSO npcInfoDiolog;

    [Header("Object")]
    [SerializeField] private GameObject canvasLogBox;
    [SerializeField] private TextMeshProUGUI txtLogBox;

    [Header("Config")]
    [SerializeField] private bool startPoint = true;
    [SerializeField] private bool finishPoint = true;

    private bool playerIsNear = false;
    private string questId;
    private QuestStates currentQuestState;

    private QuestIcon questIcon;
    private int logCount;

    private void Awake()
    {
        questId = questInfoQuest.id;
        questIcon = GetComponentInChildren<QuestIcon>();
        canvasLogBox.SetActive(false);
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

        if (npcInfoDiolog != null)
        {
            switch (currentQuestState)
            {
                case QuestStates.CAN_START:
                    Talking(npcInfoDiolog.init);
                    break;
                case QuestStates.IN_PROGRESS:
                    Talking(npcInfoDiolog.progress);
                    break;
                case QuestStates.CAN_FINISH:
                    Talking(npcInfoDiolog.reward);
                    break;
                case QuestStates.FINISHED:
                    Talking(npcInfoDiolog.end);
                    break;
            }
        }

    }

    private void Talking(string[] logs)
    {
        if (logCount < logs.Length)
        {
            canvasLogBox.SetActive(true);
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
            canvasLogBox.SetActive(false);
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
