using UnityEngine;

public class Managers : MonoBehaviour
{
    private static Managers s_instance = null;
    public static Managers Instance { get { return s_instance; } }

    #region Core
    private static GameEventsManager s_event = new GameEventsManager();
    private static MonsterManager s_monster = new MonsterManager();
    private static QuestManager s_quest = new QuestManager();
    private static ResourcesManager s_resource = new ResourcesManager();

    public static GameEventsManager EVENT {  get { return s_event; } }
    public static MonsterManager MONSTER { get { return s_monster; } }
    public static QuestManager QUEST { get { return s_quest; } }
    public static ResourcesManager RESOURCE { get {  return s_resource; } }
    #endregion

    private void Awake()
    {
        Init();
    }

    private void Update()
    {
        s_quest.Update();
    }

    private void OnEnable()
    {
        s_quest.Enable();
        
    }
    private void OnDisable()
    {
        s_quest.Disable();
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
        s_monster.Init();
        s_quest.Init();
        Debug.Log("Manager Init Success");
    }    

    public GameObject CreateObject(string objName, Transform parent)
    {
        GameObject go = new GameObject { name = objName };
        go.transform.SetParent(parent);

        return go;
    }



}
