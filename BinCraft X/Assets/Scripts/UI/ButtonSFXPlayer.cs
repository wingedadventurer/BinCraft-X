using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonSFXPlayer : MonoBehaviour
{
    void Start()
    {
        if (TryGetComponent<Button>(out Button button))
        {
            button.onClick.AddListener(PlayClickSFX);
        }
        else
        {
            Destroy(this);
        }
    }

    private void PlayClickSFX()
    {
        Audio.instance.PlaySFX(SFXID.Button);
    }
}
