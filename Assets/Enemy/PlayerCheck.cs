using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCheck : MonoBehaviour
{
    private bool isPlayer = false; 
    private string groundTag = "Player";
    private bool isGroundEnter, isGroundStay, isGroundExit; 
    public bool IsPlayerCheck()
{
   if(isGroundEnter || isGroundStay)
   {
      isPlayer = true;
   }
   else if(isGroundExit)
   {
      isPlayer = false;
   } 

   isGroundEnter = false;
   isGroundStay = false;
   isGroundExit = false;
   return isPlayer; 
}
 
private void OnTriggerEnter2D(Collider2D collision)
{
   if (collision.tag == groundTag)
   {
      isGroundEnter = true;
   }
}
 
private void OnTriggerStay2D(Collider2D collision)
{
   if (collision.tag == groundTag)
   {
      isGroundStay = true;
   }
}
     
private void OnTriggerExit2D(Collider2D collision)
{
   if (collision.tag == groundTag)
   {
      isGroundExit = true;
   }
}
}