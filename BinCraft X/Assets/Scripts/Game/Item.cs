using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(Health))]
public class Item : MonoBehaviour
{
    [SerializeField] private DataItem data;
    public DataItem Data
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

    public int amount = 1;

    private Rigidbody rigidBody;
    private Health health;
    private GameObject model;
    private Interactable[] interactables;

    private bool picked;
    private bool canInteract;
    private Vector3 posPickStart;
    private Transform transformPickEnd;
    private float fPick = 0.0f;

    private void Awake()
    {
        rigidBody = GetComponent<Rigidbody>();

        health = GetComponent<Health>();
        health.Depleted.AddListener(OnHealthDepleted);

        foreach (Interactable interactable in GetComponentsInChildren<Interactable>())
        {
            interactable.Interacted.AddListener(OnInteracted);
            interactable.InteractEntered.AddListener(OnInteractEnter);
        }
    }

    private void Start()
    {
        Data = data;
        canInteract = true;
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
            if (health)
            {
                health.enabled = data.hp > 0;
                health.SetHPMax(data.hp, true);
            }
        }
        else
        {
            if (health)
            {
                health.enabled = false;
            }
        }
    }

    public void OnInteractEnter()
    {
        if (!canInteract) { return; }

        UpdateInteractionText();
    }

    public void OnInteracted()
    {
        if (!canInteract) { return; }

        amount = Inventory.instance.AddItem(data, amount);
        if (amount == 0)
        {
            //UIGame.instance.SetInteractPrompt("");

            foreach (Collider collider in GetComponentsInChildren<Collider>())
            {
                collider.enabled = false;
            }

            canInteract = false;

            rigidBody.isKinematic = true;
            health.enabled = false;

            posPickStart = transform.position;
            transformPickEnd = FindObjectOfType<Player>().transform;

            DOTween.To(() => fPick, x => fPick = x, 1.0f, 0.15f).SetEase(Ease.InCubic).OnComplete(OnPickTweenComplete);

            Audio.instance.PlaySFX(SFXID.ItemTake).SetVolume(0.8f);

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
