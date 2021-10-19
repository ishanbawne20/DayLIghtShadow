using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombScript : MonoBehaviour
{
    public float bombRange = 0.5f;
    public LayerMask PlayerLayer;
    public float dealDamage = 10;

    AudioSource source;

    Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
        source = GetComponent<AudioSource>();
    }

    void Update()
    {
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);

        if (info.IsName("bomb_explosion") && info.normalizedTime >= 1)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(transform.position, bombRange, PlayerLayer);

        foreach (Collider2D player in enemies)
        {
            if (player.tag == "Player")
            {
                player.GetComponent<PlayerBasics>().GetDamage(dealDamage);
            }
        }
        animator.SetTrigger("exp");
        source.Play();
    }

//    private void OnDrawGizmos()
//    {
//        Gizmos.DrawWireSphere(transform.position, bombRange);
//    }
}
