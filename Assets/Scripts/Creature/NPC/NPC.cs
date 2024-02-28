using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(QuestPoint))]
[RequireComponent(typeof(CraftPoint))]
public class NPC : Creature
{
    [SerializeField] private NPCInfoDialogSO dialogue;
    [SerializeField] private bool useCrafting;
    [SerializeField] private bool useQuest;    
    private CraftPoint craft;
    private QuestPoint quest;

    [HideInInspector] public GameObject panelLogBox;
    [HideInInspector] public GameObject panelLogBoxButtons;
    [HideInInspector] public TextMeshProUGUI txtLogBox;
    [HideInInspector] public GameObject panelCraft;
    [HideInInspector] public GameObject questIcon;
    [HideInInspector] public bool playerIsNear = false;

    private PlayerController playerController;
    private Button btnQuestTalk;
    private Button btnCraftOpen;
    private int logCount;

    public bool PlayerOtherAction
    {
        set
        {
            playerController.otherAction = value;
        }
    }
    private void OnValidate()
    {        
        quest = GetComponent<QuestPoint>();
        craft = GetComponent<CraftPoint>();
        quest.enabled = useQuest;
        craft.enabled = useCrafting;
    }    

    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        panelLogBox = Managers.CANVAS.panelLogBox;
        panelLogBoxButtons = Managers.CANVAS.panelLogBoxButtons;
        txtLogBox = Managers.CANVAS.txtLogBox;        
        panelCraft = Managers.CANVAS.panelCraft;
        btnQuestTalk = Managers.CANVAS.btnQuestTalk;
        btnCraftOpen = Managers.CANVAS.btnCraftOpen;

        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();

        btnQuestTalk.onClick.AddListener(OnClickQuestTalk);
        btnCraftOpen.onClick.AddListener(OnClickCraftOpen);        
        btnQuestTalk.gameObject.SetActive(useQuest);
        btnCraftOpen.gameObject.SetActive(useCrafting);

        questIcon = GetComponentInChildren<QuestIcon>().gameObject;
        questIcon.SetActive(useQuest);

        _health.SetHealth(100);

        panelLogBoxButtons.SetActive(false);
        panelLogBox.SetActive(false);

        return true;
    }

    private void OnEnable()
    {
        Managers.EVENT.inputEvents.onToggleGPressed += ToggleGPressed;
    }

    private void OnDisable()
    {
        Managers.EVENT.inputEvents.onToggleGPressed -= ToggleGPressed;
    }

    private void ToggleGPressed()
    {
        if (playerIsNear == false || quest.isQuestTalk == true || craft.isCrafting == true)
            return;

        if (quest.currentQuestState.Equals(QuestStates.FINISHED))
            btnQuestTalk.gameObject.SetActive(false);

        PlayerOtherAction = true;
        if (dialogue != null)
        {
            Talking(dialogue.init);
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
            logCount = 0;
            panelLogBoxButtons.SetActive(true);
        }
    }

    public void OnClickQuestTalk()
    {
        panelLogBoxButtons.SetActive(false);
        quest.isQuestTalk = true;
        Managers.EVENT.inputEvents.ToggleGPressed();
    }

    public void OnClickCraftOpen()
    {
        panelLogBox.SetActive(false);
        craft.OpenCraftWindow();
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNear = true;
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNear = false;
        }
    }
}
