using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    private int hp;
    public int Hp
    {
        set
        {
            hp = Mathf.Clamp(value, 0, int.MaxValue);

            if (hp == 0 && enabled) // TODO: check if "enabled" check is necessary here
            {
                // TODO: death event
            }
        }
    }
}
