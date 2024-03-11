using UnityEngine;

public class CraftPoint : MonoBehaviour
{
    [HideInInspector] public bool isCrafting;

    private NPC npc;

    private void Awake()
    {
        npc = GetComponent<NPC>();
        Managers.EVENT.inputEvents.onEscPressed += OnCloseCraftWindow;
    }

    private void OnDisable()
    {
        Managers.EVENT.inputEvents.onEscPressed -= OnCloseCraftWindow;
    }

    public void OpenCraftWindow()
    {
        isCrafting = true;
        npc.panelCraft.SetActive(true);
    }

    private void OnCloseCraftWindow()
    {       
        if (isCrafting == false)
            return;

        isCrafting = false;
        npc.panelCraft.SetActive(false);
        npc.playerAnimation.GetComponentInChildren<PlayerAnimation>().Talk(false);        
    }
}
