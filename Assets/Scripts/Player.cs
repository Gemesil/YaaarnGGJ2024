using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject aimArrow;
private CameraController cameraController;
private Rigidbody rigidBody;

void Start()
{
    cameraController = Camera.main.gameObject.GetComponent<CameraController>(); 
    rigidBody= GetComponent<Rigidbody>();
}

    // Update is called once per frame
    void Update()
    {
     cameraController.SlideToPosition(transform.position);
    }

public void RotateAimArrow(float xAxis, float yAxis)
{
    aimArrow.transform.Rotate(xAxis,yAxis,0);
    Vector3 eulerRotation = aimArrow.transform.rotation.eulerAngles;
    aimArrow.transform.rotation = Quaternion.Euler(eulerRotation.x, eulerRotation.y, 0);
}

public void ReleaseYarn()
{
    int layerMask = ~gameObject.layer;
    RaycastHit hit;
     if (Physics.Raycast(transform.position, aimArrow.transform.forward, out hit, Mathf.Infinity, layerMask))
        {
        float power= 500f*Mathf.Sqrt(hit.distance);
         rigidBody.AddForce(aimArrow.transform.forward*power);
        }

}


}
