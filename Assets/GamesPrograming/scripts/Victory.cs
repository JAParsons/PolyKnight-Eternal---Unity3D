using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Victory : MonoBehaviour
{
    public static bool paused = false;
    public GameObject pauseUI;
    private EnemyController player;
    public float t = 0f;


    private void Start()
    {
        paused = false;
        player = GetComponent<EnemyController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (player.health <= 0 && !paused)
        {
            t = t + Time.deltaTime;
            if (t >= 2)
            {
                Pause();
            }
        }
    }

    public void Resume()
    {
        pauseUI.SetActive(false); //hide UI
        Time.timeScale = 1f; //set time to normal speed
        paused = false;
    }

    public void Pause()
    {
        pauseUI.SetActive(true);
        Time.timeScale = 0f;
        paused = true;
    }

    public void LoadMainMenu()
    {
        paused = false;
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void Exit()
    {
        Application.Quit();
    }
}
