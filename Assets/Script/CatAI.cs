using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatAI : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private Transform player;
    [SerializeField] private float jumpTime;
    [SerializeField] private float jumpHeight;
    private float gravity;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        gravity = Physics.gravity.y;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyUp(KeyCode.Space))
        {
            rb.velocity = CalculateJump();
        }
        //Gravitate();
    }
    private Vector3 CalculateJump()
    {
        // 
        float x = (player.position.x - transform.position.x) / jumpTime;
        float z = (player.position.z - transform.position.z) / jumpTime;

        // calc time in midair
        //gravity = (-8 * jumpHeight) / (float)Math.Pow(jumpTime, 2); //its a formula
        //float y = 2 * jumpHeight - gravity * (jumpTime * jumpTime / 2);
        float y = jumpTime * (-gravity);

        return new Vector3(x, y, z);
    }

    private void Gravitate()
    {
        Vector3 gravityVector = gravity * Vector3.up;
        rb.AddForce(gravityVector, ForceMode.Acceleration);
    }
}
