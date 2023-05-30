using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class clear : MonoBehaviour
{
    private GameObject[] enemyObjects;
	public GameObject command;

	 public AudioClip gameclearSE;


	private bool firstPush = false;//一回だけ

	 public Text end;

	 public bool isclear = false;

	    public FadeImage fade;

	void Start()
	{
		command.SetActive(false);
	}

	void Update () {

		// Enemyというタグが付いているオブジェクトのデータを箱の中に入れる。
		enemyObjects = GameObject.FindGameObjectsWithTag("Enemy");

		// データの入った箱のデータが０に等しくなった時（Enemyというタグが付いているオブジェクトが全滅したとき）
		if(enemyObjects.Length == 0 && !isclear)
		{
            isclear = true;
			command.SetActive(true);
			GManager.instance.PlaySE (gameclearSE);
        }
		if(Input.GetKey(KeyCode.Return))
		{
          end.text = string.Format("魔物を　やっつけた！");
		  Invoke("firstpush",0.1f);
		  
		}
		if(Input.GetKey(KeyCode.Return) && firstPush)
		{
			command.SetActive(false);
			fade.StartFadeOut();
		     Invoke("load",1f);
		}
    }
	void load()
	{
		SceneManager.LoadScene("stageselect");
	}

	void firstpush()
	{
		firstPush = true;//一回だけ
	}
}
