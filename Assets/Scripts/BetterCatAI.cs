using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterCatAI : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] private Transform player;
    [SerializeField] private float jumpForceY;
    [SerializeField] private float jumpCooldown;

    private float cooldownTimer = 1;

    private bool fall;
    private bool jump;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        fall = false;
    }

    // Update is called once per frame
    void Update()
    {
        AI();
    }

    private void AI()
    {
        if(rb.velocity.y <= 0 && !fall && jump)
        {
            jump = false;
            fall = true;
        }
        else if (rb.velocity.y >= 0 && fall && !jump) 
        {
            cooldownTimer = jumpCooldown;
            fall = false;
        }

        if(cooldownTimer >= 0)
        {
            //if the timer is running make it tick
            cooldownTimer -= Time.deltaTime;
            Debug.Log(cooldownTimer);
            if(cooldownTimer <= 0)
            {
                //check if the tick reached the end of the timer
                Jump();
            }
        }
    }

    private Vector3 CalculateJumpSpeed()
    {
        float y = jumpForceY;

        //calculate the jump time 
        /*
         * V0 + a*t = V  /Formula for constant acceleration
         * -V + a*t = V  /The arch starts and ends with the same speed just in the opposite direction
         * a*t = 2V
         * t = 2V/a
         */
        float time = 2 * jumpForceY / -Physics.gravity.y;

        //calculate horizontal speeds
        float x = player.position.x - transform.position.x;
        x = x / time;
        float z = player.position.z - transform.position.z;
        z = z / time;

        //Create and return the vector
        return new Vector3 (x, y, z);
    }

    private void Jump()
    {
        jump = true;
        rb.velocity = CalculateJumpSpeed();
    }
}
