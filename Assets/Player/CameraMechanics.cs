using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMechanics : MonoBehaviour
{
    [HideInInspector]
    public Transform player;
    public float smooth;
    public Vector3 offset;
    public float magnetToMouse;

    public float zoomSpeed;
    public float minZoom;
    public float maxZoom;

    private float _scrollInput;
    private Vector3 _velocity = Vector3.zero;

    void FixedUpdate()
    { 
        _FollowPlayer();
    }

    void Update()
    {
        _Zooming();
    }

    private void _FollowPlayer() {
        if (player != null) {
            if (Vector2.Distance(transform.position, player.position) > GameManager.gameManager.PartOfMapSize(0.25f)) {
                transform.position = player.position;
            }
            else {
                Vector3 mousePosition_ = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                transform.position = Vector3.SmoothDamp(transform.position, Vector3.Lerp(player.position, mousePosition_, magnetToMouse) + offset, ref _velocity, smooth * Time.deltaTime);
            }
        }
    } 

    private void _Zooming() {
        // Get the mouse wheel input
        _scrollInput = Input.GetAxis("Mouse ScrollWheel");

        if (_scrollInput != 0) {
            // Calculate the new zoom level based on the scroll input
            float newZoom_ = Camera.main.orthographicSize - _scrollInput * zoomSpeed;

            // Clamp the zoom level to the defined range
            newZoom_ = Mathf.Clamp(newZoom_, minZoom, maxZoom);

            // Apply the new zoom level to the camera's orthographic size
            Camera.main.orthographicSize = newZoom_;
        }
    }
}
