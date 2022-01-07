using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public float hpStart;
    public float hpMax = 999999;

    public float hp;
    public float Hp
    {
        set
        {
            hp = Mathf.Clamp(value, 0, hpMax);

            if (hp == 0 && enabled) // TODO: check if "enabled" check is necessary here
            {
                // TODO: death event
            }
        }
        get
        {
            return hp;
        }
    }

    private void Start()
    {
        Hp = hpStart;
    }
}
