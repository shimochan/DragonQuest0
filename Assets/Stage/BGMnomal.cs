using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMnomal : MonoBehaviour
{
    public Hero hero;
    AudioSource audioSource;
    public bool isDie;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
       
    }

    // Update is called once per frame
    void Update()
    {
        isDie = hero.isDie;
        if(isDie)
        {
            audioSource.Stop();
            
        }
    }
}
