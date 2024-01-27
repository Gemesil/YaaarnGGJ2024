using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public GameObject aimArrow;
    public GameObject yarnPiece;
    public float moveSpeed=20f;
    public float yarnLifetime = 8f;
    public float yarnGunCooldown = 1f;
    private CameraController cameraController;
    private Rigidbody rigidBody;
    private bool isYarnGunIncooldown=true;
    private int obstacleLayer = 6;

    void Start()
    {
        cameraController = Camera.main.gameObject.GetComponent<CameraController>();
        rigidBody = GetComponent<Rigidbody>();
        StartCoroutine(DontLetShootYarnAtStart());
    }

    private IEnumerator DontLetShootYarnAtStart()
    {
        yield return new WaitForSeconds(1);
        isYarnGunIncooldown=false;
    }

    // Update is called once per frame
    void Update()
    {
        cameraController.SlideToPosition(transform.position);
    }

    public void RotateAimArrow(float xAxis, float yAxis)
    {
        aimArrow.transform.Rotate(xAxis, yAxis, 0);
        Vector3 eulerRotation = aimArrow.transform.rotation.eulerAngles;
        aimArrow.transform.rotation = Quaternion.Euler(eulerRotation.x, eulerRotation.y, 0);
    }

    public void ReleaseYarn(bool isAttachShoot)
    {
        int layerMask = ~gameObject.layer;
        float depth = 0.2f;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, aimArrow.transform.forward, out hit, Mathf.Infinity, layerMask))
        {
            float power = 500f * Mathf.Sqrt(hit.distance);
            StartCoroutine(ShootYarn((int)Mathf.Floor(hit.distance / depth), power,isAttachShoot));
        }
    }

    public void Move(float xAxis,float yAxis)
    {
        if(xAxis==0 && yAxis==0)
        {
            return;
        }
        Vector3 force=new Vector3(moveSpeed*xAxis,8,moveSpeed*yAxis);
        rigidBody.AddForce(transform.TransformDirection(force));
        Vector3 camPos = cameraController.transform.position;
        camPos.y= transform.position.y;
        Vector3 camForward= cameraController.transform.forward;
        camForward.y=0;
        transform.rotation =  transform.rotation*Quaternion.FromToRotation(transform.forward,camForward );;
    }

    private IEnumerator ShootYarn(int pieceCount, float power, bool attachShoot)
    {
        if(isYarnGunIncooldown)
        {
            yield break;
        }
        isYarnGunIncooldown=true;
        float depth = 0.2f;
        Vector3 dir = aimArrow.transform.forward;
        Quaternion aimArrowrRotation = aimArrow.transform.rotation;
        Vector3 startPos = transform.position;
        List<GameObject> yarns = new List<GameObject>();
        for (int i = 0; i < pieceCount; i++)
        {
            yarns.Add(Instantiate(yarnPiece, startPos + dir * depth * i, aimArrowrRotation));
            if (pieceCount < 50)
            {
                if (i % 2 == 0)
                {
                    yield return null;
                }
            }
            else if (i % 7 == 0)
            {
                yield return null;
            }
        }
        yield return null;
         if(pieceCount>0)
        {
            Joint playerJoint= GetComponent<Joint>();   
            if(playerJoint==null && attachShoot)
            {
                playerJoint = gameObject.AddComponent<FixedJoint>();
            }else if(!attachShoot)
            {
             Destroy(playerJoint);
             playerJoint=null;
            }
            if(playerJoint!=null)
            {
            playerJoint.connectedBody = attachShoot?yarns[0].GetComponent<Rigidbody>():null;
            }
        }
        if(!attachShoot)
        {
        rigidBody.AddForce(dir * power);
        }
        for (int i = 0; i < pieceCount-1; i++)
        {
            Rigidbody rigidbody = yarns[i+1].GetComponent<Rigidbody>();
           // rigidbody.isKinematic=true;
            yarns[i].GetComponent<Joint>().connectedBody= rigidbody;
        }
        yield return new WaitForSeconds(attachShoot?yarnGunCooldown*2f:yarnGunCooldown);
        isYarnGunIncooldown = false;
        if(yarnLifetime>yarnGunCooldown)
        {
        yield return new WaitForSeconds(yarnLifetime-yarnGunCooldown);
        }
        for (int i = 0; i < yarns.Count; i++)
        {
            Destroy(yarns[i]);
        }
        if(attachShoot && GetComponent<Joint>()!=null)
        {
            Destroy(GetComponent<Joint>());
        }
    }

void OnCollisionEnter(Collision collision)
    {     
        if(collision.collider.gameObject.layer == obstacleLayer)
        {
          GameOver();
        }
    }

private void GameOver()
{
    Cursor.visible=true;
SceneManager.LoadScene("GameOver");
}

}
