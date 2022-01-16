using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private GameObject prefabParticlesTrail;
    [SerializeField] private GameObject prefabParticlesExplode;

    [HideInInspector] public float damage;

    private GameObject goParticlesTrail;

    //public void SetEnabled(bool value)
    //{

    //}

    private void OnEnable()
    {
        goParticlesTrail = Instantiate(prefabParticlesTrail, transform);
    }

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

        goParticlesTrail.GetComponent<ParticleSystem>().Stop();
        goParticlesTrail.transform.SetParent(null);

        gameObject.SetActive(false);

        Instantiate(prefabParticlesExplode, transform.position, Quaternion.identity);
    }
}
