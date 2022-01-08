using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private DataItem itemAmmo;
    [SerializeField] private DataItem itemGreenCube;
    [SerializeField] private DataItem itemYellowCube;

    private Health health;

    private void Awake()
    {
        health = GetComponent<Health>();
    }

    void Start()
    {
        health.Changed.AddListener(UpdateHealthUI);
        health.Depleted.AddListener(OnHealthDepleted);
        Inventory.instance.Changed.AddListener(UpdateAmmoAndCubesUI);
        UIGame.instance.SetHealth(health.GetHP(), health.GetHPMax());

        UpdateHealthUI();
        UpdateAmmoAndCubesUI();
    }

    // TODO: move this to Game
    private void OnHealthDepleted()
    {
        Game.instance.OnGameLose();
    }

    private void UpdateHealthUI()
    {
        UIGame.instance.SetHealth(health.GetHP(), health.GetHPMax());
    }

    private void UpdateAmmoAndCubesUI()
    {
        // ammo
        UIGame.instance.SetAmmo(Inventory.instance.GetItemCount(itemAmmo));

        // cubes
        int countCubes = Inventory.instance.GetItemCount(itemGreenCube) + Inventory.instance.GetItemCount(itemYellowCube);
        UIGame.instance.SetCubesCurrent(countCubes);
    }
}
