using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GManager : MonoBehaviour
{
    public static GManager instance = null;
    public int maxhp; //最大体力　　
    public int hp; //体力　　
    public int maxmp;//最大ｍｐ
    public int mp;//ｍｐ
    public int continueNum;
    private AudioSource audioSource = null; 

    private void Start()
{
    audioSource = GetComponent<AudioSource>();
}

   private void Awake()
   {
       
       if(instance == null)
       {
           instance = this;
           DontDestroyOnLoad(this.gameObject);
       }
       else
       {

            Destroy(this.gameObject);    
           
       }
   }
   public void PlaySE(AudioClip clip)
{
     
         audioSource.PlayOneShot(clip);
     
     
}
}
