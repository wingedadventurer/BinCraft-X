using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public DataItem data;
    public int amount = 1;

    private MeshFilter mf;
    private MeshRenderer mr;
    private MeshCollider mc;
    private Health health;
    private Interactable interactable;

    private void Awake()
    {
        mf = GetComponent<MeshFilter>();
        mr = GetComponent<MeshRenderer>();
        mc = GetComponent<MeshCollider>();
        health = GetComponent<Health>();

        interactable = GetComponent<Interactable>();
        interactable.Interacted.AddListener(OnInteracted);
        interactable.InteractEnter.AddListener(OnInteractEnter);
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
            mc.sharedMesh = mf.mesh;
            health.enabled = data.hp > 0;
            health.SetHPMax(data.hp, true);
        }
        else
        {
            mf.mesh = null;
            mr.material = null;
            mc.sharedMesh = null;
            health.enabled = false;
        }
    }

    public void OnInteractEnter()
    {
        interactable.interaction.SetPromptText("[E] Pick up " + data.name);
    }

    public void OnInteracted()
    {
        // TODO: add the item to inventory
        
        interactable.interaction.SetPromptText("");
        Destroy(gameObject);
    }
}
