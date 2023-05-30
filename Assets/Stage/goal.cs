using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class goal : MonoBehaviour
{
    [Header("フェード")]
    public FadeImage fade;
     public AudioClip stairsSE;

    void Update()
    {
        if (fade.IsFadeOutComplete())
        {
            SceneManager.LoadScene("stage1boss");
        }

    }

     private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
             fade.StartFadeOut();
             GManager.instance.PlaySE (stairsSE);
        }
    }

    
    
}
