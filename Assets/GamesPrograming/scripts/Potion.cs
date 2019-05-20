using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potion : MonoBehaviour
{
    public GameObject healthMesh;
    public GameObject staminaMesh;
    public GameObject expMesh;
    private string potionType;
    private float potionValue;
    public AudioClip pickup;
    public AudioSource soundSource;

    void Awake()
    {
        int type = Random.Range(1, 10);
        if (type >= 1 && type <= 4)
        {
            HealthInit();
        }
        if (type >= 5 && type <= 9)
        {
            StaminaInit();
        }
        //Debug.Log("Type = "+type);        
    }

    private void OnTriggerEnter(Collider other) //when player collides with potion
    {
        if (other.CompareTag("Player"))
        {
            soundSource.clip = pickup;
            soundSource.Play();
            Pickup();
        }
    }

    public void Pickup() 
    {
        Destroy(gameObject);
        ApplyEffect();
    }

    public void ApplyEffect() // apply the effect of the potion
    {
        if (potionType == "health")
        {
            GameObject.Find("Player").GetComponent<KnightController>().giveHealth(potionValue); //add health to player
        }
        else if (potionType == "stamina")
        {
            GameObject.Find("Player").GetComponent<KnightController>().giveStamina(potionValue); //add stamina to player
        }
        else if (potionType == "exp")
        {
            GameObject.Find("Player").GetComponent<KnightController>().giveExp(potionValue); //add exp to player
        }
    }

    public void HealthInit() // initiate to a health potion
    {
        potionType = "health";
        potionValue = 40f;
        healthMesh.SetActive(true);
        staminaMesh.SetActive(false);
        expMesh.SetActive(false);
    }

    public void StaminaInit() // initiate to a stamina potion
    {
        potionType = "stamina";
        potionValue = 30f;
        healthMesh.SetActive(false);
        staminaMesh.SetActive(true);
        expMesh.SetActive(false);
    }

    public void ExpInit(float val) // initiate to a exp potion
    {
        potionType = "exp";
        potionValue = val;
        healthMesh.SetActive(false);
        staminaMesh.SetActive(false);
        expMesh.SetActive(true);
    }
}
