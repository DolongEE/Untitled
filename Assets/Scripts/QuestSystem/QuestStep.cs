using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class QuestStep : MonoBehaviour
{
    private bool isFinished = false;

    protected void FinishedQuestStep()
    {
        if (isFinished == false)
        {
            isFinished = true;

            // TODO - 

            Destroy(gameObject);
        }
    }
}
