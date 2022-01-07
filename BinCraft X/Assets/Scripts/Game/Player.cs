using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Game game;
    private Health health;

    private void Awake()
    {
        game = FindObjectOfType<Game>();
        health = GetComponent<Health>();
    }

    void Start()
    {
        health.Depleted.AddListener(OnHealthDepleted);
    }

    private void OnHealthDepleted()
    {
        game.OnGameLose();
    }
}
