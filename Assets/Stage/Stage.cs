using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stage : MonoBehaviour
{
     public GameObject playerObj;
     public GameObject[] continuePoint;
     public Hero H;

     void Start()
     {
          playerObj.transform.position = continuePoint[0].transform.position;
          
        
     }
     void Update()
     {
         bool Isdeadarea;
          Isdeadarea = H.IsDeadArea;
         if(Isdeadarea)
         {
         playerObj.transform.position = continuePoint[GManager.instance.continueNum].transform.position;
         
         }
     }

}
