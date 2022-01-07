using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGame : MonoBehaviour
{
    public static UIGame instance;

    [SerializeField] private Text textInteractPrompt;
    [SerializeField] private Slider sliderHP;
    [SerializeField] private Text textAmmo;
    [SerializeField] private Text textCubes;

    private void Awake()
    {
        instance = this;

        SetInteractPrompt("");
        SetHealth(0, 100);
        SetAmmo(0);
        SetCubes(0, 0);
    }
    
    public void SetInteractPrompt(string text)
    {
        textInteractPrompt.text = text;
    }

    public void SetHealth(float hp, float hpMax)
    {
        sliderHP.maxValue = hpMax;
        sliderHP.value = hp;
    }

    public void SetAmmo(int amount)
    {
        textAmmo.text = amount.ToString();
    }

    public void SetCubes(int current, int needed)
    {
        textCubes.text = current + "/" + needed;
    }
}
