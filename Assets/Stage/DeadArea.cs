using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeadArea : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)	
    {
        if(collision.tag == "Ground" && GManager.instance.hp != 1)
       {
         GManager.instance.hp /= 2;
        
       }
    }
}
