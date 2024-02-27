using UnityEngine;

public class Quest
{
    public QuestInfoSO info;
    public QuestStates state;
    private int currentQuestStepIndex;

    public Quest(QuestInfoSO questInfo)
    {
        info = questInfo;
        state = QuestStates.REQUIREMENTS_NOT_MET;
        currentQuestStepIndex = 0;
    }

    public void MoveToNextStep()
    {
        currentQuestStepIndex++;
    }

    public bool CurrentStepExists()
    {
        return (currentQuestStepIndex < info.questStepPrefabs.Length);
    }

    public void InstantiateCurrentQuestStep(Transform parentTransform)
    {
        GameObject questStepPrefab = GetCurrentQuestStepPrefab();
        if (questStepPrefab != null)
        {
            QuestStep questStep = Object.Instantiate(questStepPrefab, parentTransform)
                .GetComponent<QuestStep>();
            questStep.InitializeQuestStep(info.id);
        }
    }

    private GameObject GetCurrentQuestStepPrefab()
    {
        GameObject questStepPrefab = null;
        if (CurrentStepExists())
        {
            questStepPrefab = info.questStepPrefabs[currentQuestStepIndex];
        }
        else
        {
            Debug.LogWarning($"Tried to get quest step prefab, but stepIndex was out of range indicating that there's no current step : QuestId= {info.id}, stepIndex= {currentQuestStepIndex}");
        }
        return questStepPrefab;
    }
}
