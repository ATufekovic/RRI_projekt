using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour
{
    private bool isPaused = false;
    private Canvas canvasPauseMenu;
    // Start is called before the first frame update
    void Start()
    {
        canvasPauseMenu = transform.GetChild(0).gameObject.GetComponent<Canvas>();
        canvasPauseMenu.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        CheckPause();
    }

    private void CheckPause()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            isPaused = !isPaused;
            if (isPaused)
            {
                Time.timeScale = 0;
                ShowPauseMenu();
            } else
            {
                Time.timeScale = 1;
                ClosePauseMenu();
            }
        }
    }

    private void ClosePauseMenu()
    {
        canvasPauseMenu.enabled = false;
    }

    private void ShowPauseMenu()
    {
        canvasPauseMenu.enabled = true;
    }

    public void RestartCurrentScene()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("MainMenu");
    }
}
