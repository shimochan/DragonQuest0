using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doraki : MonoBehaviour
{
    public AudioClip magicSE;

    Animator animator;

    public float timeleft;

    private Rigidbody2D rb;

    private GameObject player;

    //弾のプレハブオブジェクト
    public GameObject mera;

    public Transform ShotPosition;

    float meraspeed = 1.5f; //速さ
    float y;


    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.Find("hero");
        rb = GetComponent<Rigidbody2D>();
         y = transform.position.y;
    }

    void Update()
    {
        transform.position =
            new Vector2(transform.position.x,
                Mathf.Sin(Time.time) * 2 + y);
        Vector2 ppos = player.transform.position;
        Vector2 mpos = this.transform.position;
        if (ppos.x < mpos.x)
        {
            this.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            this.transform.rotation = Quaternion.Euler(0, 0, 0);
        }

        timeleft -= Time.deltaTime;
        if (timeleft <= 0.0)
        {
            timeleft = 5f;
            GManager.instance.PlaySE (magicSE);
            animator.SetTrigger("magic");
        }
    }

    public void Mera() //メラプレファブ
    {
        var pos = ShotPosition.position;

        //弾のプレハブを作成
        var t = Instantiate(mera, ShotPosition.position, transform.rotation);

        //弾のプレハブの位置を敵の位置にする
        t.transform.position = pos;

        //敵からプレイヤーに向かうベクトルをつくる
        //プレイヤーの位置から敵の位置（弾の位置）を引く
        Vector2 vec = player.transform.position - pos;

        //弾のRigidBody2Dコンポネントのvelocityに先程求めたベクトルを入れて力を加える
        t.GetComponent<Rigidbody2D>().velocity = vec * meraspeed;
    }
}
