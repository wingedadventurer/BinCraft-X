using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Well : MonoBehaviour
{
    [SerializeField] private float periodSpawn;
    [SerializeField] private DataEnemy dataEnemy;

    [Header("Patroling")]
    [SerializeField] private float patrolDurationWait;
    [SerializeField] private List<Transform> patrolPoints = new List<Transform>();

    [Header("Parent container for spawned enemies")]
    [SerializeField] private Transform transformParent;

    [Header("Ref")]
    [SerializeField] private DataItem dataItemLid;
    [SerializeField] private GameObject prefabEnemy;
    [SerializeField] private GameObject goLid;
    [SerializeField] private Transform transformSpawn;

    [HideInInspector] public Enemy lastSpawnedEnemy;

    [HideInInspector] public UnityEvent Spawned;

    private bool closed;
    private Inventory inventory;
    private UIGame uiGame;

    void Start()
    {
        inventory = Inventory.instance;
        uiGame = UIGame.instance;

        foreach (Interactable interactable in GetComponentsInChildren<Interactable>())
        {
            interactable.Interacted.AddListener(OnInteracted);
            interactable.InteractEntered.AddListener(OnInteractEnter);
            interactable.InteractExited.AddListener(OnInteractExit);
        }

        goLid.SetActive(false);

        SpawnEnemy();
    }

    private void SpawnEnemy()
    {
        if (closed) { return; }

        //GameObject goEnemy = Instantiate(prefabEnemy, transformParent); // this messes up NavMeshAgent for some reason
        GameObject goEnemy = Instantiate(prefabEnemy, transformSpawn.position + Vector3.up, Quaternion.identity);

        goEnemy.transform.parent = transformParent;

        Enemy enemy = goEnemy.GetComponent<Enemy>();
        enemy.Data = dataEnemy;
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
        Invoke("SpawnEnemy", periodSpawn);
    }

    public void OnInteractEnter()
    {
        UpdateInteractionText();
    }

    public void OnInteractExit()
    {
        UpdateInteractionText();
    }

    public void OnInteracted()
    {
        if (closed) { return; }

        if (inventory.HasItem(dataItemLid))
        {
            inventory.RemoveItem(dataItemLid);
            closed = true;
            goLid.SetActive(true);
            UpdateInteractionText();
            CancelInvoke("SpawnEnemy");
        }
    }

    private void UpdateInteractionText()
    {
        string s;

        if (!closed)
        {
            if (inventory.HasItem(dataItemLid))
            {
                s = "[E] Close";

            }
            else
            {
                s = "Needs Well Lid";
            }
        }
        else
        {
            s = "";
        }

        uiGame.SetInteractPrompt(s);
    }
}
