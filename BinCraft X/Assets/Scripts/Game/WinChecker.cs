using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinChecker : MonoBehaviour
{
    [SerializeField] private DataItem dataIceCube;

    private int countRemainingIceCubes;
    private int countEnemies;

    private UIGame uiGame;

    void Start()
    {
        uiGame = UIGame.instance;

        // get count of ice cubes
        foreach (Item item in FindObjectsOfType<Item>())
        {
            if (item.Data == dataIceCube)
            {
                countRemainingIceCubes++;
            }
        }

        uiGame.SetCubesRemaining(countRemainingIceCubes);

        // hook up furnace finishes
        foreach (Furnace furnace in FindObjectsOfType<Furnace>())
        {
            furnace.Finished.AddListener(OnFurnaceFinished);
        }

        // hook up enemy spawns
        foreach (Well well in FindObjectsOfType<Well>())
        {
            well.Spawned.AddListener(delegate { OnEnemySpawned(well.lastSpawnedEnemy); });
        }

        // hook up enemy deaths
        foreach (Enemy enemy in FindObjectsOfType<Enemy>())
        {
            enemy.Died.AddListener(OnEnemyDied);
            countEnemies++;
        }

        // hook up player death
        FindObjectOfType<Player>().Died.AddListener(OnPlayerDied);
    }

    private void OnFurnaceFinished()
    {
        countRemainingIceCubes--;
        UIGame.instance.SetCubesRemaining(countRemainingIceCubes);
        CheckForWin();
    }

    private void OnEnemySpawned(Enemy enemy)
    {
        enemy.Died.AddListener(OnEnemyDied);
        countEnemies++;
        uiGame.SetEnemiesRemaining(countEnemies);
    }

    private void OnEnemyDied()
    {
        countEnemies--;
        uiGame.SetEnemiesRemaining(countEnemies);
        CheckForWin();
    }

    private void OnPlayerDied()
    {
        Game.instance.OnGameLose();
    }

    private void CheckForWin()
    {
        if (countRemainingIceCubes == 0 && countEnemies == 0)
        {
            Game.instance.OnGameWin();
        }
    }
}
