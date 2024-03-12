using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    public float speed = 10.0f;

    public float sensitivity;
    public float WaterHeight = 15.5f;
    public GameObject cam;
    private CharacterController character;
    private float moveFB, moveLR;
    private float rotX, rotY;
    private float gravity = -9.8f;

    public Texture2D cursorTexture;
    private Vector2 cursorHotspot;

    void Start()
    {
        character = GetComponent<CharacterController>();
        cursorHotspot = new Vector2(0f, 0f);
        Cursor.SetCursor(cursorTexture, cursorHotspot, CursorMode.ForceSoftware);

        sensitivity *= sensitivity;
    }
    void Update()
    {
        if (Managers.otherAction)
            return;

        moveFB = Input.GetAxis("Horizontal") * speed;
        moveLR = Input.GetAxis("Vertical") * speed;

        rotX = Input.GetAxis("Mouse X") * sensitivity;
        rotY = Input.GetAxis("Mouse Y") * sensitivity;

        gravity = -9.8f;
        Vector3 movement = new Vector3(moveFB, gravity, moveLR);

        if (EventSystem.current.IsPointerOverGameObject())
            return;

        CameraRotation(cam, rotX, rotY);

        movement = transform.rotation * movement;
        character.Move(movement * Time.deltaTime);

        PlayerStatus.Instance.RechargingStamina();
        PlayerStatus.Instance.PlayerDeath();
    }

    void CameraRotation(GameObject cam, float rotX, float rotY)
    {
        transform.Rotate(0, rotX * Time.deltaTime, 0);

        float clampRotY = Mathf.Clamp(cam.transform.rotation.eulerAngles.x - rotY * Time.deltaTime, 0f, 30f);
        cam.transform.rotation = Quaternion.Euler(clampRotY, cam.transform.rotation.eulerAngles.y, cam.transform.rotation.eulerAngles.z);
    }
}