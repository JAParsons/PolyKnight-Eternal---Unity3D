using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private Animator anim;
    public Image healthBar;
    public CapsuleCollider col;
    private NavMeshAgent agent;
    public Potion pot;
    public GameObject player;
    public GameObject UI;
    public AI script;
    public float maxHealth = 200;
    public float health;
    public bool alive;
    public bool dead;
    public float exp = 25f;
    public bool attacking;
    public bool flinching;

    public AudioClip hit;
    public AudioClip death;
    public AudioSource soundSource;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        col = GetComponent<CapsuleCollider>();
        agent = GetComponent<NavMeshAgent>();
        script = GetComponent<AI>();
        //maxHealth = 200;
        health = maxHealth;
        alive = true;
        dead = false;
        //exp = 25f;
        //pot = new Potion();
        //pot.HealthInit();
    }

    // Update is called once per frame
    void Update()
    {
        
        //takeDamage(0.1f);

        if (!alive && !dead)
        {
            col.enabled = !col.enabled;
            script.enabled = !script.enabled;
            agent.enabled = !agent.enabled;
            UI.SetActive(false);

            int death = Random.Range(0,2);
            if (death == 0) { anim.SetTrigger("death"); }
            else if (death == 1) { anim.SetTrigger("death2"); }

            dead = true;

            GameObject.Find("Player").GetComponent<KnightController>().giveExp(exp); //give exp

            int temp = Random.Range(0,100);
            if (temp >40)
            {
                Instantiate(pot, transform.position, Quaternion.identity);
            }
            
        }
    }

    public void OnTriggerEnter(Collider collision)
    {
        if(collision.gameObject.tag == "Weapon")
        {
            Weapon w = collision.gameObject.GetComponent<Weapon>(); // get the weapon script from the colliding object 
            float a = w.damage;
            takeDamage(a);
            Debug.Log(a);
        }
    }

    public void isAttacking()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Standing Melee Attack Horizontal") || anim.GetCurrentAnimatorStateInfo(0).IsName("Sword And Shield Slash") || anim.GetCurrentAnimatorStateInfo(0).IsName("Standing Melee Attack Downward"))
        {
            attacking = true;
            //Debug.Log("Attacking!");
            //Debug.Log(movementDisabled);
        }
        else
        {
            attacking = false;
            anim.SetBool("attack1", false);
        }
    }

    // take off damage and update health bar UI
    void takeDamage(float damage)
    {
        health = health - damage;

        healthBar.fillAmount = health/maxHealth;

        soundSource.clip = hit;
        soundSource.Play(); //play hit sound effect

        if (health <= 0) //die
        {
            alive = false;
            soundSource.clip = death;
            soundSource.Play(); //play death sound effect
        }
       // else if (!flinching && health > 0)
        {
            //anim.SetTrigger("damage");
            //isFlinching();
           // anim.SetBool("hurt", false);
        }
       // isFlinching();
    }

    public void isFlinching()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Flinch"))
        {
            flinching = true;
        }
        else
        {
            flinching = false;
            anim.SetTrigger("noDamage");
            takeDamage(0);
        }
    }
}
