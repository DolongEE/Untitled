using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingTrigger : MonoBehaviour
{
    public GameObject endTriggerCanvas;

    private void Start()
    {
        endTriggerCanvas.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            endTriggerCanvas.SetActive(true);
        }
    }
}
