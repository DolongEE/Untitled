using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{    
    private Vector3 moveDirection;
    private float moveSpeed = 4f;
    Camera camera;

    void Start()
    {
        camera = Camera.main;
    }

    void Update()
    {
        bool hasControl = (moveDirection != Vector3.zero);
        if(hasControl)
        {
            transform.rotation = Quaternion.LookRotation(moveDirection);
            transform.Translate(Vector3.forward * moveSpeed * Time.deltaTime);
        }
    }

    private void LateUpdate()
    {
        
    }

    void OnMove(InputValue value)
    {
        Vector3 input = value.Get<Vector3>();
        if (input != null)
        {
            moveDirection = new Vector3(input.x, input.y, input.z);
        }
    }
}
