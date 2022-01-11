using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[ExecuteInEditMode]
public class Enemy : MonoBehaviour
{
    [SerializeField] private DataEnemy data;
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

    public Gradient gradientHealth;

    private MeshFilter mf;
    private MeshRenderer mr;
    private Health health;

    [HideInInspector] public UnityEvent Died;

    private void Awake()
    {
        mf = GetComponent<MeshFilter>();
        mr = GetComponent<MeshRenderer>();
        Data = data;

        Debug.Log(gameObject.name);

        if (Application.isPlaying)
        {
            health = GetComponent<Health>();
            health.Changed.AddListener(OnHealthChanged);
            health.Depleted.AddListener(OnHealthDepleted);
        }
    }

    private void Start()
    {
        Data = data;
        if (Application.isPlaying)
        {
            UpdateColorHealth();
        }
    }

    private void ApplyData()
    {
        if (data)
        {
            if (mf) { mf.sharedMesh = data.mesh; }
            if (mr) { mr.material = data.material; }
            if (Application.isPlaying && health)
            {
                health.SetHPMax(data.hp, true);
            }
        }
        else
        {
            if (mf) { mf.sharedMesh = Resources.GetBuiltinResource<Mesh>("Cylinder.fbx"); }
            if (mr) { mr.material = null; }
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
        Died.Invoke();
        Destroy(gameObject);
    }

    private void OnValidate()
    {
        Data = data;
    }
}
