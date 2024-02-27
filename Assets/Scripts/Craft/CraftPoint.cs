using UnityEngine;

public class CraftPoint : MonoBehaviour
{
    private GameObject panelCraft;
    [HideInInspector] public bool isCrafting;

    private NPC npc;

    private void Awake()
    {
        npc = GetComponent<NPC>();
        panelCraft = npc.panelCraft;
        Managers.EVENT.inputEvents.onEscPressed += OnCloseCraftWindow;
    }

    private void OnDisable()
    {
        Managers.EVENT.inputEvents.onEscPressed -= OnCloseCraftWindow;
    }

    public void OpenCraftWindow()
    {
        isCrafting = true;
        panelCraft.SetActive(true);
    }

    private void OnCloseCraftWindow()
    {       
        if (isCrafting == false)
            return;

        isCrafting = false;
        panelCraft.SetActive(false);
        npc.PlayerOtherAction = false;
    }
}
