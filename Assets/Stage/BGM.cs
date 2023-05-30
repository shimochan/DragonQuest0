using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGM : MonoBehaviour
{
    public Hero hero;
    AudioSource audioSource;
    public bool isDie;

    public clear clear;

    public bool isclear;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        Invoke("audiostart",1.7f);
    }

    // Update is called once per frame
    void Update()
    {
        isclear = clear.isclear;
        isDie = hero.isDie;
        if(isDie | isclear)
        {
            audioSource.Stop();
            
        }
    }
    void audiostart()
    {
        audioSource.Play();
    }


}
