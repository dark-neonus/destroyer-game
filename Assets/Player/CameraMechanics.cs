using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMechanics : MonoBehaviour
{
    public Transform player;
    public float smooth;
    public Vector3 offset;
    public float magnetToMouse;

    public float zoomSpeed;
    public float minZoom;
    public float maxZoom;

    private float scrollInput;

    void FixedUpdate()
    { 
        FollowPlayer();
    }

    void Update()
    {
        Zooming();
    }

    private void FollowPlayer() {
        if (player != null) {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.position = Vector3.Lerp(transform.position, Vector3.Lerp(player.position, mousePosition, magnetToMouse) + offset, smooth);
        }
    } 

    private void Zooming() {
        // Get the mouse wheel input
        scrollInput = Input.GetAxis("Mouse ScrollWheel");

        if (scrollInput != 0) {
            // Calculate the new zoom level based on the scroll input
            float newZoom = Camera.main.orthographicSize - scrollInput * zoomSpeed;

            // Clamp the zoom level to the defined range
            newZoom = Mathf.Clamp(newZoom, minZoom, maxZoom);

            // Apply the new zoom level to the camera's orthographic size
            Camera.main.orthographicSize = newZoom;
        }
    }
}
