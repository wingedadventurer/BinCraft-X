using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public DataEnemy data;

    private MeshFilter mf;
    private MeshRenderer mr;
    private Health health;

    private void Awake()
    {
        mf = GetComponent<MeshFilter>();
        mr = GetComponent<MeshRenderer>();
        health = GetComponent<Health>();
    }

    void Start()
    {
        ApplyData();
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
}
