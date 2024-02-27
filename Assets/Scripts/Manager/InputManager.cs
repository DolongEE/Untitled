using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputManager : MonoBehaviour
{
    [SerializeField] private PlayerInput playerInput;

    private void OnValidate()
    {
        playerInput = GetComponent<PlayerInput>();
        playerInput.actionEvents[0].AddListener(SubmitPressed);
        playerInput.actionEvents[1].AddListener(ToggleGPressed);
        playerInput.actionEvents[2].AddListener(EscPressed);
    } 

    public void SubmitPressed(InputAction.CallbackContext context)
    {        
        if (context.started)
        {
            Managers.EVENT.inputEvents.SubmitPressed();
        }
    }

    public void ToggleGPressed(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Managers.EVENT.inputEvents.ToggleGPressed();
        }
    }

    public void EscPressed(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            Managers.EVENT.inputEvents.EscPressed();
        }
    }
}
