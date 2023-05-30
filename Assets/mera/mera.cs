using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mera : MonoBehaviour
{
    public GameObject ImpactPrefab;
    public float leftTime;//消滅時間
    public int mt;  //メラ攻撃力
    float speed = 15f;//速さ
    Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * speed;
        Destroy(gameObject,leftTime);//時間になったら消す
        
    }

private void OnCollisionEnter2D(Collision2D collision)//衝突したら破裂
{

    Instantiate(ImpactPrefab,transform.position,transform.rotation);
    Destroy(gameObject);
    
        if(collision.gameObject.GetComponent<Enemy>())
        {

            collision.gameObject.GetComponent<Enemy>().EnemyDamage(mt);

        }
    
     
}

}
