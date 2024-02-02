using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.HID;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public float speed = 10.0f;

    public float sensitivity = 30.0f;
    public float WaterHeight = 15.5f;
    CharacterController character;
    public GameObject cam;
    float moveFB, moveLR;
    float rotX, rotY;
    public bool webGLRightClickRotation = true;
    float gravity = -9.8f;

    public bool otherAction = false;

    void Start()
    {
        character = GetComponent<CharacterController>();
        if (Application.isEditor)
        {
            webGLRightClickRotation = false;
            sensitivity = sensitivity * 1.5f;
            //LockCursor();
        }
        else
        {
            LockCursor();
        }
    }

    void CheckForWaterHeight()
    {
        if (transform.position.y < WaterHeight)
        {
            gravity = 0f;
        }
        else
        {
            gravity = -9.8f;
        }
    }

    void Update()
    {
        if (otherAction == true)
            return;

        moveFB = Input.GetAxis("Horizontal") * speed;
        moveLR = Input.GetAxis("Vertical") * speed;

        rotX = Input.GetAxis("Mouse X") * sensitivity;
        rotY = Input.GetAxis("Mouse Y") * sensitivity;

        CheckForWaterHeight();
        Vector3 movement = new Vector3(moveFB, gravity, moveLR);

        if (webGLRightClickRotation)
        {
            if (Input.GetKey(KeyCode.Mouse0))
            {
                CameraRotation(cam, rotX, rotY);
            }
        }
        else if (!webGLRightClickRotation)
        {
            CameraRotation(cam, rotX, rotY);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            PlayerStatus.Instance.playerStaminabar.fillAmount -= 0.3f;
        }

        movement = transform.rotation * movement;
        character.Move(movement * Time.deltaTime);

        PlayerStatus.Instance.RechargingStamina();
        PlayerStatus.Instance.PlayerDeath();
    }

    void CameraRotation(GameObject cam, float rotX, float rotY)
    {
        transform.Rotate(0, rotX * Time.deltaTime, 0);
        cam.transform.Rotate(-rotY * Time.deltaTime, 0, 0);
    }

    void LockCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
}
