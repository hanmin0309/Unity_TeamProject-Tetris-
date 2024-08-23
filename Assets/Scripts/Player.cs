using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public Animator anim;

    public int health, maxHealth;

    public Slider playerHpBar;
    public Text playerHpText;

    AudioManager audioManager;

    // Start is called before the first frame update
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag ("AudioManager").GetComponent<AudioManager> ();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //anim.SetTrigger("doAttack");
    }


    public void TakeDamage(int damage)
    {

        if (health <= 0)
        {
            anim.SetTrigger("doDie");
            GameManager.gm.GameOver();
            return;
        }

        anim.SetTrigger("doHit");
        health -= damage;
        //Debug.Log("슬라이더 값" + (float) health / maxHealth);
        playerHpBar.value = (float)health / maxHealth;
        playerHpText.text = health + "/" + maxHealth;
        GameManager.gm.health = health;
    }



    public void AttackMotion()
    {
        audioManager.PlaySFX(audioManager.sword);
        anim.SetTrigger("doAttack");
    }
}
