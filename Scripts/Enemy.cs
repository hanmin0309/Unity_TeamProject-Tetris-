using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{

    public string type;
    public float speed;
    public float health;
    public float maxHealth;

    public RuntimeAnimatorController[] animCon;
    public Slider hpSlider;
    public Text hpText;

    public Rigidbody2D target;
    public Rigidbody2D bossEndPoint;
    bool isLive;

    Rigidbody2D rigid;
    Collider2D coll;
    Animator anim;
    SpriteRenderer spriter;
    WaitForFixedUpdate wait;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        anim = GetComponent<Animator>();
        spriter = GetComponent<SpriteRenderer>();
        wait = new WaitForFixedUpdate();
    }

    private void OnEnable()
    {
        bossEndPoint = GameManager.gm.bossEndPoint.GetComponent<Rigidbody2D>();
        target = GameManager.gm.player.GetComponent<Rigidbody2D>();
        isLive = true;
        coll.enabled = true;
        rigid.simulated = true;
        spriter.sortingOrder = 2;
        anim.SetBool("Dead", false);
        health = maxHealth;
    }


    // Start is called before the first frame update
    void Start()
    {

    }
    void FixedUpdate()
    {
        //if (!GameManager.instance.isLive)
        //return;

        if (!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
            return;

        if (type == "Boss")
        {
            float stoppingDistance = 0.1f;
            Vector2 dirVecEnd = bossEndPoint.position - rigid.position;

            if (dirVecEnd.magnitude > stoppingDistance)
            {
                Vector2 nextVecEnd = dirVecEnd.normalized * speed * Time.fixedDeltaTime;
                rigid.MovePosition(rigid.position + nextVecEnd);
                rigid.velocity = Vector2.zero;
            }

        }
        else
        {
            Vector2 dirVec = target.position - rigid.position;
            Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
            rigid.MovePosition(rigid.position + nextVec);
            rigid.velocity = Vector2.zero;
        }
    }



    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") || !isLive)
        {
            return;
        }

        Debug.Log("충돌");
        //gameObject.SetActive(false);
        StartCoroutine(KnockBack());
        health -= 3;
        Debug.Log("체력 : " + health);
        if (health > 0)
        {
            anim.SetTrigger("Hit");
        }
        else
        {
            isLive = false;
            coll.enabled = false;
            rigid.simulated = false;
            spriter.sortingOrder = 1;
            anim.SetBool("Dead", true);
            GameManager.gm.kill++;
            //Dead();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Debug.Log("TriggerStay");

        if (!collision.CompareTag("Player") || !isLive)
        {
            return;
        }

        Debug.Log("충돌");
        //gameObject.SetActive(false);
        StartCoroutine(KnockBack());
        health -= 3;
        Debug.Log("체력 : " + health);
        if (health > 0)
        {
            anim.SetTrigger("Hit");
        }
        else
        {
            isLive = false;
            coll.enabled = false;
            rigid.simulated = false;
            spriter.sortingOrder = 1;
            anim.SetBool("Dead", true);
            GameManager.gm.kill++;

            //Dead();
        }
    }

    IEnumerator KnockBack()
    {
        Debug.Log("넉백 실행");
        yield return wait;
        Vector3 playerPos = GameManager.gm.player.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        rigid.AddForce(dirVec.normalized * 6 , ForceMode2D.Impulse);

    }

    public void Dead()
    {
        if(type == "Boss")
        {
            GameManager.gm.bossTime = false;
        }
        gameObject.SetActive(false);
    }
    public void Init(SpawnData data)
    {
        anim.runtimeAnimatorController = animCon[data.spriteType];
        speed = data.speed;
        maxHealth = data.health;
        health = data.health;
    }
}
