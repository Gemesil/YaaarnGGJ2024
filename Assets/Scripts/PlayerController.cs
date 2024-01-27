using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float aimSpeed = 5f;
    private Player player;
    private float currentAxisX=0;
    private float currentAxisY=0;
    void Start()
    {
        player = GetComponent<Player>();
    }
    void FixedUpdate()
    {
      player.Move(currentAxisX,currentAxisY);
    }
    void Update()
    {
        currentAxisX = Input.GetAxis("Horizontal");
        currentAxisY = Input.GetAxis("Vertical");
        if (Input.GetMouseButton(0))
        {
            player.RotateAimArrow(-Input.GetAxis("Mouse Y") * aimSpeed, Input.GetAxis("Mouse X") * aimSpeed);
        }
        if (Input.GetMouseButtonUp(0))
        {
            player.ReleaseYarn(false);
        }
        if (Input.GetMouseButtonDown(1))
        {
             player.ReleaseYarn(true);
        }else if(Input.GetMouseButtonUp(1))
        {
           Joint joint = player.gameObject.GetComponent<Joint>();
           if(joint!=null)
           {
            Destroy(joint);
           }
        }
    }
}
