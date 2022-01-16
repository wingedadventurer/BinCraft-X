using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGame : MonoBehaviour
{
    public static UIGame instance;

    [SerializeField] private GameObject panel;
    [SerializeField] private Gradient gradientHealth;
    [SerializeField] private Text textInteractPrompt;
    [SerializeField] private Slider sliderHP;
    [SerializeField] private Image imageHPBar;
    [SerializeField] private Text textAmmo;
    [SerializeField] private Text textCubes;
    [SerializeField] private Text textEnemies;
    [SerializeField] private GameObject goCrosshair;

    private void Awake()
    {
        instance = this;

        SetInteractPrompt("");
        SetHealth(0, 100);
        SetAmmo(0);
        SetCubesRemaining(0);
        SetEnemiesRemaining(0);
    }
    
    public void SetPanelVisible(bool value)
    {
        panel.SetActive(value);
    }

    public void SetCrosshairVisible(bool value)
    {
        goCrosshair.SetActive(value);
    }

    public void SetInteractPrompt(string text)
    {
        textInteractPrompt.text = text;
    }

    public void SetHealth(float hp, float hpMax)
    {
        sliderHP.maxValue = hpMax;
        sliderHP.value = hp;
        imageHPBar.color = gradientHealth.Evaluate(hp / hpMax);
    }

    public void SetAmmo(int amount)
    {
        textAmmo.text = amount.ToString();
    }

    public void SetCubesRemaining(int amount)
    {
        textCubes.text = amount.ToString();
    }

    public void SetEnemiesRemaining(int amount)
    {
        textEnemies.text = amount.ToString();
    }
}
