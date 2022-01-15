using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Enemy : MonoBehaviour
{
    public DataEnemy Data
    {
        set
        {
            data = value;
            ApplyData();
        }
        get
        {
            return data;
        }
    }

    [Header("Ref")]
    [SerializeField] private MeshRenderer meshRendererHealthGradient;
    [SerializeField] private DataEnemy data;
    [SerializeField] private Health health;
    [SerializeField] private Gradient gradientHealth;

    [HideInInspector] public UnityEvent Died;

    private void Awake()
    {
        Data = data;

        health.Changed.AddListener(OnHealthChanged);
        health.Depleted.AddListener(OnHealthDepleted);
    }

    private void Start()
    {
        Data = data;
        UpdateColorHealth();
    }

    private void ApplyData()
    {
        if (data)
        {
            health.SetHPMax(data.hp, true);
        }
    }

    private void UpdateColorHealth()
    {
        meshRendererHealthGradient.material.color = gradientHealth.Evaluate(health.GetFactor());
    }

    private void OnHealthChanged()
    {
        UpdateColorHealth();
    }

    private void OnHealthDepleted()
    {
        Died.Invoke();
        Destroy(gameObject);
    }
}
