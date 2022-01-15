using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Songs
{
    Menu,
    Game,
}

public class Audio : MonoBehaviour
{
    [SerializeField] private AudioClip acMusicMenu;
    [SerializeField] private AudioClip acMusicGame;
    [SerializeField] private AudioSource asMusic;

    public static Audio instance;

    private void Awake()
    {
        if (!instance)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }

    }

    public void PlaySong(Songs song)
    {
        AudioClip clip = null;
        
        switch (song)
        {
            case Songs.Menu:
                clip = acMusicMenu;
                break;
            case Songs.Game:
                clip = acMusicGame;
                break;
            default:
                break;
        }
        if (clip)
        {
            asMusic.Stop();
            asMusic.clip = clip;
            asMusic.Play();
        }
    }

    public void PlayGameMusic()
    {
        asMusic.Stop();
        asMusic.clip = acMusicGame;
        asMusic.Play();
    }
}
