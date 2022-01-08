using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public DataEnemy data;

    public Gradient gradientHealth;

    private MeshFilter mf;
    private MeshRenderer mr;
    private Health health;

    private void Awake()
    {
        mf = GetComponent<MeshFilter>();
        mr = GetComponent<MeshRenderer>();
        health = GetComponent<Health>();

        health.Changed.AddListener(OnHealthChanged);
        health.Depleted.AddListener(OnHealthDepleted);
    }

    private void Start()
    {
        ApplyData();
        UpdateColorHealth();
    }

    public void SetActive(bool value)
    {

    }

    private void ApplyData()
    {
        if (data)
        {
            mf.mesh = data.mesh;
            mr.material = data.material;
            health.SetHPMax(data.hp, true);
        }
        else
        {
            mf.mesh = null;
            mr.material = null;
        }
    }

    private void UpdateColorHealth()
    {
        mr.material.color = gradientHealth.Evaluate(health.GetFactor());
    }

    private void OnHealthChanged()
    {
        UpdateColorHealth();
    }

    private void OnHealthDepleted()
    {
        Destroy(gameObject);
    }
}
