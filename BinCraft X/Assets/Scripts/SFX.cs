using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFX : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;

    private bool played;

    public void SetClip(AudioClip ac)
    {
        audioSource.clip = ac;
    }

    public void Play()
    {
        played = true;
        audioSource.Play();
    }

    private void Update()
    {
        if (played && !audioSource.isPlaying)
        {
            Destroy(gameObject);
        }
    }

    public void SetLoop(bool value)
    {
        audioSource.loop = value;
    }

    public void SetVolume(float value)
    {
        audioSource.volume = value;
    }

    public void SetPosition(Vector3 position)
    {
        transform.position = position;
        audioSource.spatialBlend = 1;
    }

    public void SetMaxDistance(float value)
    {
        audioSource.maxDistance = value;
    }

    public void Destroy()
    {
        Destroy(gameObject);
    }
}
