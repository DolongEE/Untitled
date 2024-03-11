using UnityEngine;
using UnityEngine.UI;

public class CanvasManager
{
    public GameObject panelCraft;
    public GameObject panelQuestContent;    

    public void Init()
    {
        panelQuestContent = GameObject.Find("QuestPanel").GetComponentInChildren<GridLayoutGroup>().gameObject;
        panelCraft = GameObject.Find("CraftPanel");
        panelCraft.SetActive(false);
    }
}
