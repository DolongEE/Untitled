using UnityEngine;

public class TutorialQuestStep_4 : QuestStep
{
    private GameObject player;

    private int killCount;
    private int oldKillCount;

    private const int MAX_KILL_COUNT = 2;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    void Update()
    {
        GetKillCountToPlayer();

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

    private void GetKillCountToPlayer()
    {
        killCount = player.GetComponent<PlayerStatus>().KillCount;
    }

    public override string UpdateDescription()
    {
        description = $"Àû Ã³Áö : {killCount} / {MAX_KILL_COUNT}";
        return description;
    }

    public override void OpenDoor()
    {
        Door = GameObject.Find("Door_4").GetComponent<Door>();
        Door.OpenDoor();
    }
}
