using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public float baseDamage = 15;
    public float damage;
    public bool attacked;
    public BoxCollider col;
    public EnemyController player;

    // Start is called before the first frame update
    void Start()
    {
        //baseDamage = 15;
        damage = 0;
        attacked = false;
        col = GetComponent<BoxCollider>();
       // col.enabled = false;
        player = GetComponentInParent<EnemyController>();
    }

    // Update is called once per frame
    void Update()
    {
        if(player != null) //if not the player using the weapon (enemy)
        {
            //Debug.Log("YEEESSSSS");
            player.isAttacking();
            if (player.attacking)
            {
                attacked = true;
                damage = baseDamage;
                col.enabled = true;
            }
            else
            {
                attacked = false;
                col.enabled = false;
            }
            
        }
        else
        {
            if (GameObject.Find("Player").GetComponent<KnightController>().movementDisabled && !attacked) //check the movementDisabled var from the player script
            {
                attacked = true;
                damage = baseDamage;
                col.enabled = true;
            }
            else
            {
                attacked = false;
                damage = 0;
                col.enabled = false;
            }
            //Debug.Log(attacking);
        }
    }
}
