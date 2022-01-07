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
        if (dataItem)
        {
            mf.mesh = dataItem.mesh;
            mr.material = dataItem.material;
            mc.sharedMesh = mf.mesh;
            health.enabled = dataItem.hp > 0;
            health.Hp = dataItem.hp;
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
        interactable.interaction.SetPromptText("[E] Pick up " + dataItem.name);
    }

    public void OnInteracted()
    {
        // TODO: add the item to inventory
        
        interactable.interaction.SetPromptText("");
        Destroy(gameObject);
    }
}
