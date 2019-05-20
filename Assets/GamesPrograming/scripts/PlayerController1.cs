using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float speed;
    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        speed = 7;
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //transform.Translate(Input.GetAxis("Horizontal")*Time.deltaTime*speed, 0f, Input.GetAxis("Vertical") * Time.deltaTime * speed);
        float moveHorizontal = Input.GetAxisRaw("Horizontal");
        float moveVertical = Input.GetAxisRaw("Vertical");
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
      
        if (movement != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement), 0.15F);
            //transform.rotation = Quaternion.LookRotation(movement);
        }
        transform.Translate(movement * speed * Time.deltaTime, Space.World);

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            anim.SetBool("running", true);
        }
        else
        {
            anim.SetBool("running", false);
        }
        
        if(speed > 7) { speed = 7; }
    }
}
