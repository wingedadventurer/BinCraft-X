using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Interaction : MonoBehaviour
{
    public float detectDistanceMax = 1.5f;

    [SerializeField] private Text textInteractPrompt;

    private Interactable interactableLast;
    private int mask;

    private void Start()
    {
        // exclude Player from raycasting   
        mask = ~(1 << LayerMask.NameToLayer("Player"));
        SetPromptText("");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && interactableLast)
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
            Interactable interactable = go.GetComponent<Interactable>();
            if (interactable)
            {
                if (interactableLast != interactable)
                {
                    interactableLast = interactable;
                    interactableLast.interaction = this;
                    interactableLast.InteractEnter.Invoke();
                }
            }
            else
            {
                if (interactableLast)
                {
                    interactableLast = null;
                    SetPromptText("");
                }
            }
        }
        else
        {
            if (interactableLast)
            {
                interactableLast = null;
                SetPromptText("");
            }
        }
    }

    public void SetPromptText(string text)
    {
        textInteractPrompt.text = text;
    }
}
