using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public static Game instance;

    const string TEXT_MENU_PAUSED = "Game Paused";
    const string TEXT_MENU_WIN = "You are victorious!";
    const string TEXT_MENU_LOSE = "You died!";

    [SerializeField] private DataItem dataGreenCube;
    [SerializeField] private DataItem dataYellowCube;

    public GameObject canvas;
    public GameObject panelPause;
    public GameObject panelGame;
    public GameObject panelInventory;
    public Text textPause;

    public bool playerControllable;

    // set to false on game win or lose
    private bool running = true;

    private Player player;

    private int countCubesNeeded;

    private bool paused;
    public bool Paused
    {
        set
        {
            paused = value;
            panelPause.SetActive(paused);
            panelGame.SetActive(!paused);

            FindObjectOfType<Movement>().enabled = !paused;
            FindObjectOfType<MouseLook>().enabled = !paused;
            FindObjectOfType<Interaction>().enabled = !paused;
            FindObjectOfType<Shooting>().enabled = !paused;
            FindObjectOfType<BulletPool>().SetBulletsEnabled(!paused);

            // NOTE: this works weird and agent loses their velocity on resume
            foreach (NavMeshAgent nma in FindObjectsOfType<NavMeshAgent>()) { nma.isStopped = paused; }

            foreach (Patroling patroling in FindObjectsOfType<Patroling>()) { patroling.enabled = !paused; }
            foreach (HealthConstantModifier healthConstantModifier in FindObjectsOfType<HealthConstantModifier>()) { healthConstantModifier.enabled = !paused; }

            SetMouseLocked(!paused);
        }
        get
        {
            return paused;
        }
    }

    private void Awake()
    {
        instance = this;

        player = FindObjectOfType<Player>();
    }

    private void Start()
    {
        canvas.SetActive(true);
        panelPause.SetActive(false);
        panelGame.SetActive(true);
        panelInventory.SetActive(false);

        Paused = false;
        playerControllable = true;

        // get count of needed cubes
        foreach (Item item in FindObjectsOfType<Item>())
        {
            if (item.data == dataGreenCube || item.data == dataYellowCube)
            {
                countCubesNeeded++;
            }
        }
        UIGame.instance.SetCubesNeeded(countCubesNeeded);
    }

    private void Update()
    {
        if (running)
        {
            // pause menu
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (panelInventory.activeSelf)
                {
                    panelInventory.SetActive(false);
                    SetMouseLocked(true);
                    playerControllable = true;
                }
                else
                {
                    Paused = !Paused;
                    textPause.text = TEXT_MENU_PAUSED;
                }
            }

            // inventory
            if (Input.GetKeyDown(KeyCode.Tab) && !paused)
            {
                bool inventoryVisible = !panelInventory.activeSelf;
                panelInventory.SetActive(inventoryVisible);
                SetMouseLocked(!inventoryVisible);
                playerControllable = !inventoryVisible;
            }

            // DEBUG: test game win and lose
            if (Input.GetKeyDown(KeyCode.Alpha9)) { OnGameWin(); }
            if (Input.GetKeyDown(KeyCode.Alpha0)) { OnGameLose(); }

            // DEBUG: print inventory
            if (Input.GetKeyDown(KeyCode.P)) { Inventory.instance.Print(); }
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

    private void SetMouseLocked(bool value)
    {
        Cursor.lockState = value ? CursorLockMode.Locked : CursorLockMode.None;
    }
}
