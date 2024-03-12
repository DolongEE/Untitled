using UnityEditor.PackageManager.Requests;
using UnityEngine;

public class TutorialQuestStep_3 : QuestStep
{
    private EquipmentInventory equipmentInventory;
    private bool equipHelmet;
    private bool equipArmor;

    private void Awake()
    {
        equipmentInventory = Managers.INVENTORY.equipment;  

        CheckEquipItem();
    }

    void Update()
    {
        CheckEquipItem();

        if (equipHelmet && equipArmor)
        {
            FinishedQuestStep();
        }
        _questUI.questDescription.text = UpdateDescription();
    }

    public override string UpdateDescription()
    {
        string reuslt1 = equipArmor ? "1/1" : "0/1";
        string reuslt2 = equipHelmet ? "1/1" : "0/1";
        description = $"Armor :{reuslt1} \nHelmet{reuslt2}";
        return description;
    }

    private void CheckEquipItem()
    {
        foreach (var slot in equipmentInventory.equipSlots)
        {
            if (equipHelmet == false && slot.equipmentType == EItemType.Helmet && slot.Item != null)
            {
                equipHelmet = true;
            }
            if (equipArmor == false && slot.equipmentType == EItemType.Armor && slot.Item != null)
            {
                equipArmor = true;
            }
        }
    }

    public override void OpenDoor()
    {
        Door = GameObject.Find("Door_3").GetComponent<Door>();
        Door.OpenDoor();
    }
}
