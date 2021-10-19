using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerBasics : MonoBehaviour
{
    [Header("Variables")]
    public float runSpeed = 25;
    public float maxHealth = 100;
    public float attackRange = 1f;
    public float teleTime = 10;
    public float teleStanDev = 6;
    public float slashDamage = 25;

    [Header("Private Variables")]
    float health = 100;
    float inpX = 0;
    float inpY = 0;
    float teltime = 0;
    float attMult = 1;

    [Header("References")]
    public Transform[] telePoints;
    public LayerMask EnemyLayers;
    public Transform AttackPoint;
    public AudioSource telp;
    CharacterController2D controller;
    Animator animator;
    Rigidbody2D rb;
    AudioSource source;
    
    void Start()
    {
        health = maxHealth;
        teltime = teleTime + Random.Range(0, teleStanDev);
        controller = GetComponent<CharacterController2D>();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        source = GetComponent<AudioSource>();
    }

    void Update()
    {
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);

        checkInput();

        if (teltime <= 0)
        {
            animator.SetTrigger("Teleport");
            telp.Play();
            teltime = teleTime + Random.Range(0, teleStanDev);
            attMult = 0;
        }
        else
        {
            teltime -= Time.deltaTime;
        }

        if(info.IsName("player_telep") && info.normalizedTime >= 1)
        {
            Teleport();
        }

        if (info.IsName("player_telep_in") && info.normalizedTime >= 1)
        {
            attMult = 1;
        }

        if(health <= 0)
        {
            SceneManager.LoadScene(0);
        }

    }

    private void FixedUpdate()
    {
        movement();
    }

    void checkInput()
    {
        inpX = Input.GetAxis("Horizontal") * runSpeed;
        inpY = Input.GetAxis("Vertical");

        if (Input.GetAxis("Fire1")*attMult> 0)
        {
            Attack();
        }
    }

    void movement()
    {
        animator.SetBool("Running", inpX != 0);
        animator.SetBool("OnGround", controller.m_GroundCheck);
        animator.SetFloat("yVel", rb.velocity.y);
        controller.Move(inpX * Time.deltaTime, false, inpY > 0);
    }

    void Attack()
    {
        animator.SetTrigger("Attack");
        source.Play();
        Collider2D[] enemies = Physics2D.OverlapCircleAll(AttackPoint.position, attackRange, EnemyLayers);

        foreach(Collider2D enemy in enemies)
        {
            enemy.gameObject.GetComponent<BomberBasic>().GetDamage(slashDamage);
        }
    }

    void Teleport()
    {
        Random.InitState((int)Time.time);
        Transform telp = telePoints[Random.Range(0, telePoints.Length)];
        transform.position = telp.position;
        teltime = teleTime + Random.Range(0, teleStanDev);
    }

    public void GetDamage(float damage)
    {
        health -= damage;
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawWireSphere(AttackPoint.position, attackRange);
    //}

    public float getHealth()
    {
        return health;
    }
}
