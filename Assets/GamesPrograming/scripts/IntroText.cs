using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroText : MonoBehaviour
{
    public static bool paused = false;
    public GameObject textUI;
    public float count = 0f;


    private void Start()
    {
        paused = false;
        textUI.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        count = count + Time.deltaTime;
        if (count > 2 && count< 3) { textUI.SetActive(true); }
        else if (count > 9) { textUI.SetActive(false); }
    }

    public void Resume()
    {
        textUI.SetActive(false); //hide UI
        paused = false;
    }

    public void Pause()
    {
        textUI.SetActive(true);
        paused = true;
    }
}
