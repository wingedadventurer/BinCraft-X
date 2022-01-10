using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIInventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] Image image;
    [SerializeField] Text text;

    [HideInInspector] public int x;
    [HideInInspector] public int y;

    [HideInInspector] public UnityEvent Entered;
    [HideInInspector] public UnityEvent Exited;
    [HideInInspector] public UnityEvent Pressed;
    [HideInInspector] public UnityEvent Released;
    [HideInInspector] public UnityEvent RightPressed;
    [HideInInspector] public UnityEvent RightReleased;

    private void Awake()
    {
        SetSprite(null);
        SetAmount(0);
    }

    public void SetSprite(Sprite sprite)
    {
        image.sprite = sprite;

        // null sprite equals to white image so in that case we disable it
        image.enabled = sprite;
    }

    public void SetAmount(int amount)
    {
        text.text = amount > 0 ? amount.ToString() : "";
    }

    #region POINTER STUFF
    public void OnPointerEnter(PointerEventData eventData)
    {
        Entered.Invoke();
        //Debug.Log(x + " " + y + " " + "enter");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Exited.Invoke();
        //Debug.Log(x + " " + y + " " + "exit");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Pressed.Invoke();
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            RightPressed.Invoke();
        }
        //Debug.Log(x + " " + y + " " + "down with " + eventData.button);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            Released.Invoke();
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            RightReleased.Invoke();
        }
        //Debug.Log(x + " " + y + " " + "up with " + eventData.button);
    }
    #endregion
}
