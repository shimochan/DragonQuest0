using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int Enemyhp; //体力

    public int Enemyat; //攻撃力

    public float speed; //速さ

    public float gravity; //重力

    private bool isattack = false; //攻撃判定

    private bool isGround = true; //前に地面があるか

    private bool isWall = false; //前に壁があるか

    private bool isEnemy = false; //前に敵がいるか

    private bool isPlayer = false; //前に勇者がいるか

    private bool isDie = false; //死亡判定

    private bool isDamage = false; //ダメージ判定

    private bool isrealyGround = false; //今地面についているか

    private float x = -1; //進行方向

    Animator animator;

    private GameObject player;

    private SpriteRenderer sr;

    private Rigidbody2D rb;

    public GroundCheck ground;

    public PlayerCheck PlayerCheck;

    public AudioClip heroattackSE;

    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameObject.Find("hero");
        sr = GetComponent<SpriteRenderer>();
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (sr.isVisible)//画面内に入ったら動く
        {
            if (
                !isGround && !isattack || isEnemy //向き変え移動
            )
            {
                isGround = true;

                isEnemy = false;

                if (this.transform.eulerAngles.y == 180)
                {
                    x = 1;
                    this.transform.rotation = Quaternion.Euler(0, 0, 0);
                }
                else
                {
                    x = -1;
                    this.transform.rotation = Quaternion.Euler(0, 180, 0);
                }
            }

            rb.velocity = new Vector2(x * speed, gravity);

            //攻撃
            if (isPlayer)
            {
                isPlayer = false;
                if (!isattack)
                {
                    StartCoroutine("attack");
                }
            }

            if (isDamage)//ダメージの時は動かない
            {
                rb.velocity = new Vector2(0, gravity);
            }
            else
            {
                rb.velocity = new Vector2(x * speed, gravity);
            }


            if (isattack || isDie)//攻撃中、死亡の時は動かない
            {
                rb.velocity = new Vector2(0, 0);
            }

            isrealyGround = ground.IsGround();
            isPlayer = PlayerCheck.IsPlayerCheck();

            if (!isrealyGround)//地面がないと落ちる
            {
                rb.velocity = new Vector2(0, gravity);
            }
        }
        else
        {
            rb.Sleep();
        }
    }

    public void EnemyDamage(int damage)//ダメージ
    {
        isDamage = true;
        Enemyhp -= damage;
        var pdir = player.transform.forward;
        var edir = transform.forward;
        GManager.instance.PlaySE(heroattackSE); 

        if (pdir != edir)//向いている向きが違うなら
        {
            if (transform.localScale.x >= 0)
            {
                transform.Translate(Vector3.left * 0.25f);
            }
            else
            {
                transform.Translate(Vector3.right * 0.25f);
            }
        }
        animator.SetTrigger("IsHurt");
        if (Enemyhp <= 0)
        {
            Die();
        }
    }

    public void Damagefalse()//ダメージ判定をfalseに。アニメーションにつけてある
    {
        isDamage = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.GetComponent<Hero>())
        {
            collision.gameObject.GetComponent<Hero>().HeroDamage(Enemyat);
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGround = false;
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            isGround = true;
        }
        if (collision.gameObject.tag == "Enemy")
        {
            isEnemy = true;
        }
    }


    void Die()
    {
        Enemyhp = 0;
        animator.SetTrigger("Die");
        isDie = true;
    }

    public void diedestroy()//消滅
    {
        Destroy (gameObject);
    }

    IEnumerator attack()//攻撃待ち
    {
        isattack = true;
        animator.SetTrigger("attack");
        yield return new WaitForSeconds(2.0f);
        isattack = false;
    }
}
