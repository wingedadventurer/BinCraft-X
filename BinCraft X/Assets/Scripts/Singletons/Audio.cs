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
    Fireball,
    FireStart,
    FireLoop,
    ItemTake,
    Hit,
    Button,
    Jump,
    InventoryItemTake,
    InventoryItemDrop,
    InventoryOpen,
    InventoryClose,
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
    [SerializeField] private AudioClip acFireStart;
    [SerializeField] private AudioClip acFireLoop;
    [SerializeField] private AudioClip acItemTake;
    [SerializeField] private AudioClip acHit;
    [SerializeField] private AudioClip acButton;
    [SerializeField] private AudioClip acJump;
    [SerializeField] private AudioClip acInventoryItemTake;
    [SerializeField] private AudioClip acInventoryItemDrop;
    [SerializeField] private AudioClip acInventoryOpen;
    [SerializeField] private AudioClip acInventoryClose;

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

    public SFX PlaySFX(SFXID id)
    {
        AudioClip clip = null;

        switch (id)
        {
            case SFXID.Fireball: clip = acFireball; break;
            case SFXID.FireStart: clip = acFireStart; break;
            case SFXID.FireLoop: clip = acFireLoop; break;
            case SFXID.ItemTake: clip = acItemTake; break;
            case SFXID.Hit: clip = acHit; break;
            case SFXID.Button: clip = acButton; break;
            case SFXID.Jump: clip = acJump; break;
            case SFXID.InventoryItemTake: clip = acInventoryItemTake; break;
            case SFXID.InventoryItemDrop: clip = acInventoryItemDrop; break;
            case SFXID.InventoryOpen: clip = acInventoryOpen; break;
            case SFXID.InventoryClose: clip = acInventoryClose; break;
            default: break;
        }

        return PlaySFX(clip);
    }

    public SFX PlaySFX(AudioClip clip)
    {
        if (clip)
        {
            GameObject goAS = Instantiate(prefabSFX, SFXContainer.transform);
            goAS.name = "SFX " + clip.name;
            SFX sfx = goAS.GetComponent<SFX>();
            sfx.SetClip(clip);
            sfx.Play();

            return sfx;
        }

        return null;
    }

    public void PlayGameMusic()
    {
        asMusic.Stop();
        asMusic.clip = acMusicGame;
        asMusic.Play();
    }
}
