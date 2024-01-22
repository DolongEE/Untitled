using UnityEngine;

public class Managers : MonoBehaviour
{
    private static Managers s_instance = null;
    public static Managers Instance { get { return s_instance; } }

    #region Core


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
