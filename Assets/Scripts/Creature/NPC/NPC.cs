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

    [HideInInspector] public GameObject panelLogBox;
    [HideInInspector] public GameObject panelLogBoxButtons;
    [HideInInspector] public TextMeshProUGUI txtLogBox;
    [HideInInspector] public GameObject panelCraft;
    [HideInInspector] public GameObject questIcon;
    [HideInInspector] public bool playerIsNear = false;

    public Button btnQuestTalk;
    private Button btnCraftOpen;
    private int logCount;

    private CraftPoint craft;
    private QuestPoint quest;

    public PlayerAnimation playerAnimation;

    private void OnValidate()
    {
        quest = GetComponent<QuestPoint>();
        craft = GetComponent<CraftPoint>();
        quest.enabled = useQuest;
        craft.enabled = useCrafting;

        Transform canvas = transform.Find("NPC_Canvas");
        panelLogBox = canvas.Find("LogBoxPanel").gameObject;
        panelLogBoxButtons = panelLogBox.transform.Find("LogBoxButtons").gameObject;
        txtLogBox = panelLogBox.transform.Find("txtLogBox").GetComponent<TextMeshProUGUI>();
        btnQuestTalk = panelLogBoxButtons.transform.Find("btnQuestTalk").GetComponent<Button>();
        btnCraftOpen = panelLogBoxButtons.transform.Find("btnCraftOpen").GetComponent<Button>();
    }

    private void Awake()
    {
        panelCraft = Managers.CANVAS.panelCraft;

        btnQuestTalk.onClick.AddListener(OnClickQuestTalk);
        btnCraftOpen.onClick.AddListener(OnClickCraftOpen);
        btnQuestTalk.gameObject.SetActive(useQuest);
        btnCraftOpen.gameObject.SetActive(useCrafting);

        questIcon = GetComponentInChildren<QuestIcon>().gameObject;
        questIcon.SetActive(useQuest);

        panelLogBoxButtons.SetActive(false);
        panelLogBox.SetActive(false);
    }

    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        _health.SetHealth(100f);


        return true;
    }

    private void OnEnable()
    {
        TalkToggleAdd();
    }

    private void OnDisable()
    {
        TalkToggleRemove();
    }

    private void ToggleGPressed()
    {
        if (playerIsNear == false || quest.isQuestTalk == true || craft.isCrafting == true) return;
        if (btnQuestTalk.gameObject.activeSelf == false && btnCraftOpen.gameObject.activeSelf == false) return;

        if (dialogue != null) 
            Talking(dialogue.init);
    }

    private void Talking(string[] logs)
    {
        if (logCount < logs.Length)
        {
            playerAnimation.Talk(true);
            panelLogBox.SetActive(true);
            txtLogBox.text = logs[logCount++];
        }
        else
        {
            panelLogBoxButtons.SetActive(true);
        }
    }

    public void OnClickQuestTalk()
    {
        logCount = 0;
        TalkToggleRemove();
        quest.isQuestTalk = true; 
        panelLogBoxButtons.SetActive(false);       
        Managers.EVENT.inputEvents.ToggleGPressed();
    }

    public void OnClickCraftOpen()
    {
        logCount = 0;
        panelLogBox.SetActive(false);
        craft.OpenCraftWindow();
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNear = true;
            playerAnimation = other.GetComponentInChildren<PlayerAnimation>();
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNear = false;
            playerAnimation = null;
        }
    }

    public void TalkToggleAdd()
    {
        Managers.EVENT.inputEvents.onToggleGPressed += ToggleGPressed;
    }
    public void TalkToggleRemove()
    {
        Managers.EVENT.inputEvents.onToggleGPressed -= ToggleGPressed;
    }
}
