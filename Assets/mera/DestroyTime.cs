using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTime : MonoBehaviour
{
   private float leftTime = 0.3f;
    void Start()
    {
          Destroy(gameObject,leftTime);
    }
}
