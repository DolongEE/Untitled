using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CraftPoint : NPC
{
    [Header("Dialogue")]
    [SerializeField] private NPCInfoDialogSO npcInfoDialog;
    [SerializeField] private GameObject panelCraft;
    [SerializeField] private CraftStates currentCraftState;

    private bool isCrafting;
    private int logCount;

    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        panelCraft = GameObject.Find("CraftPanel");

        return true;
    }

    private void Start()
    {        
        currentCraftState = CraftStates.NONE;
        panelCraft.SetActive(false);
    }

    protected override void TogglePressed()
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

    protected override void Talking(string[] logs)
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

    protected override void OnTriggerExit(Collider other)
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
