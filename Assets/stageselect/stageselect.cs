using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class stageselect : MonoBehaviour
{
    [Header("フェード")] public FadeImage fade;
     private bool goNextScene = false;
    private bool firstPush = false;//一回だけ

    public void PressStart()
    {
        if (!firstPush)
            {
                firstPush = true;
                fade.StartFadeOut();
            }
    }
    private void Update()
     {
          if (!goNextScene && fade.IsFadeOutComplete())
          {
               SceneManager.LoadScene("stage1");
               goNextScene = true;
          }
    }
}
