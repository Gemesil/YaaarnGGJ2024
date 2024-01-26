using UnityEngine;
using System.Collections;


public class CameraController : MonoBehaviour
{
    public Vector3 offset;
    public float ZoomSpeed;
    public Vector3 newTarget;
    public float newAngle;
    public float defaultZoom;
    public float moveSpeed;
    public float rotateSpeed;
    public float minZoomOut = -4;
    public float maxZoomOut = -40;
    [HideInInspector]
    public Camera playerCamera;
    private float newZoom;
    private float currentAngle;

    private void Awake()
    {
        playerCamera = GetComponentInChildren<Camera>();
        currentAngle = 0;
        NewZoom = defaultZoom;
        playerCamera.transform.localPosition = Vector3.forward * NewZoom;
    }


    public float NewZoom
    {
        get
        {
            return newZoom;
        }

        set
        {
            newZoom = Mathf.Clamp(value, maxZoomOut, minZoomOut);
        }
    }

    public void SlideToPosition(Vector3 target)
    {
        newTarget = target;
    }

    public void JumpToPosition(Vector3 target)
    {
        newTarget = target;
        transform.position = target + offset;
    }

    public void SlideToRotation(float angle)
    {
        newAngle = angle;
    }


    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(transform.position, newTarget + offset, moveSpeed * Time.deltaTime);
        currentAngle = Mathf.Lerp(currentAngle, newAngle, rotateSpeed * Time.deltaTime);
        transform.localEulerAngles = new Vector3(0, currentAngle, 0);
    }
}
