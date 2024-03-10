using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField]private bool isOpen;
    private Quaternion rotate;

    private void Awake()
    {        
        rotate = Quaternion.Euler(0, transform.eulerAngles.y - 80, 0);
    }

    void Update()
    {
        if (isOpen)
            transform.rotation = Quaternion.Slerp(transform.rotation, rotate, 0.1f);
    }

    public void OpenDoor()
    {
        isOpen = true;
    }
}
