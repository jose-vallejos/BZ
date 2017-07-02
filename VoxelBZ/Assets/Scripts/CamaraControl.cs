using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class CamaraControl : MonoBehaviour
{

    public float turnSpeed = 15.0f;
    public float panSpeed = 10.0f;
    public float zoomSpeed = 10.0f;

    private Vector3 mouseOrigin;
    private bool isPanning;
    private bool isRotating;
    private bool isZooming;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Check if the mouse was clicked over a UI element
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                mouseOrigin = Input.mousePosition;
                isRotating = true;
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                mouseOrigin = Input.mousePosition;
                isPanning = true;
            }
        }

        if (Input.GetMouseButtonDown(2))
        {
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                mouseOrigin = Input.mousePosition;
                isZooming = true;
            }

        }

        if (!Input.GetMouseButton(0)) isRotating = false;
        if (!Input.GetMouseButton(1)) isPanning = false;
        if (!Input.GetMouseButton(2)) isZooming = false;

        if (isRotating)
        {
            Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - mouseOrigin);

            transform.RotateAround(transform.position, transform.right, -pos.y * turnSpeed);
            transform.RotateAround(transform.position, Vector3.up, pos.x * turnSpeed);
        }

        if (isPanning)
        {
            Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - mouseOrigin);

            Vector3 move = new Vector3(pos.x * panSpeed, pos.y * panSpeed, 0);
            transform.Translate(move, Space.Self);
        }

        if (isZooming)
        {
            Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - mouseOrigin);

            Vector3 move = pos.y * zoomSpeed * transform.forward;
            transform.Translate(move, Space.World);
        }
    }
}