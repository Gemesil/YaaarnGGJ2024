using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterCatAI : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] private Transform player;
    [SerializeField] private float dashSpeed;

    [SerializeField] private float jumpForceY;
    [SerializeField] private float jumpCooldown;

    private Vector3 playerLastPosition;
    private float cooldownTimer = 1;

    private bool fall = false;
    private bool jump = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        fall = false;
        Dash();
    }

    // Update is called once per frame
    void Update()
    {
        AI();
    }

    private void AI()
    {
        
        
        if (PastPlayer())
        {
            Dash();
        }
        SetSpeed(); // idk why this roomba is slowing down making a physics material didnt work
    }

    private void TurnToPlayer()
    {
        //Get direction vector
        float x = player.position.x - transform.position.x;
        float z = player.position.z - transform.position.z;
        float angle = -Mathf.Atan2(-x, z);
        angle = angle * Mathf.Rad2Deg;
        Quaternion q = new Quaternion();
        q.eulerAngles = new Vector3(0, angle, 0);
        rb.transform.rotation = q;

    }

    private void Dash()
    {
        //Debug.Log("Changed " + playerLastPosition + " to " + player.position + "\nVelocity is " + rb.velocity);
        TurnToPlayer();
        playerLastPosition = player.position;
        SetSpeed();
    }

    private bool PastPlayer()
    {
        //start by understanding the direction of the dash, and then check if youre past the old player position (of when the roomba started its dash)
        Vector3 dirVector = new Vector3();
        dirVector = rb.velocity;

        //Debug.DrawLine(transform.position, playerLastPosition, Color.green);
        //Debug.DrawLine(transform.position, transform.position + dirVector, Color.blue);


        //now we can compare the current position against the old player position
        //I multiply the position by -1 if the direction is negative to check whever or not the cat is past that point
        bool pastX;
        bool pastZ;

        if (dirVector.x > 0)
        {
            pastX = transform.position.x >= playerLastPosition.x;
            //if (pastX) 
              //  Debug.Log(debugFrames + " X: " + pastX + " = " + transform.position.x + " >= " + playerLastPosition.x);
        }
        else
        {
            pastX = transform.position.x <= playerLastPosition.x;
            //if (pastX)
              //  Debug.Log(debugFrames + " X: " + pastX + " = " + transform.position.x + " <= " + playerLastPosition.x);
        }

        if (dirVector.z > 0)
        {
            pastZ = transform.position.z >= playerLastPosition.z;
            //if (pastZ)
              //  Debug.Log(debugFrames + " Z: " + pastZ + " = " + transform.position.z + " >= " + playerLastPosition.z);
        }
        else
        {
            pastZ = transform.position.z <= playerLastPosition.z;
            //if (pastZ)
              //  Debug.Log(debugFrames + " Z: " + pastZ + " = " + transform.position.z + " <= " + playerLastPosition.z);
        }

        /*
        if(pastX && pastZ)
        {
            Debug.Log("frame: " + debugFrames + " velocity: " + rb.velocity);
            Debug.DrawLine(rb.position, player.position, Color.red);
            //Debug.Break();
        }
        */
        return pastX && pastZ;
    }

    private void SetSpeed()
    {
        Vector3 dirVector = new Vector3();
        dirVector = playerLastPosition - transform.position;

        //normelize horizontal speed
        dirVector.y = 0;
        dirVector.Normalize();

        //return the fall speed if needed
        dirVector.y = rb.velocity.y; 
        rb.velocity = dirVector * dashSpeed;
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

    private void JumpRules()
    {
        if (rb.velocity.y <= 0 && !fall && jump)
        {
            jump = false;
            fall = true;
        }
        else if (rb.velocity.y >= 0 && fall && !jump)
        {
            cooldownTimer = jumpCooldown;
            fall = false;
        }

        if (cooldownTimer >= 0)
        {
            //if the timer is running make it tick
            cooldownTimer -= Time.deltaTime;
            if (cooldownTimer <= 0)
            {
                //check if the tick reached the end of the timer
                Jump();
            }
        }
    }
}
