using UnityEngine;

public class TutorialQuestStep_4 : QuestStep
{
    private int killCount;
    private int oldKillCount;

    private const int MAX_KILL_COUNT = 3;

    private void Awake()
    {

    }

    void Update()
    {
        if (killCount.Equals(oldKillCount) == false)
        {
            oldKillCount = killCount;
            _questUI.questDescription.text = UpdateDescription();
        }

        if (killCount >= MAX_KILL_COUNT)
        {
            FinishedQuestStep();
        }
    }

    public void AddKillCount()
    {
        killCount++;
    }

    public override string UpdateDescription()
    {
        description = $"³ª¹µ°¡Áö : {killCount} / {MAX_KILL_COUNT}";
        return description;
    }

    public override void OpenDoor()
    {
        Door = GameObject.Find("Door_4").GetComponent<Door>();
        Door.OpenDoor();
    }
}
