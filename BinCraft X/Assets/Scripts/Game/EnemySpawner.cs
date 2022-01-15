using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    public GameObject prefabEnemy;
    public Transform transformParent;
    public Transform transformSpawn;
    public DataEnemy data;
    public List<Transform> patrolPoints = new List<Transform>();
    public float patrolDurationWait;
    public float periodSpawn;

    [HideInInspector] public Enemy lastSpawnedEnemy;

    [HideInInspector] public UnityEvent Spawned;

    private float t;

    void Start()
    {
        Spawn();
    }

    private void Spawn()
    {
        // this messes up NavMeshAgent for some reason
        //GameObject goEnemy = Instantiate(prefabEnemy, transformParent);
        
        GameObject goEnemy = Instantiate(prefabEnemy, transformSpawn.position + Vector3.up, Quaternion.identity);
        goEnemy.transform.parent = transformParent;

        Enemy enemy = goEnemy.GetComponent<Enemy>();
        enemy.Data = data;
        Patroling patroling = enemy.GetComponent<Patroling>();
        patroling.points = patrolPoints;
        patroling.durationWait = patrolDurationWait;
        patroling.StartPatroling();

        enemy.Died.AddListener(StartTimer);

        lastSpawnedEnemy = enemy;

        Spawned.Invoke();
    }

    private void StartTimer()
    {
        Invoke("Spawn", periodSpawn);
    }
}
