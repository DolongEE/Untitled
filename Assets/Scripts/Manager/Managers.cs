using UnityEngine;

public class Managers : MonoBehaviour
{
    private static Managers s_instance = null;
    public static Managers Instance { get { return s_instance; } }

    #region Core
    private static NPCManager _npc = new NPCManager();

    public static NPCManager NPC { get { return _npc; } }
    #endregion

    private void Awake()
    {

    }

    private void Start()
    {
        Init();
    }

    private void Update()
    {

    }

    private static void Init()
    {
        if (s_instance == null)
        {
            GameObject manager = GameObject.Find("@Managers");
            if (manager == null)
                manager = new GameObject { name = "@Managers" };

            DontDestroyOnLoad(manager);

        }
    }
}
