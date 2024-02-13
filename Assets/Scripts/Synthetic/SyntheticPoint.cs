using TMPro;
using UnityEngine;

public class SyntheticPoint : MonoBehaviour
{
    [Header("Dialogue")]
    [SerializeField] private NPCInfoDialogSO npcInfoDialog;

    [Header("Object")]
    [SerializeField] private GameObject canvasLogBox;
    [SerializeField] private GameObject canvasSynthetic;
    [SerializeField] private TextMeshProUGUI txtLogBox;

    [SerializeField] private bool playerIsNear = false;
    [SerializeField] private SyntheticStates currentSyntheticState;

    private int logCount;

    private void Awake()
    {
        currentSyntheticState = SyntheticStates.NONE;
        canvasLogBox.SetActive(false);
        canvasSynthetic.SetActive(false);
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
        if (playerIsNear == false)
            return;

        if (npcInfoDialog != null)
        {
            switch (currentSyntheticState)
            {
                case SyntheticStates.NONE:
                    currentSyntheticState = SyntheticStates.TALK;
                    Talking(npcInfoDialog.init);
                    break;
                case SyntheticStates.SYNTHETIC:
                    SyntheticProgress();
                    break;
                case SyntheticStates.TALK:
                    break;
                case SyntheticStates.EXIT:
                    Talking(npcInfoDialog.end);
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
            if (currentSyntheticState.Equals(SyntheticStates.TALK))
            {
                currentSyntheticState = SyntheticStates.EXIT;
            }
            else if (currentSyntheticState.Equals(SyntheticStates.EXIT))
            {
                currentSyntheticState = SyntheticStates.NONE;
            }
            logCount = 0;
            canvasLogBox.SetActive(false);
        }
    }


    private void SyntheticProgress()
    {
        canvasSynthetic.SetActive(true);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNear = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNear = false;
        }
    }
}
