using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class NPC : Creature
{
    [SerializeField]protected GameObject panelLogBox;
    [SerializeField] protected TextMeshProUGUI txtLogBox;
    protected PlayerController playerController;

    [SerializeField] protected bool playerIsNear = false;
    protected bool PlayerOtherAction
    {
        set
        {
            playerController.otherAction = value;
        }
    }
    private void OnValidate()
    {
        panelLogBox = GameObject.Find("LogBoxPanel");
    }

    protected override bool Init()
    {
        if (base.Init() == false)
            return false;
                
        txtLogBox = panelLogBox.GetComponentInChildren<TextMeshProUGUI>();

        CreatureType = CreatureType.NPC;
        Managers.EVENT.creatureEvents.CreatureCreate(this);
        _health.SetHealth(100); 

        panelLogBox.SetActive(false);

        return true;
    }

    private void OnEnable()
    {
        Managers.EVENT.inputEvents.onToggleGPressed += ToggleGPressed;        
    }

    private void OnDisable()
    {
        Managers.EVENT.inputEvents.onToggleGPressed -= ToggleGPressed;
    }

    protected virtual void ToggleGPressed() { }
    protected virtual void Talking(string[] logs) { }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNear = true;
            playerController = other.GetComponent<PlayerController>();
        }
    }

    protected virtual void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerIsNear = false;
            PlayerOtherAction = false;
            playerController = null;
        }
    }
}
