using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI : MonoBehaviour
{
    public float lookrad = 10f;
    private Transform target;
    private NavMeshAgent agent;
    private bool moving = false;
    private Vector3 previousPosition;
    public float curSpeed;
    private Animator anim;
    public int idleType = 0;
    public int lastIdleType = 0;
    public bool running = false;
    public float velocity;
    public string currentAttack;
    public int level = 0; //defines AI type and difficulty
    public bool movementDisabled = false;
    public float attackCooldown = 2f;
    private float timer;
    private float wanderTimer;
    private string state;
    public float wanderRadius;

    // Start is called before the first frame update
    void Start()
    {
        target = PlayerManager.instance.player.transform; // set player position as the target
        agent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        state = "wander";
        wanderTimer = 3;
        wanderRadius = 20f;
        timer = wanderTimer;
        //level = 3;
    }

    // Update is called once per frame
    void Update()
    {
        attackCooldown -= Time.deltaTime;
        anim.SetFloat("speed", velocity); //set speed param in animator
        velocity = agent.velocity.magnitude / agent.speed;
        checkSpeed();
        animate();

        float distance = Vector3.Distance(target.position, transform.position); //get distance from AI and player 

        if (distance <= lookrad) //if player is in range of AI
        {
            state = "chase";
            agent.SetDestination(target.position); //move towards player

            if (distance <= agent.stoppingDistance)
            {
                // if (attackCooldown <= 0)
                {
                    attack(); //attack
                    attackCooldown = 2f;
                }
                facePlayer(); //face player
            }
        }
        else if(agent.velocity != Vector3.zero) //if not moving change state
        {
            state = "wander";
        }
        else if (state == "wander")
        {
            wander();
        }
    }

    public void facePlayer() //ajust rotaion to face player
    {
        Vector3 dir = (target.position - transform.position).normalized;
        Quaternion lookRot = Quaternion.LookRotation(new Vector3(dir.x, 0, dir.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRot, Time.deltaTime * 5f);
    }

    public void wander() //AI wanders searching for player
    {
        timer += Time.deltaTime;

        if (timer >= wanderTimer)
        {
            Vector3 newPos = RandomNavSphere(transform.position, wanderRadius, -1);
            agent.SetDestination(newPos);
            timer = 0;
        }
    }

    public static Vector3 RandomNavSphere(Vector3 origin, float dist, int layermask) //return random position in a sphere
    {
        Vector3 randDirection = Random.insideUnitSphere * dist;

        randDirection += origin;

        NavMeshHit navHit;

        NavMesh.SamplePosition(randDirection, out navHit, dist, layermask);

        return navHit.position;
    }

    public void attack()
    {
        isAttacking();
        if (!movementDisabled)
        {
            int temp = Random.Range(0, 2); //choose random attack
            if (temp == 0 && level >= 2)
            {
                anim.SetBool("attack2", true);
            }
            else if (temp == 1 && level >= 3)
            {
                anim.SetBool("attack3", true);
            }
            else
            {
                anim.SetBool("attack1", true);
            }
        }
        else
        {
            anim.SetBool("attack1", false);
            anim.SetBool("attack2", false);
            anim.SetBool("attack3", false);
        }
        //isAttacking();
    }

    public void isAttacking()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Standing Melee Attack Horizontal") || anim.GetCurrentAnimatorStateInfo(0).IsName("Sword And Shield Slash") || anim.GetCurrentAnimatorStateInfo(0).IsName("Standing Melee Attack Downward"))
        {
            movementDisabled = true;
            //Debug.Log("Attacking!");
            //Debug.Log(movementDisabled);
        }
        else
        {
            movementDisabled = false;
            anim.SetBool("attack1", false);
            anim.SetBool("attack2", false);
            anim.SetBool("attack3", false);
        }
    }

    public void animate()
    {
        if (moving && !running)
        {
            running = true;
        }
        else
        {
            anim.SetBool("shoulderLook", false);
            anim.SetBool("groundLook", false);
            anim.SetBool("running", false);
            anim.SetBool("attack1", false);
            anim.SetBool("attack2", false);
            anim.SetBool("attack3", false);


            //play random idle animaion when not moving
            idleType = Random.Range(0, 13);

            if (idleType == 2 || idleType == 5) //if last idle animation was shoulder look
            {
                //while (idleType == lastIdleType)
                {
                    idleType = Random.Range(0, 13);
                }
            }
            else if (idleType == 3 || idleType == 6) //if last idle animation was ground look
            {
                while (idleType == lastIdleType)
                {
                    idleType = Random.Range(0, 13);
                }
            }

            lastIdleType = idleType; //update the last idle type variable

            if (idleType == 2 || idleType == 5) //set animation to shoulder look
            {
                //anim.SetBool("shoulderLook", true);
            }
            else if (idleType == 3 || idleType == 6) //set animation to ground look
            {
                //anim.SetBool("groundLook", true);
            }
        }
    }

    public void checkSpeed() //check if moving 
    {
        if (agent.velocity != Vector3.zero) { moving = true; }
        else { moving = false; running = false; }
    }

    private void OnDrawGizmosSelected() //draw the detection sphere
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, lookrad);
    }

}
