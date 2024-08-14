using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Animator anim;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //anim.SetTrigger("doAttack");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        anim.SetTrigger("doAttack");
    }
}
