using System.Collections.Generic;
using UnityEngine;

public class QuestManager
{
    private Dictionary<string, Quest> _questDictionary;

    private GameObject QuestRoot
    {
        get
        {
            GameObject root = GameObject.Find("@Managers");
            Transform[] objects = root.GetComponentsInChildren<Transform>();
            GameObject newGameObject = null;
            bool isExist = false;
            foreach (Transform go in objects)
            {
                isExist = go.name.Equals("QuestList");
                if (isExist)
                {
                    newGameObject = go.gameObject;
                    break;
                }
            }
            if (isExist == false)
            {
                newGameObject = Managers.Instance.CreateObject("QuestList", root.transform);
            }

            return newGameObject;
        }
    }

    [SerializeField]
    private int currentQuestLevel;

    public void Init()
    {
        _questDictionary = CreateQuestDictionary();

        foreach (Quest quest in _questDictionary.Values)
        {
            Managers.EVENT.questEvents.QuestStateChange(quest);
        }
    }

    public void Update()
    {
        foreach (Quest quest in _questDictionary.Values)
        {
            if (quest.state == QuestStates.REQUIREMENTS_NOT_MET && CheckRequirementsMet(quest))
            {
                ChangeQuestState(quest.info.id, QuestStates.CAN_START);
            }
        }
    }

    public void Enable()
    {
        Managers.EVENT.questEvents.onStartQuest += StartQuest;
        Managers.EVENT.questEvents.onAdvanceQuest += AdvanceQuest;
        Managers.EVENT.questEvents.onFinishQuest += FinishQuest;

        Managers.EVENT.questEvents.onQuestLevelChange += QuestLevelChange;
    }

    public void Disable()
    {
        Managers.EVENT.questEvents.onStartQuest -= StartQuest;
        Managers.EVENT.questEvents.onAdvanceQuest -= AdvanceQuest;
        Managers.EVENT.questEvents.onFinishQuest -= FinishQuest;

        Managers.EVENT.questEvents.onQuestLevelChange -= QuestLevelChange;
    }

    private void ChangeQuestState(string id, QuestStates state)
    {
        Quest quest = GetQuestById(id);
        quest.state = state;
        Managers.EVENT.questEvents.QuestStateChange(quest);
    }

    private void QuestLevelChange()
    {
        currentQuestLevel++;
    }

    private bool CheckRequirementsMet(Quest quest)
    {
        bool meetsRequirements = true;

        if (currentQuestLevel < quest.info.questLevelRequirement)
        {
            meetsRequirements = false;
        }

        foreach (QuestInfoSO prerequisiteQuestInfo in quest.info.questPrerequisites)
        {
            if (GetQuestById(prerequisiteQuestInfo.id).state != QuestStates.FINISHED)
            {
                meetsRequirements = false;
            }
        }

        return meetsRequirements;
    }


    private void StartQuest(string id)
    {
        Quest quest = GetQuestById(id);
        quest.InstantiateCurrentQuestStep(QuestRoot.transform);
        ChangeQuestState(quest.info.id, QuestStates.IN_PROGRESS);
        Debug.Log($"Quest Start : {quest.info.id}");
    }

    private void AdvanceQuest(string id)
    {
        Quest quest = GetQuestById(id);

        quest.MoveToNextStep();
        if (quest.CurrentStepExists())
        {
            quest.InstantiateCurrentQuestStep(QuestRoot.transform);
        }
        else
        {
            ChangeQuestState(quest.info.id, QuestStates.CAN_FINISH);
        }
    }

    private void FinishQuest(string id)
    {
        Quest quest = GetQuestById(id);
        ClaimRewards(quest);
        ChangeQuestState(quest.info.id, QuestStates.FINISHED);
        Managers.EVENT.questEvents.QuestLevelChange();
        Debug.Log($"Quest Finish : {quest.info.id}");
    }

    private void ClaimRewards(Quest quest)
    {
        // TODO - 보상 추가        
        Debug.Log($"골드보상    : {quest.info.goldReward}");
    }

    private Dictionary<string, Quest> CreateQuestDictionary()
    {
        QuestInfoSO[] allQuests = Resources.LoadAll<QuestInfoSO>("Quests");

        Dictionary<string, Quest> idToQuestDic = new Dictionary<string, Quest>();
        foreach (QuestInfoSO questInfo in allQuests)
        {
            if (idToQuestDic.ContainsKey(questInfo.id))
            {
                Debug.LogWarning($"Duplicate Quest ID : {questInfo.id}");
            }
            idToQuestDic.Add(questInfo.id, new Quest(questInfo));
        }

        return idToQuestDic;
    }

    private Quest GetQuestById(string id)
    {
        Quest quest = _questDictionary[id];
        if (quest == null)
        {
            Debug.LogError($"Not found Quest ID : {id}");
        }
        return quest;
    }

}
