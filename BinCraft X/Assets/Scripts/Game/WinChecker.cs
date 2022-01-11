using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinChecker : MonoBehaviour
{
    [SerializeField] private DataItem dataGreenCube;
    [SerializeField] private DataItem dataYellowCube;

    private int countCubesCurrent;
    private int countCubesNeeded;
    public int countEnemies;

    // Start is called before the first frame update
    void Start()
    {
        // get count of needed cubes
        foreach (Item item in FindObjectsOfType<Item>())
        {
            if (item.Data == dataGreenCube || item.Data == dataYellowCube)
            {
                countCubesNeeded++;
            }
        }

        // hook up enemy deaths
        foreach (Enemy enemy in FindObjectsOfType<Enemy>())
        {
            enemy.Died.AddListener(OnEnemyDied);
            countEnemies++;
        }

        // hook up enemy spawner spawns
        foreach (EnemySpawner enemySpawner in FindObjectsOfType<EnemySpawner>())
        {
            enemySpawner.Spawned.AddListener(delegate { OnEnemySpawnerSpawned(enemySpawner); });
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

    private void OnEnemySpawnerSpawned(EnemySpawner enemySpawner)
    {
        enemySpawner.lastSpawnedEnemy.Died.AddListener(OnEnemyDied);
        countEnemies++;
    }

    private void CheckForWin()
    {
        if (countCubesCurrent == countCubesNeeded && countEnemies == 0)
        {
            Game.instance.OnGameWin();
        }
    }
}
