using UnityEngine;
using UnityEngine.UI;

public class TutorialQuestStep_1 : QuestStep
{
    private bool isQuest;
    private bool isInven;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q) && isQuest == false)
        {
            isQuest = true;
            _questUI.questDescription.text = UpdateDescription();
        }
        else if (Input.GetKeyUp(KeyCode.I) && isInven == false)
        {
            isInven = true;
            _questUI.questDescription.text = UpdateDescription();
        }
        else if (isQuest && isInven)
        {            
            FinishedQuestStep();            
            
        }
    }

    public override string UpdateDescription()
    {
        string reuslt1 = isQuest ? "1/1" : "0/1";
        string reuslt2 = isInven ? "1/1" : "0/1";
        description = $"퀘스트 창 : Q - {reuslt1}\n인벤토리 창 : I - {reuslt2}";
        return description;
    }

    public override void OpenDoor()
    {
        Door = GameObject.Find("Door_1").GetComponent<Door>();
        Door.OpenDoor();
    }
}
