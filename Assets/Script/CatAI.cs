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
        if(Input.GetKeyDown(KeyCode.Space))
        {
            rb.velocity = CalculateJump();
        }
        Gravitate();
    }
    private Vector3 CalculateJump()
    {

        float x = (player.position.x - transform.position.x)/jumpTime;
        float z = (player.position.z - transform.position.z)/jumpTime;
        float y = 2 *(jumpHeight/(jumpTime * jumpTime));

        return new Vector3(x, y, z);
    }

    private void Gravitate()
    {
        Vector3 gravityVector = gravity * Vector3.up;
        rb.AddForce(gravityVector, ForceMode.Acceleration);
    }
}
