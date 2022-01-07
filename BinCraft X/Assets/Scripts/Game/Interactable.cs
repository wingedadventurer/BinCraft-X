using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{
    public Interaction interaction;

    public UnityEvent InteractEnter;
    public UnityEvent Interacted;
}
