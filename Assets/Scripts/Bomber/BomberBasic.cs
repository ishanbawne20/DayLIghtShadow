using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BomberBasic : MonoBehaviour
{
    [Header("Variables")]
    public float bombCooldown = 3;
    public float antGrav = 4.5f;
    public float maxHealth = 100;

    [Header("References")]
    public GameObject Player;
    public GameObject Bomb;
    public Transform BombPoint;
    public TheWatcherScript watcher;
    Animator animator;

    [Header("Private Variables")]
    float time;
    float health;

    void Start()
    {
        time = bombCooldown;
        health = maxHealth;
        animator = GetComponent<Animator>();
    }


    void Update()
    {
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);

        if (time <= 0)
        {
            animator.SetTrigger("Attack");
            Throw();
            time = bombCooldown;
        }
        else
        {
            time -= Time.deltaTime;
        }

        if(health <= 0)
        {
            watcher.dedEnemy();
            Destroy(gameObject);
        }

        if (info.IsName("bomber_attack") && info.normalizedTime >= 1)
        {
            Throw();
        }
        if (info.IsName("bomber_death") && info.normalizedTime >= 1)
        {
            Debug.Log("Destroyed");
            Destroy(gameObject);
        }
    }

    void Throw()
    {
        GameObject bomb =  Instantiate(Bomb);
        bomb.transform.position = BombPoint.position;
        Rigidbody2D rb = bomb.GetComponent<Rigidbody2D>();
        rb.velocity = (Player.transform.position - transform.position) + antGrav * Vector3.up;
    }

    public void GetDamage(float damage)
    {
        health -= damage;
    }
}
