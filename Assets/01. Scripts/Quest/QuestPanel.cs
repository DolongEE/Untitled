using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestPanel : MonoBehaviour
{
    private Button btnWindow;
    private RectTransform QuestWindowRectTransform;
    private bool isOpenQuestWindow;
    private int direction;

    private void Awake()
    {
        btnWindow = GetComponentInChildren<Button>();
        QuestWindowRectTransform = transform.Find("QuestWindow").GetComponent<RectTransform>();
    }

    void Start()
    {       
        btnWindow.onClick.AddListener(OnClickQuestWindow);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
            OnClickQuestWindow();

        QuestWindowMove();
    }

    public void OnClickQuestWindow()
    {
        isOpenQuestWindow = !isOpenQuestWindow;
        btnWindow.GetComponentInChildren<TextMeshProUGUI>().text = isOpenQuestWindow ? ">" : "<";
        direction = isOpenQuestWindow ? 1 : -1;
    }

    private void QuestWindowMove()
    {
        if (-direction * QuestWindowRectTransform.anchoredPosition.x < 200)
        {
            QuestWindowRectTransform.position += Time.deltaTime *1000f * Vector3.left * direction;
        }
    }
}
