using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthConstantModifier : MonoBehaviour
{
    public float rate;

    private void OnTriggerStay(Collider other)
    {
        Health health = other.gameObject.GetComponent<Health>();
        if (health)
        {
            health.ChangeBy(rate * Time.deltaTime);
        }
    }
}
