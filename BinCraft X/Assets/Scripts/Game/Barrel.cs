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

    private SFX sfxFireLoop;

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
                sfxFireLoop.Destroy();
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
            Audio.instance.PlaySFX(SFXID.FireStart);

            sfxFireLoop = Audio.instance.PlaySFX(SFXID.FireLoop);
            sfxFireLoop.SetPosition(transform.position);
            sfxFireLoop.SetLoop(true);
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
                    s = "[E] Add Fuel";

                }
                else
                {
                    s = "Needs Fuel";
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
