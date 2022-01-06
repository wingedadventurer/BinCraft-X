using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    const string TEXT_MENU_PAUSED = "Game Paused";
    const string TEXT_MENU_WIN = "You died!";
    const string TEXT_MENU_LOSE = "You are victorious!";

    public GameObject canvas;
    public GameObject panelPause;
    public Text textPause;

    private bool paused;
    public bool Paused
    {
        set
        {
            paused = value;
            panelPause.SetActive(paused);
        }
        get
        {
            return paused;
        }
    }

    // set to false on game win or lose
    private bool running = true;

    private void Start()
    {
        // in case we disable canvas in editor
        canvas.SetActive(true);

        panelPause.SetActive(false);
    }

    private void Update()
    {
        if (running)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Paused = !Paused;
                textPause.text = TEXT_MENU_PAUSED;
            }

            // DEBUG: test game win and lose
            if (Input.GetKeyDown(KeyCode.Alpha9)) { OnGameWin(); }
            if (Input.GetKeyDown(KeyCode.Alpha0)) { OnGameLose(); }
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void EndGame()
    {
        SceneManager.LoadScene(0);
    }

    public void OnGameWin()
    {
        running = false;
        Paused = true;
        textPause.text = TEXT_MENU_WIN;
    }

    public void OnGameLose()
    {
        running = false;
        Paused = true;
        textPause.text = TEXT_MENU_LOSE;
    }
}
