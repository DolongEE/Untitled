using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class NPC : Creature
{
    [SerializeField] protected GameObject panelLogBox;
    [SerializeField] protected TextMeshProUGUI txtLogBox;

    [SerializeField] protected bool playerIsNear = false;

    protected override bool Init()
    {
        if (base.Init() == false)
            return false;

        CreatureType = CreatureType.NPC;
        Managers.EVENT.creatureEvents.CreatureCreate(this);
        _health.SetHealth(100);

        panelLogBox = GameObject.Find("LogBoxPanel");
        txtLogBox = panelLogBox.GetComponentInChildren<TextMeshProUGUI>();
        panelLogBox.SetActive(false);

        return true;
    }

    private void OnEnable()
    {
        Managers.EVENT.inputEvents.onQuestLogTogglePressed += TogglePressed;
    }

    private void OnDisable()
    {
        Managers.EVENT.inputEvents.onQuestLogTogglePressed -= TogglePressed;
    }

    protected virtual void TogglePressed() { }
    protected virtual void Talking(string[] logs) { }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNear = true;
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {

    }
}
