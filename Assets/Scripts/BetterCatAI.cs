using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterCatAI : MonoBehaviour
{
    Rigidbody rb;
    private LevelManager levelManager;

    [SerializeField] private Transform player;
    [SerializeField] private float dashSpeed;
    [Tooltip("The string name of the traps which is layer 11.")]
    [SerializeField] private string trapLayer;
    [SerializeField] private int hp;

    [SerializeField] private float jumpForceY;
    [Tooltip("The distance from which the roomba will jump instead of dash.")]
    [SerializeField] private float jumpDistance;

    private Vector3 playerLastPosition;

    private bool fall = false;
    private bool jump = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        fall = false;
        levelManager = GetComponent<LevelManager>();
        Dash();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        AI();
    }

    private void AI()
    {
        TurnToPlayer();
        if (!jump && !fall)
        {
            if (DistanceToPlayerHorizontal() <= jumpDistance)
                Jump();
            else
                Dash();
        }
        
        JumpBools();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (LayerMask.LayerToName(collision.gameObject.layer) == trapLayer)
        {
            hp--;
            if (hp <= 0)
                KillRoomba();
            Destroy(collision.transform.parent.gameObject);
        }
    }

    private void KillRoomba()
    {
        Destroy(gameObject);
        levelManager.PlayVictory();
    }

    private void TurnToPlayer()
    {
        TurnTo(player.position);

    }

    private void TurnTo(Vector3 target)
    {
        //Get direction vector
        float x = target.x - transform.position.x;
        float z = target.z - transform.position.z;
        float angle = -Mathf.Atan2(-x, z);
        angle = angle * Mathf.Rad2Deg;
        Quaternion q = new Quaternion();
        q.eulerAngles = new Vector3(0, angle, 0);
        rb.transform.rotation = q;

    }

    private void Dash()
    {
        //Debug.Log("Changed " + playerLastPosition + " to " + player.position + "\nVelocity is " + rb.velocity);
        Vector3 playerPos= new Vector3(player.position.x,player.position.y,0);
        Vector3 AIPos = new Vector3(transform.position.x,transform.position.y,0);
        if((playerPos-AIPos).magnitude>15f)
        {
        TurnToPlayer();
        }
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

    private void JumpBools()
    {
        if (rb.velocity.y <= 0 && !fall && jump)
        {
            jump = false;
            fall = true;
        }
        else if (rb.velocity.y >= 0 && fall && !jump)
        {
            fall = false;
        }
    }

    private float DistanceToPlayerHorizontal()
    {
        Vector3 disVector = player.position - transform.position;
        disVector.y = 0;
        float distance = disVector.magnitude;
        return distance;
    }
}
