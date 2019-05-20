using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KnightController : MonoBehaviour
{
    private Animator anim;
    private CharacterController _characterController;
    public TextMeshProUGUI levelText;
    public KnightController script;

    public float Speed;
    public float runSpeed = 7f;
    public float walkSpeed = 4f;
    public float RotationSpeed = 240.0f;
    private float Gravity = 20.0f;
    public bool movementDisabled = false;
    public bool attacking = false;
    public int level;

    public Image healthBar;
    public float maxHealth;
    public float health;

    public Image staminaBar;
    public float maxStamina;
    public float stamina;

    public Image expBar;
    public float maxExp;
    public float exp;

    public bool alive;
    public bool dead;

    private Vector3 _moveDir = Vector3.zero;

    public int idleType;
    public int lastIdleType;

    public float attack1Cost = 10f;
    public float stabCost = 15f;
    public float combo3Cost = 20f;
    public float combo2Cost = 25f;
    public float spinCost = 35f;
    public float oneforallCost = 40f;

    public AudioClip death;
    public AudioClip hit;
    public AudioSource soundSource;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        _characterController = GetComponent<CharacterController>();
        script = GetComponent<KnightController>();

        level = 1;

        idleType = 0;
        lastIdleType = idleType;

        maxHealth = 300;
        health = maxHealth;

        maxStamina = 100;
        stamina = maxStamina;

        maxExp = 100;
        exp = 0;

        alive = true;
        //anim.SetTrigger("wake");
    }

    // Update is called once per frame
    void Update()
    {
        isWaking();

        if (stamina < maxStamina)
        {
            float temp = Time.deltaTime * 5f;
            giveStamina(temp);
        }

        if (Speed > 7f && stamina > 0f && Input.GetKey(KeyCode.LeftShift))
        {
            float temp = Time.deltaTime * 10f;
            takeStamina(temp);
        }

        if (!movementDisabled)
        {
            if (stamina > 0f)
            {
                Speed = (Input.GetKey(KeyCode.LeftShift) || Input.GetKey("joystick button 8") ? runSpeed : walkSpeed); //run speed if holding shift else walk speed
                anim.SetFloat("speed", _characterController.velocity.magnitude); //set speed param in animator
                anim.SetBool("jump", false);
            }
            else
            {
                Speed = 7;
            }

            // Get Input for axis
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            // Calculate the forward vector
            Vector3 camForward_Dir = Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1)).normalized;
            Vector3 move = v * camForward_Dir + h * Camera.main.transform.right;

            if (move.magnitude > 1f) move.Normalize();

            // Calculate the rotation for the player
            move = transform.InverseTransformDirection(move);

            // Get Euler angles
            float turnAmount = Mathf.Atan2(move.x, move.z);

            transform.Rotate(0, turnAmount * RotationSpeed * Time.deltaTime, 0);

            if (_characterController.isGrounded)
            {
                //anim.SetBool("run", move.magnitude> 0);
                _moveDir = transform.forward * move.magnitude;
                _moveDir *= Speed;
            }

            _moveDir.y -= Gravity * Time.deltaTime;
            _characterController.Move(_moveDir * Time.deltaTime);



            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
            {
                anim.SetBool("running", true);
            }
            else if (Input.GetKey(KeyCode.Space))
            {
                //anim.SetBool("jump", true);
                //anim.SetTrigger("trig");
                //isJumping();
            }
            else
            {
                anim.SetBool("shoulderLook", false);
                anim.SetBool("groundLook", false);
                anim.SetBool("running", false);
                anim.SetBool("attack1", false);

                //play random idle animaion when not moving
                idleType = Random.Range(0, 13);

                if (idleType == 2 || idleType == 5) //if last idle animation was shoulder look
                {
                    while (idleType == lastIdleType)
                    {
                        idleType = Random.Range(0, 13);
                        //Debug.Log("2,5 called");
                    }
                }
                else if (idleType == 3 || idleType == 6) //if last idle animation was ground look
                {
                    while (idleType == lastIdleType)
                    {
                        idleType = Random.Range(0, 13);
                        //Debug.Log("3,6 called");
                    }
                }

                lastIdleType = idleType; //update the last idle type variable

                if (idleType == 2 || idleType == 5) //set animation to shoulder look
                {
                    anim.SetBool("shoulderLook", true);
                }
                else if (idleType == 3 || idleType == 6) //set animation to ground look
                {
                    anim.SetBool("groundLook", true);
                }

            }

            if (Input.GetKeyDown(KeyCode.Minus)) //testing player UI
            {
                takeDamage(10);
                takeStamina(10);
                giveExp(10);
            }

            if (Input.GetKeyDown(KeyCode.Equals)) //testing player UI
            {
                giveHealth(10);
                giveStamina(10);
                giveExp(10);
            }

        }

        isAttacking();
        if ((Input.GetMouseButtonDown(0) || Input.GetKeyDown("joystick button 9")) && !attacking) // attack on click
        {
            if (stamina - attack1Cost >= 0)
            {
                int ran = Random.Range(0, 3);
                stamina = stamina - attack1Cost;
                if (ran == 0) { anim.SetBool("attack1", true); }
                else if (ran == 1) { anim.SetBool("attack2", true); }
                else if (ran == 2) { anim.SetBool("attack3", true); }

                attacking = true;
            }

        }
        else { isAttacking(); }

        if ((Input.GetKeyDown(KeyCode.Alpha1) || Input.GetKeyDown("joystick button 0")) && !attacking) // abillity 1 on press 1
        {
            if (stamina - stabCost >= 0)
            {
                stamina = stamina - stabCost;
                anim.SetBool("stab", true);

                attacking = true;
            }
        }
        else if ((Input.GetKeyDown(KeyCode.Alpha2) || Input.GetKeyDown("joystick button 1")) && !attacking) // abillity 2 on press 2
        {
            if (stamina - combo3Cost >= 0)
            {
                stamina = stamina - combo3Cost;
                anim.SetBool("combo3", true);

                attacking = true;
            }
        }
        else if ((Input.GetKeyDown(KeyCode.Alpha3) || Input.GetKeyDown("joystick button 2")) && !attacking) // abillity 3 on press 3
        {
            if (stamina - combo2Cost >= 0)
            {
                stamina = stamina - combo2Cost;
                anim.SetBool("combo2", true);

                attacking = true;
            }
        }
        else if ((Input.GetKeyDown(KeyCode.Alpha4) || Input.GetKeyDown("joystick button 3")) && !attacking) // abillity 4 on press 4
        {
            if (stamina - spinCost >= 0)
            {
                stamina = stamina - spinCost;
                anim.SetBool("spin", true);

                attacking = true;
            }
        }
        else if ((Input.GetKeyDown(KeyCode.Alpha5) || Input.GetKeyDown("joystick button 0")) && !attacking) // abillity 5 on press 5
        {
            if (stamina - oneforallCost >= 0)
            {
                stamina = stamina - oneforallCost;
                anim.SetBool("oneforall", true);

                attacking = true;
            }
        }

    }

        public void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.tag == "EnemyWeapon")
        {
            Weapon w = collision.gameObject.GetComponent<Weapon>(); // get the weapon script from the colliding object 
            float a = w.damage;
            takeDamage(a);
            Debug.Log("pain " + a);
        }
    }

    public void randomIdleAnim()
    {
        int ran = Random.Range(1, 4);
        anim.SetFloat("idle", (float)ran);
        //Debug.Log(ran);
    }

    public void isAttacking()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Standing Melee Attack Horizontal") || anim.GetCurrentAnimatorStateInfo(0).IsName("Sword And Shield Slash") || anim.GetCurrentAnimatorStateInfo(0).IsName("Standing Melee Attack Downward") || anim.GetCurrentAnimatorStateInfo(0).IsName("Stab") || anim.GetCurrentAnimatorStateInfo(0).IsName("Combo3") || anim.GetCurrentAnimatorStateInfo(0).IsName("Combo2") || anim.GetCurrentAnimatorStateInfo(0).IsName("Standing Melee Attack 360 Low"))
        {
            movementDisabled = true;
            attacking = true;
            //Debug.Log("Attacking!");
            //Debug.Log(movementDisabled);
        }
        else
        {
            movementDisabled = false;
            anim.SetBool("attack1", false);
            anim.SetBool("attack2", false);
            anim.SetBool("attack3", false);
            anim.SetBool("stab", false);
            anim.SetBool("combo3", false);
            anim.SetBool("combo2", false);
            anim.SetBool("spin", false);
            anim.SetBool("oneforall", false);
            attacking = false;
        }
    }

    public void isJumping()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Standing Jump"))
        {
            movementDisabled = true;
            Debug.Log("Jumping!");
        }
        else
        {
            movementDisabled = false;
        }
    }

    public void isWaking()
    {
        if (anim.GetCurrentAnimatorStateInfo(0).IsName("Stand Up"))
        {
            movementDisabled = true;
        }
        else
        {
           // movementDisabled = false;
        }
    }

    // take off damage and update health bar UI
    public void takeDamage(float damage)
    {
        health = health - damage;

        healthBar.fillAmount = health / maxHealth;

        if (health <= 0 && alive) //die
        {
            soundSource.clip = death;
            soundSource.Play();
            anim.SetTrigger("death");
            alive = false;
            script.enabled = false;
        }
        else
        {
            soundSource.clip = hit;
            soundSource.Play();
        }
    }

    public void giveHealth(float value)
    {
        health = health + value;
        if (health > maxHealth) { health = maxHealth; }
        healthBar.fillAmount = health / maxHealth;
    }

    // take off damage and update stamina bar UI
    public void takeStamina(float damage)
    {
        if (!movementDisabled)
        {
            stamina = stamina - damage;
            staminaBar.fillAmount = stamina / maxStamina;
        }
    }

    public void giveStamina(float value)
    {
        stamina = stamina + value;
        if (stamina > maxStamina) { stamina = maxStamina; }
        staminaBar.fillAmount = stamina / maxStamina;
    }

    public void giveExp(float value)
    {
        exp = exp + value;
        expBar.fillAmount = exp / maxExp;
        if (expBar.fillAmount >= 1)
        {
            levelUp();
        }
    }

    void emptyExp()
    {
        exp = exp - maxExp;
        expBar.fillAmount = exp / (maxExp*1.2f);
    }

    void levelUp()
    {
        level++;
        levelText.text = "LEVEL " + level;
       // maxExp = maxExp * 1.2f;
        emptyExp();
        maxExp = maxExp * 1.2f; //increase the max capactity of health, stamina and exp
        maxHealth = maxHealth * 1.2f;
        maxStamina = maxStamina * 1.2f;
        stamina = maxStamina;
        health = maxHealth;
        takeDamage(0);
        giveStamina(0);
    }

}
