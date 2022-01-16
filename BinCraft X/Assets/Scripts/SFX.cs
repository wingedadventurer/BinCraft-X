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
}
