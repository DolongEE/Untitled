using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialQuestStep_1 : QuestStep
{
    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            FinishedQuestStep();
        }
    }
}
