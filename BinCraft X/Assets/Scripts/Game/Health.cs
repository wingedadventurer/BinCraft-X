using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    private float hp;
    public float Hp
    {
        set
        {
            hp = Mathf.Clamp(value, 0, float.MaxValue);

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
}
