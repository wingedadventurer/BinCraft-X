using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interaction : MonoBehaviour
{
    public float detectDistanceMax = 1.5f;
    public Text textInteractPrompt;

    private Interactable interactableLast;
    private int mask;

    private void Start()
    {
        // exclude Player from raycasting   
        mask = ~(1 << LayerMask.NameToLayer("Player"));

        UpdatePromptText();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && interactableLast)
        {
            // TODO: interact
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
            Interactable interactable = go.GetComponent<Interactable>();
            if (interactable)
            {
                if (interactableLast != interactable)
                {
                    interactableLast = interactable;
                    UpdatePromptText();
                }
            }
            else
            {
                if (interactableLast)
                {
                    interactableLast = null;
                    UpdatePromptText();
                }
            }
        }
        else
        {
            if (interactableLast)
            {
                interactableLast = null;
                UpdatePromptText();
            }
        }
    }

    private void UpdatePromptText()
    {
        if (interactableLast)
        {
            textInteractPrompt.text = "[E] Pick up NAME_OF_THE_OBJECT";
        }
        else
        {
            textInteractPrompt.text = "";
        }
    }
}
