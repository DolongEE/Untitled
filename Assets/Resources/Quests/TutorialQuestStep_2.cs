using UnityEngine;

public class TutorialQuestStep_2 : QuestStep
{
    private bool isQuest;
    private bool isInven;

    private GameObject door_2;

    private void Awake()
    {
        door_2 = GameObject.Find("Door_2");
    }

    void Update()
    {

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
        door_2.SetActive(false);
    }
}
