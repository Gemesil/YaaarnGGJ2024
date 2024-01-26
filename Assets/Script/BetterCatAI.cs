using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BetterCatAI : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] Transform player;
    [SerializeField] float jumpTime;
    [SerializeField] float jumpForceY;

    float debugTimer = 10;
    float debugTimer2 = 10;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            debugTimer = 0;
            debugTimer2 = 0;
            rb.velocity = CalculateJumpSpeed();
        }
        if (debugTimer <= jumpTime)
        {
            debugTimer += Time.deltaTime;
        }
        if(debugTimer != debugTimer2 && debugTimer > jumpTime)
        {
            Debug.Log(transform.position.x);
        }
        debugTimer2 = debugTimer;
        
    }

    private Vector3 CalculateJumpSpeed()
    {
        /*
        float x = player.position.x - transform.position.x;
        x = x / jumpTime;
        float z = player.position.z - transform.position.z;
        z = z / jumpTime;

        float y = -Physics.gravity.y * jumpTime / 2;
        */
        float y = jumpForceY;
        
        //calculate the jump time (insert formula for ofek)
        float time = 2 * jumpForceY / Physics.gravity.y;

        //calculate horizontal speed
        float x = player.position.x - transform.position.x;
        x = x / time;
        float z = player.position.z - transform.position.z;
        z = z / time;
        return new Vector3 (x, y, z);
    }
}
