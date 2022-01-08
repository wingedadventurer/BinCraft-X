using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInventorySlot : MonoBehaviour
{
    [HideInInspector] public int x;
    [HideInInspector] public int y;

    [SerializeField] Image image;
    [SerializeField] Text text;

    private void Start()
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
}
