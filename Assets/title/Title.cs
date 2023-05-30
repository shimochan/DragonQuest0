using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Title : MonoBehaviour
{
    private bool firstPush = false;//一回だけ
    private bool dragon= false;//ドラゴンが出ている
    Animator animator;

    private void Start()
    {
        Invoke("Dragon",11.2f);//１１秒後にドラゴンが出てくる
         animator = GetComponent<Animator>();
    }

    void Update()
    { 
        if (Input.GetKey(KeyCode.Return))
        {
            Invoke("dragontrue",0.1f);
        }
        
        if (Input.GetKey(KeyCode.Return) && dragon)
        {
            if (!firstPush)
            {
                firstPush = true;
                SceneManager.LoadScene("stageSelect");
            }
        }
    }

    void Dragon()
    {
        animator.SetTrigger("start");
        dragon = true;
    }

    void dragontrue()
    {
        dragon = true;
        animator.SetTrigger("start");

    }
}
