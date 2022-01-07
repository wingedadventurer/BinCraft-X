using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [HideInInspector] public float damage;

    private void OnCollisionEnter(Collision collision)
    {
        GameObject go = collision.gameObject;
        if (go.TryGetComponent<Health>(out Health health))
        {
            health.ChangeBy(-damage);
        }

        gameObject.SetActive(false);
    }
}
