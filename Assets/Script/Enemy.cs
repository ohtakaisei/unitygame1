using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    public bool isMoving = false;

    private BoxCollider2D boxCollider2;
    public LayerMask blockingLayer;

    [System.NonSerialized] public int attackDamage = 10;

    private Animator animator;

    private Transform target;
    private bool skipMove = false;

    public AudioClip enemyAttack1;
    public AudioClip enemyAttack2;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.instance.AddEnemy(this);

        boxCollider2 = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();

        target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveEnemy()
    {
        if(!skipMove)
        {
            skipMove = true;
            int xDir = 0;
            int yDir = 0;

            if(Mathf.Abs(target.position.x - transform.position.x) < float.Epsilon)
            {
                yDir = target.position.y > transform.position.y ? 1 : -1;
            }
            else
            {
                xDir = target.position.x > transform.position.x ? 1 : -1;
            }

            ATMove(xDir,yDir);
        }
        else
        {
            skipMove = false;
        }
    }

    public void ATMove(int x, int y)
    {
        RaycastHit2D hit;

        bool canMove = Move(x, y, out hit);

        if(hit.transform == null)
        {
            return;
        }

        Player hitComponent = hit.transform.GetComponent<Player>();

        if(!canMove && hitComponent != null)
        {
            //攻撃用の関数
            OnCantMove(hitComponent);
        }
    }

    public bool Move(int x, int y,out RaycastHit2D hit)
    {
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(x, y);

        boxCollider2.enabled = false;

        hit = Physics2D.Linecast(start, end, blockingLayer);

        boxCollider2.enabled = true;

        if (!isMoving && hit.transform == null)
        {
            //移動用
            StartCoroutine(Movement(end));

            return true;
        }

        return false;
    }

    IEnumerator Movement(Vector3 end)
    {
        isMoving = true;

        float remainingDistance = (transform.position - end).sqrMagnitude;

        while(remainingDistance > float.Epsilon)
        {
            transform.position = Vector3.MoveTowards(this.gameObject.transform.position, end, 10f * Time.deltaTime);

            remainingDistance = (transform.position - end).sqrMagnitude;

            yield return null;
        }
        transform.position = end;

        isMoving = false;

    }

    void OnCantMove(Player hit)
    {
        attackDamage = Random.Range(1, 10);
        hit.EnemyAttack(attackDamage);
        animator.SetTrigger("Attack");

        SoundManager.instance.RandomSE(enemyAttack1, enemyAttack2);
    }

}
