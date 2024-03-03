using UnityEngine;

public class TutorialQuestStep_1 : QuestStep
{
    private bool isQuest;
    private bool isInven;

    private GameObject door_1;

    private void Awake()
    {
        door_1 = GameObject.Find("Door_1");
    }

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
        description = $"����Ʈ â : Q - {reuslt1}\n�κ��丮 â : I - {reuslt2}";
        return description;
    }

    private void OnDisable()
    {        
        door_1.SetActive(false);
    }
}
