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

    [SerializeField] private float delayRandomSFXMin;
    [SerializeField] private float delayRandomSFXMax;

    [Header("Ref")]
    [SerializeField] private MeshRenderer[] meshRenderersHealthGradient;
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

        if (data.sfxRandom.Length > 0)
        {
            Invoke("PlayRandomSFX", Random.Range(delayRandomSFXMin, delayRandomSFXMax));
        }
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
        foreach (MeshRenderer mr in meshRenderersHealthGradient)
        {
            foreach (Material material in mr.materials)
            {
                if (material.name.StartsWith("Penguin White"))
                {
                    material.color = gradientHealth.Evaluate(health.GetFactor());
                }
            }
        }
    }

    private void OnHealthChanged()
    {
        UpdateColorHealth();
        PlayHurtSFX();
    }

    private void OnHealthDepleted()
    {
        Died.Invoke();
        Destroy(gameObject);
    }

    private void PlayRandomSFX()
    {
        AudioClip ac = data.sfxRandom[Random.Range(0, data.sfxRandom.Length)];
        SFX sfx = Audio.instance.PlaySFX(ac);
        sfx.SetPosition(transform.position);
        sfx.SetMaxDistance(25);
        Invoke("PlayRandomSFX", Random.Range(delayRandomSFXMin, delayRandomSFXMax));
    }

    private void PlayHurtSFX()
    {
        if (data.sfxRandom.Length > 0)
        {
            AudioClip ac = data.sfxHurt[Random.Range(0, data.sfxRandom.Length)];
            SFX sfx = Audio.instance.PlaySFX(ac);
            sfx.SetPosition(transform.position);
            sfx.SetMaxDistance(25);
        }
    }
}
