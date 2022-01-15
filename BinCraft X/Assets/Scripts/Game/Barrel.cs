using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barrel : MonoBehaviour
{
    [SerializeField] private float durationBurn;
    [SerializeField] private DataItem dataItemFuel;

    [SerializeField] private ParticleSystem particleFire;

    private Inventory inventory;
    private Interactable interactable;
    private UIGame uiGame;

    public GameObject areaHeal;
    public GameObject lightFire;

    private float tBurn;

    private void Awake()
    {
        interactable = GetComponent<Interactable>();
        interactable.Interacted.AddListener(OnInteracted);
        interactable.InteractEnter.AddListener(OnInteractEnter);
        particleFire.Stop();
        areaHeal.SetActive(false);
        lightFire.SetActive(false);
    }

    private void Start()
    {
        inventory = Inventory.instance;
        uiGame = UIGame.instance;
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
                UpdateInteractionText();
                areaHeal.SetActive(false);
                lightFire.SetActive(false);
            }
        }
    }

    public void OnInteractEnter()
    {
        UpdateInteractionText();
    }

    public void OnInteracted()
    {
        if (tBurn <= 0 && inventory.HasItem(dataItemFuel))
        {
            // use fuel and light the fire
            inventory.RemoveItem(dataItemFuel);
            UpdateInteractionText();
            tBurn = durationBurn;
            particleFire.Play();
            areaHeal.SetActive(true);
            lightFire.SetActive(true);
        }
    }

    private void UpdateInteractionText()
    {
        string s;
        if (tBurn > 0)
        {
            s = "";
        }
        else if (inventory.HasItem(dataItemFuel))
        {
            s = "[E] Add fuel";
            
        }
        else
        {
            s = "Missing fuel";
        }
        uiGame.SetInteractPrompt(s);
    }
}
