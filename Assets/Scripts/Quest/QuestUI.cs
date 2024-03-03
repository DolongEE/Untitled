
using TMPro;
using UnityEngine;

public class QuestUI : MonoBehaviour
{    
    [SerializeField] public TextMeshProUGUI questName;
    [SerializeField] public TextMeshProUGUI questDescription;

    private void Awake()
    {
        questName = transform.Find("Quest Name Tag").GetComponentInChildren<TextMeshProUGUI>();
        questDescription = transform.Find("Quest Description Tag").GetComponentInChildren<TextMeshProUGUI>();
    }



}
