﻿using UnityEngine;
using System.Collections;


public class CameraController : MonoBehaviour
{
    public Vector3 offset;
    public float ZoomSpeed;
    public Vector3 newTarget;
     public float newAngleX;
    public float newAngleY;
    public float defaultZoom;
    public float moveSpeed;
    public float rotateSpeed;
    public float minZoomOut = -4;
    public float maxZoomOut = -40;
    [HideInInspector]
    public Camera playerCamera;
    private float newZoom;
    private float currentAngleY;
    private float currentAngleX;
    private void Awake()
    {
        Cursor.visible=false;
        playerCamera = GetComponentInChildren<Camera>();
        currentAngleY = 0;
        currentAngleX = 0;
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

    public void SlideToRotation(float angleX,float angleY)
    {
        newAngleX = angleX;
        newAngleY = angleY;
    }


    // Update is called once per frame
    void LateUpdate()
    {
        if (!Input.GetMouseButton(0))
        {
        SlideToRotation(newAngleX-Input.GetAxis("Mouse Y") * 5f,newAngleY+Input.GetAxis("Mouse X") * 5f);
        }
        currentAngleY = Mathf.Lerp(currentAngleY, newAngleY, rotateSpeed * Time.deltaTime);
        currentAngleX = Mathf.Lerp(currentAngleX, newAngleX, rotateSpeed * Time.deltaTime);
        transform.localEulerAngles = new Vector3(currentAngleX, currentAngleY, 0);
        Vector3 rotatedOffset = Quaternion.AngleAxis(-currentAngleX, Vector3.left) * new Vector3(1,offset.y,offset.z);
        rotatedOffset+= Quaternion.AngleAxis(currentAngleY, Vector3.up) * new Vector3(1,offset.y,offset.z);
        rotatedOffset-= offset;
        Vector3 newPos=Vector3.Lerp(transform.position, newTarget + rotatedOffset, moveSpeed * Time.deltaTime);
        while(Physics.Raycast(newPos,transform.forward,10f))
        {
         newPos+= 0.1f*transform.forward;
        }
        transform.position = newPos;   
    }
}
