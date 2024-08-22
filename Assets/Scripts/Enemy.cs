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
    public int damage;

    public RuntimeAnimatorController[] animCon;
    public Slider hpSlider;
    public Text hpText;

    public Rigidbody2D target;
    public Rigidbody2D bossEndPoint;
    public bool isLive;

    Rigidbody2D rigid;
    Collider2D coll;
    Animator anim;
    SpriteRenderer spriter;
    WaitForFixedUpdate wait;

    private enum State { Moving, Attacking }
    private State currentState;
    public bool isAttacking = false;

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
        currentState = State.Moving;
        //spriter.sortingOrder = 2;
        anim.SetBool("Dead", false);

        //Debug.Log("onenable ü�� " + health);

        health = maxHealth;
        hpText.text = health + "/" + maxHealth;
        hpSlider.value = 1;
    }


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("start ü�� " + health);
        health = maxHealth;
        hpText.text = health + "/" + maxHealth;
        hpSlider.value = 1;
    }
    void FixedUpdate()
    {

        if (!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit") || !GameManager.gm.gamePlay)
            return;

        if (type == "Boss" )
        {
            float stoppingDistance = 0.1f;
            Vector2 dirVecEnd = bossEndPoint.position - rigid.position;

            if (dirVecEnd.magnitude > stoppingDistance)
            {
                anim.SetTrigger("Run");
                currentState = State.Moving;
                Vector2 nextVecEnd = dirVecEnd.normalized * speed * Time.fixedDeltaTime;
                rigid.MovePosition(rigid.position + nextVecEnd);
                rigid.velocity = Vector2.zero;
            }
            else
            {
                anim.SetTrigger("Idle");
                currentState = State.Attacking;

                if (!isAttacking)
                {
                    StartCoroutine("AttackPlayer");
                }

            }
        }
        else
        {
            float stoppingDistance = 1.2f;
            Vector2 dirVec = target.position - rigid.position;
            //Debug.Log("�Ÿ� :"+dirVec.magnitude);
            if (dirVec.magnitude > stoppingDistance)
            {
                anim.SetTrigger("Run");
                Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;
                rigid.MovePosition(rigid.position + nextVec);
                rigid.velocity = Vector2.zero;
            }
            else
            {
                anim.SetTrigger("Idle");
            }
        }
    }
    private void Update()
    {
        if (health <= 0)
        {
            isLive = false;
            coll.enabled = false;
            rigid.simulated = false;
            spriter.sortingOrder = 1;
            StopCoroutine(AttackPlayer());
            isAttacking = false;

            anim.SetBool("Dead", true);
        }
    }




    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") || !isLive)
        {
            return;
        }

        if (!isAttacking && currentState == State.Moving)
        {
            Debug.Log("�ڷ�ƾ ����");
            currentState = State.Attacking;
            StartCoroutine("AttackPlayer");
        }

    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("�ڷ�ƾ �ߴ�");
            currentState = State.Moving;  // �÷��̾ ������ ����� �̵� ���·� ��ȯ
            StopCoroutine("AttackPlayer");  // �ڷ�ƾ ����
            isAttacking = false;
        }
    }

    IEnumerator KnockBack()
    {
        Debug.Log("�˹� ����");
        yield return wait;
        Vector3 playerPos = GameManager.gm.player.transform.position;
        Vector3 dirVec = transform.position - playerPos;
        rigid.AddForce(dirVec.normalized * 6, ForceMode2D.Impulse);

    }

    public void TakeDamage(int damage)
    {
        //Debug.Log("������ ����");
        StartCoroutine(KnockBack());

        if (health > 0)
        {
            anim.SetTrigger("Hit");

            health -= damage;
            hpSlider.value = health / maxHealth;
            hpText.text = health + "/" + maxHealth;

            // Debug.Log("health" + health);

        }
        else
        {
            CheckHealth();
        }
        if (type == "Boss")
        {
            GameManager.gm.bossHealth = (int)health;

        }
    }

    IEnumerator AttackPlayer()
    {
        isAttacking = true;

        if (!isLive)
            yield break;

        yield return new WaitForSeconds(1f);

        while (currentState == State.Attacking)
        {
            if (type == "Boss")
            {
                Debug.Log("���� ����");
                anim.SetTrigger("Attack");
                target.GetComponent<Player>().TakeDamage(damage);
                yield return new WaitForSeconds(5f);
            }
            else
            {
                Debug.Log("�Ϲݸ��� ����");
                anim.SetTrigger("Attack");
                target.GetComponent<Player>().TakeDamage(damage);
                yield return new WaitForSeconds(3f);
            }
        }

        anim.SetTrigger("Idle");
        //isAttacking = false;
    }

    public void CheckHealth()
    {
        if (health <= 0)
        {
            isLive = false;
            coll.enabled = false;
            rigid.simulated = false;
            spriter.sortingOrder = 1;
            anim.SetBool("Dead", true);

            StopCoroutine(AttackPlayer());
            isAttacking = false;
        }
    }

    public void Dead()
    {
        Debug.Log("Dead �Լ� ȣ���");

        if (type == "Boss")
        {
            GameManager.gm.bossTime = false;
            GameManager.gm.score += 1000;
        }
        else
        {
            GameManager.gm.score += 100;
        }
        gameObject.SetActive(false);
        GameManager.gm.kill++;
    }
    public void Init(SpawnData data)
    {
        anim.runtimeAnimatorController = animCon[data.spriteType];
        speed = data.speed;
        maxHealth = data.health;
        health = data.health;
        damage = data.damage;
        spriter.sortingOrder = 2 + data.spriteType; ;
    }
}
