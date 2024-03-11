using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour
{
    [Header("Camera")]
    private Transform player;
    private LayerMask obstacleLayer;
    private float minDistance = 2f;
    private float maxDistance = 5f;
    private float distanceFromPlayer = 5f;
    private float cameraSpeed = 3f;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();
        obstacleLayer = LayerMask.GetMask("Wall");
    }

    private void LateUpdate()
    {
        HandleCameraCollision();
    }

    private void HandleCameraCollision()
    {
        distanceFromPlayer = Mathf.Clamp(distanceFromPlayer, minDistance, maxDistance);
        
        Vector3 desiredPosition = player.position - transform.forward * distanceFromPlayer;
        RaycastHit hit;

        if (Physics.Raycast(player.position, -transform.forward, out hit, distanceFromPlayer, obstacleLayer))
        {
            Vector3 hitPoint = hit.point;
            transform.position = Vector3.Lerp(transform.position, hitPoint, Time.deltaTime * cameraSpeed);
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * cameraSpeed);
        }

        transform.LookAt(player.position);
    }
}