using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinChecker : MonoBehaviour
{
    [SerializeField] private DataItem dataGreenCube;
    [SerializeField] private DataItem dataYellowCube;

    private int countCubesCurrent;
    private int countCubesNeeded;
    private int countEnemies;

    // Start is called before the first frame update
    void Start()
    {
        // get count of needed cubes
        foreach (Item item in FindObjectsOfType<Item>())
        {
            if (item.data == dataGreenCube || item.data == dataYellowCube)
            {
                countCubesNeeded++;
            }
        }

        foreach (Enemy enemy in FindObjectsOfType<Enemy>())
        {
            enemy.Died.AddListener(OnEnemyDied);
            countEnemies++;
        }

        FindObjectOfType<Player>().Died.AddListener(OnPlayerDied);

        // get count of enemies
        countEnemies = FindObjectsOfType<Enemy>().Length;

        UIGame.instance.SetCubesNeeded(countCubesNeeded);
        Inventory.instance.Changed.AddListener(OnInventoryChanged);
    }

    private void OnEnemyDied()
    {
        countEnemies--;
        CheckForWin();
    }

    private void OnInventoryChanged()
    {
        // update cubes UI
        countCubesCurrent = Inventory.instance.GetItemCount(dataGreenCube) + Inventory.instance.GetItemCount(dataYellowCube);
        UIGame.instance.SetCubesCurrent(countCubesCurrent);
        CheckForWin();
    }

    private void OnPlayerDied()
    {
        Game.instance.OnGameLose();
    }

    private void CheckForWin()
    {
        if (countCubesCurrent == countCubesNeeded && countEnemies == 0)
        {
            Game.instance.OnGameWin();
        }
    }
}
