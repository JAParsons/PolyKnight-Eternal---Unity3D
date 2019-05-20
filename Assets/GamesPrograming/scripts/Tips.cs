using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Tips : MonoBehaviour
{
    public static bool paused = false;
    public GameObject tip1;
    public GameObject tip2;
    public float count = 0f;


    private void Start()
    {
        tip1.SetActive(false);
        tip2.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        count = count + Time.deltaTime;
        if (count > 5 && count < 9) { tip1.SetActive(true); }
        else if (count > 9) { tip1.SetActive(false); }

        if (count > 11 && count < 19) { tip2.SetActive(true); }
        else if (count > 19) { tip2.SetActive(false); }
    }
}
