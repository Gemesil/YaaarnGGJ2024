using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIMediator : MonoBehaviour
{
    public static UIMediator current;
      public GameObject lifebar;

    public GameObject lifePoint;


    void Start()
    {
        current = this;
    }
    private void OnDestroy()
    {
        current = null;
    }

    public void SetLifePoint(int lifePoints)
    {
foreach(Transform child in lifebar.transform)
{
    Destroy(child.gameObject);
}
   for(int i=0;i<lifePoints;i++)
   {
       Instantiate(lifePoint,lifebar.transform);
   }
    }


}