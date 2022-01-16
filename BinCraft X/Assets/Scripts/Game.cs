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

    public GameObject canvas;
    public GameObject panelPause;
    public Text textMenu;

    public bool playerControllable;

    public GameObject goItems;

    // set to false on game win or lose
    private bool running = true;

    private Player player;

    private bool paused;
    public bool Paused
    {
        set
        {
            paused = value;
            panelPause.SetActive(paused);
            UIGame.instance.SetPanelVisible(!paused);

            FindObjectOfType<Movement>().enabled = !paused;
            FindObjectOfType<Looking>().enabled = !paused;
            FindObjectOfType<Interaction>().enabled = !paused;
            FindObjectOfType<Shooting>().enabled = !paused;
            FindObjectOfType<Well>().enabled = !paused;
            BulletPool.instance.gameObject.SetActive(!paused);

            Time.timeScale = paused ? 0 : 1;

            // NOTE: this works weird and agent loses their velocity on resume
            // (actually it works when timeScale is set to 0)
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
        UIGame.instance.SetPanelVisible(true);
        UIInventory.instance.SetPanelVisible(false, true);

        Paused = false;
        playerControllable = true;

        Audio.instance.PlaySong(SongID.Game);
    }

    private void Update()
    {
        if (running)
        {
            // pause menu
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (UIInventory.instance.GetPanelVisible())
                {
                    UIInventory.instance.SetPanelVisible(false);
                    SetMouseLocked(true);
                    playerControllable = true;
                }
                else
                {
                    Paused = !Paused;
                    textMenu.text = TEXT_MENU_PAUSED;
                }
            }

            // inventory
            if (Input.GetKeyDown(KeyCode.Tab) && !paused)
            {
                bool inventoryVisible = UIInventory.instance.GetPanelVisible();
                UIInventory.instance.SetPanelVisible(!inventoryVisible);
                SetMouseLocked(inventoryVisible);
                playerControllable = inventoryVisible;
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
        textMenu.text = TEXT_MENU_WIN;
    }

    public void OnGameLose()
    {
        running = false;
        Paused = true;
        textMenu.text = TEXT_MENU_LOSE;
    }

    public void SpawnItem(DataItem data, int amount)
    {
        GameObject goItem = Instantiate(data.prefabItem, goItems.transform);
        goItem.transform.position = player.transform.position;
        Item item = goItem.GetComponent<Item>();
        item.Data = data;
        item.name = data.name;
        item.amount = amount;
        item.ApplyData();
    }

    private void SetMouseLocked(bool value)
    {
        Cursor.lockState = value ? CursorLockMode.Locked : CursorLockMode.None;
    }
}
