using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [HideInInspector] public float damage;

    private void OnCollisionEnter(Collision collision)
    {
        // necessary due to multiple calls of this method
        if (!gameObject.activeSelf) { return; }

        GameObject go = collision.gameObject;
        if (go.TryGetComponent<Health>(out Health health))
        {
            health.ChangeBy(-damage);
        }

        SFX sfx = Audio.instance.PlaySFX(SFXID.Hit);
        sfx.SetPosition(transform.position);
        sfx.SetMaxDistance(20);

        gameObject.SetActive(false);
    }
}
