using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Item : MonoBehaviour
{
    public DataItem data;
    public int amount = 1;

    private MeshFilter mf;
    private MeshRenderer mr;
    private MeshCollider mc;
    private Health health;
    private Interactable interactable;

    bool picked;
    private Vector3 posPickStart;
    private Transform transformPickEnd;
    private float fPick = 0.0f;

    private void Awake()
    {
        mf = GetComponent<MeshFilter>();
        mr = GetComponent<MeshRenderer>();
        mc = GetComponent<MeshCollider>();
        health = GetComponent<Health>();

        health.Depleted.AddListener(OnHealthDepleted);

        interactable = GetComponent<Interactable>();
        interactable.Interacted.AddListener(OnInteracted);
        interactable.InteractEnter.AddListener(OnInteractEnter);
    }

    private void Start()
    {
        ApplyData();
    }

    private void Update()
    {
        if (picked)
        {
            transform.position = Vector3.Lerp(posPickStart, transformPickEnd.position, fPick);
        }
    }

    public void ApplyData()
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
        UpdateInteractionText();
    }

    public void OnInteracted()
    {

        amount = Inventory.instance.AddItem(data, amount);
        if (amount == 0)
        {
            UIGame.instance.SetInteractPrompt("");

            GetComponent<Rigidbody>().isKinematic = true;
            mc.enabled = false;
            health.enabled = false;
            interactable.enabled = false;

            posPickStart = transform.position;
            transformPickEnd = FindObjectOfType<Player>().transform;

            DOTween.To(() => fPick, x => fPick = x, 1.0f, 0.15f).SetEase(Ease.InCubic).OnComplete(OnPickTweenComplete);

            picked = true;
        }
        else
        {
            UpdateInteractionText();
        }
    }

    private void OnHealthDepleted()
    {
        Destroy(gameObject);
    }

    private void UpdateInteractionText()
    {
        string s = "[E] Pick up " + data.name;
        if (amount > 1)
        {
            s += " (" + amount + ")";
        }
        UIGame.instance.SetInteractPrompt(s);
    }

    private void OnPickTweenComplete()
    {
        Destroy(gameObject);
    }
}
