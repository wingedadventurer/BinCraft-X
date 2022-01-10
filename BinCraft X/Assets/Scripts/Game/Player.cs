using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    [SerializeField] private DataItem itemAmmo;

    private Health health;

    [HideInInspector] public UnityEvent Died;

    private void Awake()
    {
        health = GetComponent<Health>();
    }

    void Start()
    {
        health.Changed.AddListener(UpdateHealthUI);
        health.Depleted.AddListener(OnHealthDepleted);
        Inventory.instance.Changed.AddListener(UpdateAmmoUI);
        UIGame.instance.SetHealth(health.GetHP(), health.GetHPMax());

        UpdateHealthUI();
        UpdateAmmoUI();
    }

    private void OnHealthDepleted()
    {
        Died.Invoke();
    }

    private void UpdateHealthUI()
    {
        UIGame.instance.SetHealth(health.GetHP(), health.GetHPMax());
    }

    private void UpdateAmmoUI()
    {
        UIGame.instance.SetAmmo(Inventory.instance.GetItemCount(itemAmmo));
    }
}
