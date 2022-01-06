using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public DataItem dataItem;

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
        if (dataItem)
        {
            mf.mesh = dataItem.mesh;
            mr.material = dataItem.material;
            mc.sharedMesh = mf.mesh;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
