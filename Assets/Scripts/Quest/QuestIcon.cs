using UnityEngine;

public class QuestIcon : MonoBehaviour
{
    [Header("Icons")]
    [SerializeField] private GameObject requirementsNotMetToStartIcon;
    [SerializeField] private GameObject canStartIcon;
    [SerializeField] private GameObject requirementsNotMetToFinishIcon;
    [SerializeField] private GameObject canFinishIcon;

    public void SetState(QuestStates newState, bool startPoint, bool finishPoint)
    {
        // set all to inactive
        requirementsNotMetToStartIcon.SetActive(false);
        canStartIcon.SetActive(false);
        requirementsNotMetToFinishIcon.SetActive(false);
        canFinishIcon.SetActive(false);

        // set the appropriate one to active based on the new state
        switch (newState)
        {
            case QuestStates.REQUIREMENTS_NOT_MET:
                if (startPoint) { requirementsNotMetToStartIcon.SetActive(true); }
                break;
            case QuestStates.CAN_START:
                if (startPoint) { canStartIcon.SetActive(true); }
                break;
            case QuestStates.IN_PROGRESS:
                if (finishPoint) { requirementsNotMetToFinishIcon.SetActive(true); }
                break;
            case QuestStates.CAN_FINISH:
                if (finishPoint) { canFinishIcon.SetActive(true); }
                break;
            case QuestStates.FINISHED:
                break;
            default:
                Debug.LogWarning("Quest State not recognized by switch statement for quest icon: " + newState);
                break;
        }
    }
}
