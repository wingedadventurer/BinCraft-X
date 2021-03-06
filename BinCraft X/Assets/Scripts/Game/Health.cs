using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Health : MonoBehaviour
{
    [SerializeField] private float hp;
    [SerializeField] private float hpMax = 100;

    public UnityEvent Changed;
    public UnityEvent Depleted;

    private void Start()
    {
        hp = hpMax;
    }

    public void SetHP(float value)
    {
        hp = value;
    }

    public void SetHPMax(float value, bool updateHP = false)
    {
        hpMax = Mathf.Max(0, value);
        hp = updateHP ? hpMax : Mathf.Min(hp, hpMax);
    }

    public float GetHP() { return hp; }
    public float GetHPMax() { return hpMax; }

    public float GetFactor()
    {
        return hp / hpMax;
    }

    public void ChangeBy(float amount)
    {
        if (!enabled) { return; }

        hp = Mathf.Clamp(hp + amount, 0, hpMax);

        Changed.Invoke();

        if (hp == 0)
        {
            Depleted.Invoke();
        }
    }
}
