using UnityEngine;

public class TutorialQuestStep_2 : QuestStep
{
    private int twigAmount;
    private int gravelAmount;

    private const int MAX_TWIG_AMOUNT = 1;
    private const int MAX_GRAVEL_AMOUNT = 2;

    private string twigId;
    private string gravelId;

    private Inventory inventory;

    private void Awake()
    {

        inventory = Managers.INVENTORY.inventory;
        twigId = Managers.ItemDatabase.GetItemId("Twig");
        gravelId = Managers.ItemDatabase.GetItemId("Gravel");
        GetItemAmount();
    }

    void Update()
    {
        GetItemAmount();

        if (twigAmount >= MAX_TWIG_AMOUNT && gravelAmount >= MAX_GRAVEL_AMOUNT)
        {
            FinishedQuestStep();
        }
        _questUI.questDescription.text = UpdateDescription();
    }

    public override string UpdateDescription()
    {
        description = $"나뭇가지 : {twigAmount} / {MAX_TWIG_AMOUNT}\n자갈 : {gravelAmount} / {MAX_GRAVEL_AMOUNT}";
        return description;
    }

    private void GetItemAmount()
    {
        twigAmount = inventory.ItemCount(twigId);
        gravelAmount = inventory.ItemCount(gravelId);
    }

    public override void OpenDoor()
    {
        Door = GameObject.Find("Door_2").GetComponent<Door>();
        Door.OpenDoor();
    }
}
