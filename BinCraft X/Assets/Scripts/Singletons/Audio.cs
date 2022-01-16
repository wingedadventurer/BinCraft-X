using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SongID
{
    Menu,
    Game,
}

public enum SFXID
{
    Fireball
}

public class Audio : MonoBehaviour
{
    [SerializeField] private AudioSource asMusic;
    [SerializeField] private Transform SFXContainer;
    [SerializeField] private GameObject prefabSFX;

    [Header("Songs")]
    [SerializeField] private AudioClip acMusicMenu;
    [SerializeField] private AudioClip acMusicGame;

    [Header("SFXs")]
    [SerializeField] private AudioClip acFireball;

    public static Audio instance;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

    }

    public void PlaySong(SongID id)
    {
        AudioClip clip = null;
        
        switch (id)
        {
            case SongID.Menu: clip = acMusicMenu; break;
            case SongID.Game: clip = acMusicGame; break;
            default: break;
        }

        if (clip)
        {
            asMusic.Stop();
            asMusic.clip = clip;
            asMusic.Play();
        }
    }

    public void PlaySFX(SFXID id)
    {
        AudioClip clip = null;

        switch (id)
        {
            case SFXID.Fireball: clip = acFireball; break;
            default: break;
        }

        if (clip)
        {
            GameObject goAS = Instantiate(prefabSFX, SFXContainer.transform);
            goAS.name = "SFX " + clip.name;
            SFX sfx = goAS.GetComponent<SFX>();
            sfx.SetClip(clip);
            sfx.Play();
        }
    }

    public void PlayGameMusic()
    {
        asMusic.Stop();
        asMusic.clip = acMusicGame;
        asMusic.Play();
    }
}
