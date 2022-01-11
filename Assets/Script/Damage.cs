using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{

    public int wallHp = 3;
    public Sprite dmgWall;

    private SpriteRenderer spriteRenderer;

    public int enemyHP = 5;
    private Enemy enemy;

    public AudioClip chopSound1;
    public AudioClip chopSound2;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        enemy = GetComponent<Enemy>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AttackDamage(int loss)
    {
        SoundManager.instance.RandomSE(chopSound1, chopSound2);
        if (gameObject.CompareTag("Wall"))//タグで判定
        {
            spriteRenderer.sprite = dmgWall;

            wallHp -= loss;

            if (wallHp <= 0)
            {
                gameObject.SetActive(false);
            }
        }
        else if(gameObject.CompareTag("Enemy"))//タグで判定
        {
            enemyHP -= loss;//enemyのhpをplayerの攻撃力分引く

            if (enemyHP <= 0)//残りhpが0以下になったら
            {  
                GameManager.instance.DestroyEnemyToList(enemy);//関数を呼ぶ（リストから削除）
                gameObject.SetActive(false);//ゲームオブジェクトを非表示にする
            }
        }

    }
}
