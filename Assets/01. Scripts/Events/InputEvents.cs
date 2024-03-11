using System;

public class InputEvents
{
    public event Action onSubmitPressed;
    public void SubmitPressed()
    {
        if (onSubmitPressed != null) 
        {
            onSubmitPressed();
        }
    }
    public event Action onToggleGPressed;
    public void ToggleGPressed()
    {
        if (onToggleGPressed != null) 
        {
            onToggleGPressed();            
        }
    }

    public event Action onEscPressed;
    public void EscPressed()
    {
        if (onEscPressed != null)
        {
            onEscPressed();
        }
    }
}
