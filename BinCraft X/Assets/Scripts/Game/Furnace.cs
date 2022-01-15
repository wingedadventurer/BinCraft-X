using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Furnace : MonoBehaviour
{
    [SerializeField] private float durationBurn;

    [Header("Ref")]
    [SerializeField] private DataItem dataItemIceCube;
    [SerializeField] private ParticleSystem particleFire;
    [SerializeField] private ParticleSystem particleSmoke;
    [SerializeField] private GameObject goLightFire;
    [SerializeField] private GameObject goIceCube;
    [SerializeField] private Transform transformIceCubePivot;

    private Inventory inventory;
    private UIGame uiGame;
    private float tBurn;
    private bool canInteract;
    private bool interactEntered;

    [HideInInspector] public UnityEvent Finished;

    private void Awake()
    {
        particleFire.Stop();
        particleSmoke.Stop();
        goLightFire.SetActive(false);
        goIceCube.SetActive(false);
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
            transformIceCubePivot.localScale = Vector3.one * (tBurn / durationBurn);

            tBurn -= Time.deltaTime;
            if (tBurn <= 0)
            {
                // fire burnt out
                particleFire.Stop();
                particleSmoke.Stop();
                goLightFire.SetActive(false);
                goIceCube.SetActive(false);
                canInteract = true;
                Finished.Invoke();
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

        if (tBurn <= 0 && inventory.HasItem(dataItemIceCube))
        {
            // use ice cube and light the furnace
            inventory.RemoveItem(dataItemIceCube);
            tBurn = durationBurn;
            particleFire.Play();
            particleSmoke.Play();
            goLightFire.SetActive(true);
            goIceCube.SetActive(true);
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
                if (inventory.HasItem(dataItemIceCube))
                {
                    s = "[E] Add Ice Cube";

                }
                else
                {
                    s = "Needs Ice Cubes";
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
