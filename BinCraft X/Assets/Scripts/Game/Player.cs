using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    [SerializeField] private float healthDecreaseRate;
    [SerializeField] private float healthDecreaseRatePerIceCube;

    [Header("Ref")]
    [SerializeField] private DataItem itemAmmo;
    [SerializeField] private DataItem itemIceCube;
    [SerializeField] private Health health;

    [HideInInspector] public UnityEvent Died;

    private int countIceCubes;

    private Inventory inventory;
    private UIGame uiGame;

    private void Awake()
    {
        health = GetComponent<Health>();
    }

    void Start()
    {
        inventory = Inventory.instance;
        uiGame = UIGame.instance;

        health.Changed.AddListener(OnHealthChanged);
        health.Depleted.AddListener(OnHealthDepleted);
        inventory.Changed.AddListener(OnInventoryChanged);
        uiGame.SetHealth(health.GetHP(), health.GetHPMax());

        UpdateHealthUI();
        UpdateAmmoUI();
    }

    private void Update()
    {
        health.ChangeBy(- (healthDecreaseRate + healthDecreaseRatePerIceCube * countIceCubes) * Time.deltaTime);
    }

    private void OnHealthChanged()
    {
        UpdateHealthUI();
    }

    private void OnHealthDepleted()
    {
        Died.Invoke();
    }

    private void OnInventoryChanged()
    {
        UpdateAmmoUI();
        countIceCubes = inventory.GetItemCount(itemIceCube);
    }

    private void UpdateHealthUI()
    {
        uiGame.SetHealth(health.GetHP(), health.GetHPMax());
    }

    private void UpdateAmmoUI()
    {
        uiGame.SetAmmo(inventory.GetItemCount(itemAmmo));
    }
}
