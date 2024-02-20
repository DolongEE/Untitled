using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CraftPoint : MonoBehaviour
{
    [Header("Dialogue")]
    [SerializeField] private NPCInfoDialogSO npcInfoDialog;

    [Header("Object")]
    [SerializeField] private GameObject canvasLogBox;
    [SerializeField] private GameObject canvasCraft;
    [SerializeField] private TextMeshProUGUI txtLogBox;

    [SerializeField] private bool playerIsNear = false;
    [SerializeField] private CraftStates currentCraftState;

    [SerializeField] private List<Item> items = new List<Item>();

    private PlayerController playerController;
    private bool isCrafting;
    private int logCount;

    private void Awake()
    {
        canvasLogBox = transform.Find("DialogCanvas").gameObject;
        canvasCraft = transform.Find("CraftCanvas").gameObject;
        txtLogBox = canvasLogBox.GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Start()
    {        
        currentCraftState = CraftStates.NONE;
        canvasLogBox.SetActive(false);
        canvasCraft.SetActive(false);
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
            canvasLogBox.SetActive(true);
            txtLogBox.text = logs[logCount++];
        }
        else
        {
            if (currentCraftState.Equals(CraftStates.TALK))
            {
                OpenCraftWindow();
            }
            logCount = 0;
            canvasLogBox.SetActive(false);
        }
    }

    private void OpenCraftWindow()
    {
        isCrafting = true;
        canvasCraft.SetActive(true);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerController = other.GetComponent<PlayerController>();
            playerIsNear = true;
            items = other.GetComponentInChildren<Inventory>().items;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerController = null;
            playerIsNear = false;
            //items.Clear();
        }
    }
}
