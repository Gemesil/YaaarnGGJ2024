using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseGravityAfterTime : MonoBehaviour
{
     public float dealyTime=0;

    void Start()
    {
       StartCoroutine(EnableGravityAfterTime(dealyTime));
    }

 
    private IEnumerator EnableGravityAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        GetComponent<Rigidbody>().useGravity=true;
    }

}
