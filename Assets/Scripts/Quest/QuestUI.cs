
using TMPro;
using UnityEngine;

public class QuestUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI questName;
    [SerializeField] private TextMeshProUGUI questDescription;

    private void Awake()
    {
        questName = transform.Find("Quest Name Tag").GetComponentInChildren<TextMeshProUGUI>();
        questDescription = transform.Find("Quest Description Tag").GetComponentInChildren<TextMeshProUGUI>();
    }



}
