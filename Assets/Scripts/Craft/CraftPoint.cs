using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CraftPoint : MonoBehaviour
{
    [Header("Dialogue")]
    [SerializeField] private NPCInfoDialogSO npcInfoDialog;

    [Header("Object")]
    [SerializeField] private GameObject panelLogBox;
    [SerializeField] private GameObject panelCraft;
    [SerializeField] private TextMeshProUGUI txtLogBox;

    [SerializeField] private bool playerIsNear = false;
    [SerializeField] private CraftStates currentCraftState;

    private bool isCrafting;
    private int logCount;

    private void Awake()
    {
        panelLogBox = GameObject.Find("LogBoxPanel");
        panelCraft = GameObject.Find("CraftPanel");
        txtLogBox = panelLogBox.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {        
        currentCraftState = CraftStates.NONE;
        panelLogBox.SetActive(false);
        panelCraft.SetActive(false);
    }

    private void OnEnable()
    {
        Managers.EVENT.inputEvents.onSubmitPressed += SubmitPressed;
        Managers.EVENT.inputEvents.onQuestLogTogglePressed += TogglePressed;
    }

    private void OnDisable()
    {
        Managers.EVENT.inputEvents.onSubmitPressed -= SubmitPressed;
        Managers.EVENT.inputEvents.onQuestLogTogglePressed -= TogglePressed;
    }

    private void SubmitPressed()
    {

    }

    private void TogglePressed()
    {        
        if (isCrafting) return;

        if (playerIsNear == false)
            return;

        if (npcInfoDialog != null)
        {
            switch (currentCraftState)
            {
                case CraftStates.NONE:                    
                    currentCraftState = CraftStates.TALK;
                    break;
                case CraftStates.TALK:
                    Talking(npcInfoDialog.init);
                    break;
            }
        }
    }

    private void Talking(string[] logs)
    {        
        if (logCount < logs.Length)
        {
            panelLogBox.SetActive(true);
            txtLogBox.text = logs[logCount++];
        }
        else
        {
            if (currentCraftState.Equals(CraftStates.TALK))
            {
                OpenCraftWindow();
            }
            logCount = 0;
            panelLogBox.SetActive(false);
        }
    }

    private void OpenCraftWindow()
    {
        isCrafting = true;
        panelCraft.SetActive(true);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNear = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNear = false;
            isCrafting = false;
            panelCraft.SetActive(playerIsNear);
            currentCraftState = CraftStates.NONE;
        }
    }
}
