using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialOffset : MonoBehaviour
{
    public Renderer texture;
    public float startTime;
    public float maxOffset = 1.0f;
    public float offsetRate;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.timeSinceLevelLoad; // set start time
        texture = GetComponent<Renderer>(); //get the skydome's renderer component
        offsetRate = 0.005f;
}

    // Update is called once per frame
    void Update()
    {
        //offsetRate = 0.005f;
        startTime = Time.timeSinceLevelLoad + 200;
        texture.material.SetTextureOffset("_MainTex", new Vector2(startTime * offsetRate, 0)); //= Mathf.PingPong(startTime * offsetRate, maxIntensity); //update the new intensity with time passed
        //sun.intensity = Time.time * offsetRate;         
    }
}
