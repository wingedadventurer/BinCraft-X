using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    [HideInInspector] public Interaction interaction;

    [HideInInspector] public UnityEvent InteractEntered;
    [HideInInspector] public UnityEvent InteractExited;
    [HideInInspector] public UnityEvent Interacted;
}
