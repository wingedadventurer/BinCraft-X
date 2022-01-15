using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour
{
    [SerializeField] private float durationBurn;

    [Header("Ref")]
    [SerializeField] private GameObject areaHeal;
    [SerializeField] private GameObject lightFire;
    [SerializeField] private DataItem dataItemFuel;
    [SerializeField] private ParticleSystem particleFire;

    private Inventory inventory;
    private UIGame uiGame;
    private float tBurn;
    private bool canInteract;
    private bool interactEntered;

    private void Awake()
    {
        particleFire.Stop();
        areaHeal.SetActive(false);
        lightFire.SetActive(false);
    }

    private void Start()
    {
        inventory = Inventory.instance;
        uiGame = UIGame.instance;

        foreach (Interactable interactable in GetComponentsInChildren<Interactable>())
        {
            interactable.Interacted.AddListener(OnInteracted);
            interactable.InteractEntered.AddListener(OnInteractEnter);
            interactable.InteractExited.AddListener(OnInteractExit);
        }

        canInteract = true;
    }

    private void Update()
    {
        if (tBurn > 0)
        {
            tBurn -= Time.deltaTime;
            if (tBurn <= 0)
            {
                // fire burnt out
                particleFire.Stop();
                areaHeal.SetActive(false);
                lightFire.SetActive(false);
                canInteract = true;
                UpdateInteractionText();
            }
        }
    }

    public void OnInteractEnter()
    {
        interactEntered = true;
        UpdateInteractionText();
    }

    public void OnInteractExit()
    {
        interactEntered = false;
        UpdateInteractionText();
    }

    public void OnInteracted()
    {
        if (!canInteract) { return; }
        if (tBurn <= 0 && inventory.HasItem(dataItemFuel))
        {
            // use fuel and light the fire
            inventory.RemoveItem(dataItemFuel);
            tBurn = durationBurn;
            particleFire.Play();
            areaHeal.SetActive(true);
            lightFire.SetActive(true);
            canInteract = false;
            UpdateInteractionText();
        }
    }

    private void UpdateInteractionText()
    {
        string s;

        if (canInteract)
        {
            if (interactEntered)
            {
                if (inventory.HasItem(dataItemFuel))
                {
                    s = "[E] Add fuel";

                }
                else
                {
                    s = "Needs fuel";
                }
            }
            else
            {
                s = "";
            }
        }
        else
        {
            s = "";
        }

        uiGame.SetInteractPrompt(s);
    }
}
