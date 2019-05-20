using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public Light sun; 
    public float materialOffset;
    public float lightIntensity;
    public float startTime;
    public float maxIntensity = 1.0f;
    public float intensitySpeed = 0.01f;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.timeSinceLevelLoad; // set start time
        sun = GetComponent<Light>(); //get the directional light (sun) component
    }

    // Update is called once per frame
    void Update()
    {
        startTime = Time.timeSinceLevelLoad + 80;
        sun.intensity = Mathf.PingPong(startTime * intensitySpeed, maxIntensity); //update the new intensity with time passed
        //sun.intensity = Time.time * intensitySpeed;         
    }

}
