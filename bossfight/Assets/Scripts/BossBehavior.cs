using UnityEngine;
using NUnit.Compatibility;
using System.Collections;
using Unity.VisualScripting;
using UnityEditor;
public class BossBehavior : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private float distanceWakeUp;
    [SerializeField] private float distanceAttack;
    [SerializeField] private float distanceHome;
    [SerializeField] private float speed;
    [SerializeField] private GameObject home;
    [SerializeField] private int life;

    private Animator animator;
    private SpriteRenderer spriteRender;
    private bool walking;
    private bool wakeUp;
    private bool attacking;
    private bool sleeping;
    private bool goingHome;

    void Awake()
    {
        animator = GetComponent<Animator>();
        spriteRender = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        home = GameObject.FindGameObjectWithTag("Finish");
        wakeUp = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (wakeUp == false)
        {
            if (Vector2.Distance(transform.position, player.transform.position) < distanceWakeUp)
            {
                wakeUp = true;
                goingHome = false;
                sleeping = false;
                walking = false;    
            }
            else
            {
                walking = true;
                GoHome();
            }
        }
        else
        {
            if (Vector2.Distance(transform.position, player.transform.position) > distanceWakeUp)
            {
                wakeUp = false;
            }
            if (Vector2.Distance(transform.position, player.transform.position) < distanceAttack)
            {
                Attack();
            }
            else
            {
                walking = true;
                Move();
            }
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(Damage());
        }


        AnimationManager();
    }

    void Move()
    {
        if (attacking == true) return;

        transform.position = Vector2.MoveTowards(transform.position,player.transform.position,speed * Time.deltaTime);

        if (wakeUp == true)
        {
            if (player.transform.position.x > transform.position.x)
            {
                spriteRender.flipX = false;
            }
            else if (player.transform.position.x < transform.position.x)
            {
                spriteRender.flipX = true;
            }
        }
    }
    void GoHome()
    {
        
        transform.position = Vector2.MoveTowards(transform.position, home.transform.position, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, player.transform.position) > distanceHome)
        {
            goingHome = true;
        }
        else
        {
            sleeping = true;
            walking = false;    
            goingHome = false;
        }
        if (goingHome == true)
        { 
            if (home.transform.position.x > transform.position.x)
            {
                spriteRender.flipX = false;
            }
            else if (home.transform.position.x < transform.position.x)
            {
                spriteRender.flipX = true;
            }

        }
    }

    void Attack()
    {
        StartCoroutine(Attacking());
    }

    IEnumerator Attacking()
    {
        walking = false;
        attacking = true;
        wakeUp = true;
        yield return new WaitForSeconds(1.5f);
        attacking = false;

    }

    void AnimationManager()
    {
        animator.SetBool("pSleep", sleeping);
        animator.SetBool("pWake", walking);
        animator.SetBool("pAttack", attacking);
        animator.SetBool("pDie", life <= 0);
    }


    IEnumerator Damage()
    {
        life--;

        if (life > 0)
        {
            spriteRender.color = Color.red;
            yield return new WaitForSeconds(0.25f);
            spriteRender.color = Color.white;
            yield return new WaitForSeconds(0.25f);
        }
        else
        {
            yield return new WaitForSeconds(0.4f);
            Destroy(gameObject);
        }

    }




}
