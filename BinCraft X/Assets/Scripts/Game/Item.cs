using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public DataItem dataItem;
    public int amount = 1;

    private MeshFilter mf;
    private MeshRenderer mr;
    private MeshCollider mc;

    private void Awake()
    {
        mf = GetComponent<MeshFilter>();
        mr = GetComponent<MeshRenderer>();
        mc = GetComponent<MeshCollider>();
    }

    void Start()
    {
        ApplyData();
    }

    private void ApplyData()
    {
        if (dataItem)
        {
            mf.mesh = dataItem.mesh;
            mr.material = dataItem.material;
            mc.sharedMesh = mf.mesh;

            // add health component if it doesn't exist
            if (dataItem.hp > 0 && !gameObject.GetComponent<Health>())
            {
                Health health = gameObject.AddComponent<Health>();
                health.Hp = dataItem.hp;
            }
        }
        else
        {
            mf.mesh = null;
            mr.material = null;
            mc.sharedMesh = null;

            // destroy health component if it exists
            Health health = gameObject.GetComponent<Health>();
            if (health)
            {
                Destroy(health);
            }
        }
    }
}
