using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class CanvasManager
{
    public GameObject panelLogBox;
    public GameObject panelLogBoxButtons;
    public GameObject panelCraft;
    public Button btnQuestTalk;
    public Button btnCraftOpen;
    public TextMeshProUGUI txtLogBox;
    

    public void Init()
    {
        panelLogBox = GameObject.Find("LogBoxPanel");
        panelLogBoxButtons = GameObject.Find("LogBoxButtons");
        txtLogBox = GameObject.Find("txtLogBox").GetComponent<TextMeshProUGUI>();
        btnQuestTalk = GameObject.Find("btnQuestTalk").GetComponent<Button>();
        btnCraftOpen = GameObject.Find("btnCraftOpen").GetComponent<Button>();

        panelCraft = GameObject.Find("CraftPanel");
        panelCraft.SetActive(false);
    }
}
