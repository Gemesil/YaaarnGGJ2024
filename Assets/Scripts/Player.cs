using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject aimArrow;
    public GameObject yarnPiece;
    public float yarnLifetime = 8f;
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
     float depth = 0.2f;
    RaycastHit hit;
     if (Physics.Raycast(transform.position, aimArrow.transform.forward, out hit, Mathf.Infinity, layerMask))
    {
           float power= 500f*Mathf.Sqrt(hit.distance);
            StartCoroutine(ShootYarn((int)Mathf.Floor(hit.distance/depth),power));
    }

}

private IEnumerator ShootYarn(int pieceCount,float power)
{
   float depth = 0.2f;
   Vector3 dir=aimArrow.transform.forward;
   Quaternion aimArrowrRotation = aimArrow.transform.rotation;
   Vector3 startPos = transform.position;
   List<GameObject> yarns= new List<GameObject>();
   for(int i=0; i<pieceCount;i++)
   {
   yarns.Add(Instantiate(yarnPiece,startPos+dir*depth*i,aimArrowrRotation));
   if(pieceCount<50)
   {
    if(i%2==0)
    {
       yield return null;
    }
   }
   else if(i%7==0)   
   {
       yield return null;
   }
   }
       yield return null;
   rigidBody.AddForce(dir*power);
   yield return new WaitForSeconds(yarnLifetime);
for (int i = 0; i < yarns.Count; i++)
{
    Destroy(yarns[i]);
}
}



}
