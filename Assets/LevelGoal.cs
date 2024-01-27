using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGoal : MonoBehaviour
{


void OnTriggerEnter()
{
    GetComponent<LevelManager>().PlayNextLevel();
}

}
