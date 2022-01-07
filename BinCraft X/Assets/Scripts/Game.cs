using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    const string TEXT_MENU_PAUSED = "Game Paused";
    const string TEXT_MENU_WIN = "You are victorious!";
    const string TEXT_MENU_LOSE = "You died!";

    public GameObject canvas;
    public GameObject panelPause;
    public GameObject panelGame;
    public Text textPause;

    private bool paused;
    public bool Paused
    {
        set
        {
            paused = value;
            panelPause.SetActive(paused);
            panelGame.SetActive(!paused);
            player.GetComponent<MouseLook>().active = !paused;
            player.GetComponent<Movement>().active = !paused;
            Cursor.lockState = paused ? CursorLockMode.None : CursorLockMode.Locked;
        }
        get
        {
            return paused;
        }
    }

    // set to false on game win or lose
    private bool running = true;

    private GameObject player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Start()
    {
        // in case we disable canvas in editor
        canvas.SetActive(true);

        Paused = false;
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
