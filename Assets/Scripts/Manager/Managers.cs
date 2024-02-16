
using UnityEngine;

public class Managers : MonoBehaviour
{
    private static Managers s_instance = null;
    public static Managers Instance { get { return s_instance; } }

    #region Core    
    private static CoroutineManager s_coroutine = new CoroutineManager();
    private static GameEventsManager s_event = new GameEventsManager();
    private static CreatureManager s_creature = new CreatureManager();
    private static InventoryManager s_inventory = new InventoryManager();
    private static QuestManager s_quest = new QuestManager();
    private static ResourcesManager s_resource = new ResourcesManager();
    private static CraftManager s_synthetic = new CraftManager();

    public static CoroutineManager COROUTINE { get { return s_coroutine; } }
    public static GameEventsManager EVENT { get { return s_event; } }
    public static CreatureManager CREATURE { get { return s_creature; } }
    public static InventoryManager INVENTORY { get { return s_inventory; } }
    public static QuestManager QUEST { get { return s_quest; } }
    public static ResourcesManager RESOURCE { get { return s_resource; } }
    public static CraftManager SYNTHETIC { get { return s_synthetic; } }
    #endregion

    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        s_quest.Update();
        s_inventory.Update();
    }

    private void OnEnable()
    {
        s_quest.Enable();

    }
    private void OnDisable()
    {
        Clear();
    }

    private static void Init()
    {
        GameObject go = GameObject.Find("@Managers");
        if (s_instance == null)
        {
            if (go == null)
                go = new GameObject { name = "@Managers" };
            DontDestroyOnLoad(go);
        }
        s_resource.Init();
        s_event.Init();       
        s_creature.Init();
        s_inventory.Init();
        s_quest.Init();
    }

    public GameObject CreateObject(string objName, Transform parent)
    {
        GameObject go = new GameObject { name = objName };
        go.transform.SetParent(parent);

        return go;
    }

    public static void Clear()
    {
        s_quest.Clear();
        s_coroutine.Clear();
    }
}
