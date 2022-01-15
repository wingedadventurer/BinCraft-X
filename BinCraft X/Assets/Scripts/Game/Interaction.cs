using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interaction : MonoBehaviour
{
    public float detectDistanceMax = 1.5f;

    public bool controllable;

    private Interactable interactableLast;
    private int mask;

    private void Start()
    {
        // exclude Player from raycasting
        mask = ~(1 << LayerMask.NameToLayer("Player"));
        UIGame.instance.SetInteractPrompt("");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && Game.instance.playerControllable && interactableLast)
        {
            interactableLast.Interacted.Invoke();
        }
    }
    
    private void FixedUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        RaycastHit raycastHit;
        Physics.Raycast(ray, out raycastHit, detectDistanceMax, mask);

        if (raycastHit.collider)
        {
            GameObject go = raycastHit.collider.gameObject;
            
            if (go.TryGetComponent<Item>(out Item item))
            {
                Interactable interactable = go.GetComponent<Interactable>();

                if (interactableLast != interactable)
                {
                    interactableLast = interactable;
                    interactableLast.interaction = this;
                    interactableLast.InteractEnter.Invoke();
                }
            }
            else if (go.TryGetComponent<Barrel>(out Barrel barrel))
            {
                Interactable interactable = go.GetComponent<Interactable>();

                if (interactableLast != interactable)
                {
                    interactableLast = interactable;
                    interactableLast.interaction = this;
                    interactableLast.InteractEnter.Invoke();
                }
            }
            else
            {
                ClearInteractableLast();
            }
        }
        else
        {
            ClearInteractableLast();
        }
    }

    private void ClearInteractableLast()
    {
        if (interactableLast)
        {
            interactableLast = null;
            UIGame.instance.SetInteractPrompt("");
        }
    }
}
