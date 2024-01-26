using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float aimSpeed=5f;
    private Player player; 
 
    void Start()
    {
         player = GetComponent<Player>();
    }
    // Start is called before the first frame update
    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButton(0))
        {
        player.RotateAimArrow(-Input.GetAxis("Mouse Y")*aimSpeed,Input.GetAxis("Mouse X")*aimSpeed);
        }
        if (Input.GetMouseButtonUp(0))
        {
            player.ReleaseYarn();
        }
    }
}
