using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Hero : MonoBehaviour
{
    public float Movespeed; //基本の移動速度

    public float movespeedNow; //今の移動速度

    public float jumpSpeed; //ジャンプ力

    public float gravity; //重力

    public bool isJump = false; //ジャンプ判定

    public bool isGround = false; //着地判定

    public bool isSquat = false; //しゃがみ判定

    public float jumpPos = 0.0f;

    public float jumpHeight; //最大高

    public float ySpeed;

    private int m_currentAttack = 0;

    private float m_timeSinceAttack = 0.0f;

    public bool Dush = false; //　走っているかどうか

    public bool push = false; //　最初に移動ボタンを押したかどうか

    public float nextButtonDownTime = 0.2f; //　次に移動ボタンが押されるまでの時間

    private float nowTime = 0f; //　最初に移動ボタンが押されてからの経過時間

    private int m_facingDirection = 1; //向いている方向

    public float m_avoidForce; //回避スピード

    public bool m_avoiding = false; //回避判定

    public bool isRight = true; //右向いているかどうか

    public bool isDie = false;//死亡判定

    public int at; //攻撃力

    public bool IsDeadArea = false;//死亡エリア判定

    public float attackRadius;//攻撃範囲

    public LayerMask enemyLayer;

    public GameObject meraPrefab;

    public Transform AttackPosition;

    public Transform ShotPosition;

    public GroundCheck ground;

    public GroundCheck Head;

    public bool isHead = false;//頭判定

    public Text texthp;//hp表示

    public Text textmaxhp;//maxhp表示

    public Text textmp;//mp表示

    public Text textmaxmp;//maxmp表示

    Rigidbody2D rb;

    Animator animator;

    private bool isContinue = false;

    private float continueTime = 0.0f;

    private float blinkTime = 0.0f;

    private SpriteRenderer sr = null;

    public AudioClip damageSE;

    public AudioClip magicSE;

    public AudioClip gameoverSE;

      public AudioClip missSE;

    [Header("フェード")]
    public FadeImage fade;

    private bool goNextScene = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        GManager.instance.hp = GManager.instance.maxhp;
        GManager.instance.mp = GManager.instance.maxmp;
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        float movepowwer = Movespeed; //基本の速さ
        bool isReady1 =
            animator.GetCurrentAnimatorStateInfo(0).IsName("Attack1");
        bool isReady2 =
            animator.GetCurrentAnimatorStateInfo(0).IsName("Attack2");
        bool isReady3 =
            animator.GetCurrentAnimatorStateInfo(0).IsName("Attack3");
        bool isheroguard =
            animator.GetCurrentAnimatorStateInfo(0).IsName("heroguard");
        bool isSquat =
            animator.GetCurrentAnimatorStateInfo(0).IsName("herosquat");
        bool isDieanimation =
            animator.GetCurrentAnimatorStateInfo(0).IsName("herodie");

        //移動
        float x = Input.GetAxisRaw("Horizontal");

        if(isDie)
        {
            x = 0;
        }



        if (x > 0 && !isRight && !isDieanimation)
        {
            transform.Rotate(0f, 180f, 0);
            m_facingDirection = 1;
            isRight = true;
        }
        if (x < 0 && isRight && !isDieanimation)
        {
            transform.Rotate(0f, 180f, 0);
            m_facingDirection = -1;
            isRight = false;
        }
        animator.SetFloat("speed", Mathf.Abs(x));

        //　走っていない時
        if (!Dush &&!isDie)
        {
            //　移動キーを押した
            if (Input.GetButtonDown("Horizontal"))
            {
                //　最初に1回押していない時は押した事にする
                if (!push)
                {
                    push = true;

                    nowTime = 0f;
                    //　2回目のボタンだったら1→２までの制限時間内だったら走る
                }
                else
                {
                    if (nowTime <= nextButtonDownTime)
                    {
                        Dush = true;
                    }
                }
            }
            //　走っている時にキーを押すのをやめたら走るのをやめる
        }
        else
        {
            if (!Input.GetButton("Horizontal"))
            {
                Dush = false;
                push = false;
            }
        }

        //　最初の移動キーを押していれば時間計測
        if (push)
        {
            //　時間計測
            nowTime += Time.deltaTime;

            if (nowTime > nextButtonDownTime)
            {
                push = false;
            }
        }

        if (
            Dush //ダッシュ
        )
        {
            movespeedNow = movepowwer * 1.5f;
        }

        if (Dush && isReady1 | isReady2 | isReady3 | isheroguard | isSquat)
        {
            movespeedNow = 0;
        }

        //ジャンプ
        float ySpeed = -gravity;
        isGround = ground.IsGround(); //着地判定
        isHead = Head.IsGround();

        float verticalKey = Input.GetAxis("Jump");
        if (isGround && Input.GetKeyDown(KeyCode.Space) &&!isDie)
        {
            if (verticalKey > 0 && !isDie)
            {
                ySpeed = jumpSpeed;
                jumpPos = transform.position.y; //ジャンプした位置を記録する
                isJump = true;
            }
            else
            {
                isJump = false;
            }
        }
        else if (isJump &&!isDie)
        {
            //上ボタンを押されている。かつ、現在の高さがジャンプした位置から自分の決めた位置より下ならジャンプを継続する
            if (
                verticalKey > 0 &&
                jumpPos + jumpHeight > transform.position.y &&
                !isHead && !isDie
            )
            {
                ySpeed = jumpSpeed;
            }
            else
            {
                isJump = false;
            }
        }
        animator.SetBool("jump", isJump);
        animator.SetBool("ground", isGround);

        rb.velocity = new Vector2(x * movespeedNow, ySpeed); //移動

        if(isGround && isDie)
        {
            rb.velocity = new Vector2(0,0); //移動
        }

        //しゃがみ
        isSquat = Input.GetKey("s");
        animator.SetBool("squat", isSquat);

        //攻撃
        m_timeSinceAttack += Time.deltaTime;

        if (
            Input.GetKeyDown("j") &&
            m_timeSinceAttack > 0.25f &&
            isGround &&
            !isDie
        )
        {
            m_currentAttack++;

            // Loop back to one after third attack
            if (m_currentAttack > 3) m_currentAttack = 1;

            // Reset Attack combo if time since last attack is too large
            if (m_timeSinceAttack > 0.5f) m_currentAttack = 1;

            // Call one of three attack animations "Attack1", "Attack2", "Attack3"
            animator.SetTrigger("Attack" + m_currentAttack);

            // Reset timer
            m_timeSinceAttack = 0.0f;
        }

        if (isReady1 | isReady2 | isReady3 | isheroguard | isSquat)
        {
            movespeedNow = 0;
        }
        else
        {
            movespeedNow = movepowwer;
        }

        //空中攻撃
        if (Input.GetKeyDown("j") && !isGround)
        {
            animator.SetTrigger("heroattack4");
        }

        //回避
        if (Input.GetKeyDown("w") && !m_avoiding && !isDie)
        {
            m_avoiding = true;
            animator.SetTrigger("avoid");
        }
        else
        {
            m_avoiding = false;
        }
        bool isavoid =
            animator.GetCurrentAnimatorStateInfo(0).IsName("heroavoid");
        if (isavoid)
        {
            rb.velocity = new Vector2(m_facingDirection * m_avoidForce, ySpeed); //移動
        }

        //ガード
        if (Input.GetKeyDown("l"))
        {
            animator.SetTrigger("guard 0");
            animator.SetBool("guard", true);
        }
        if (Input.GetKeyUp("l"))
        {
            animator.SetBool("guard", false);
        }

        //メラ
        if (Input.GetKeyDown("k") && GManager.instance.mp > 0 && !isDie)
        {
            GManager.instance.PlaySE (magicSE);
            GManager.instance.mp -= 2;
            animator.SetTrigger("mera");
        }
        
        //hp表示
        texthp.text = GManager.instance.hp.ToString();
        textmaxhp.text = GManager.instance.maxhp.ToString();
        textmp.text = GManager.instance.mp.ToString();
        textmaxmp.text = GManager.instance.maxmp.ToString();

        if (isContinue)//落下ちかちか
        {
            //明滅　ついている時に戻る
            if (blinkTime > 0.2f)
            {
                sr.enabled = true;
                blinkTime = 0.0f;
            }
            else //明滅　消えているとき
            if (blinkTime > 0.1f)
            {
                sr.enabled = false;
            }
            else
            //明滅　ついているとき
            {
                sr.enabled = true;
            }

            //1秒たったら明滅終わり
            if (continueTime > 1.0f)
            {
                isContinue = false;
                blinkTime = 0f;
                continueTime = 0f;
                sr.enabled = true;
            }
            else
            {
                blinkTime += Time.deltaTime;
                continueTime += Time.deltaTime;
            }
        }

        if (!goNextScene && fade.IsFadeOutComplete())
        {
            SceneManager.LoadScene("stageSelect");
            goNextScene = true;
        }

        
        
        
    }

    public void Mera() //メラプレファブ
    {
        Instantiate(meraPrefab, ShotPosition.position, transform.rotation);
    }

    public void Attack() //アタックプレファブ
    {
        Collider2D[] hitEnemys =
            Physics2D
                .OverlapCircleAll(AttackPosition.position,
                attackRadius,
                enemyLayer);
        foreach (Collider2D hitEnemy in hitEnemys)
        {
            hitEnemy.GetComponent<Enemy>().EnemyDamage(at);
           
        }
    }

    private void OnDrawGizmosSelected() //攻撃範囲
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(AttackPosition.position, attackRadius);
    }

    

    public void HeroDamage(int damage) //敵にぶつかったらダメージ
    {
    bool isheroguard =
            animator.GetCurrentAnimatorStateInfo(0).IsName("heroguard");
        if(isheroguard)
        {
        GManager.instance.hp -= damage / 2;
        int d = damage/2;
         if(d <= 0)
         {
            GManager.instance.PlaySE (missSE);
         }
         else
         {
             GManager.instance.PlaySE (damageSE);
         }
        }
        else
        {
        GManager.instance.hp -= damage;
        GManager.instance.PlaySE (damageSE);
        animator.SetTrigger("hurt");
        }
        

        if (GManager.instance.hp <= 0)
        {
            Die();
        }
    }

    void Die() //死亡
    {
        GManager.instance.hp = 0;
        animator.SetTrigger("die");
        isDie = true;
        GManager.instance.PlaySE (gameoverSE);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "DeadArea")
        {
            IsDeadArea = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "DeadArea")
        {
            GManager.instance.PlaySE (damageSE);
            IsDeadArea = false;
            isContinue = true;
        }
    }

    public void GameOver()
    {
        fade.StartFadeOut();
    }
}
