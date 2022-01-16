using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject panelMain;
    public GameObject panelCredits;
    public GameObject panelLoading;
    public Slider slider;

    private void Start()
    {
        panelMain.SetActive(true);
        panelCredits.SetActive(false);
        panelLoading.SetActive(false);
        Audio.instance.PlaySong(SongID.Menu);
    }

    public void StartGame()
    {
        StartCoroutine(LoadGameScene());
    }

    public void QuitGame()
    {
        Debug.Log("quit");
        Application.Quit();
    }

    private IEnumerator LoadGameScene()
    {
        panelMain.SetActive(false);
        panelLoading.SetActive(true);
        slider.value = 0;

        AsyncOperation load = SceneManager.LoadSceneAsync(1);

        while (!load.isDone)
        {
            slider.value = load.progress;
            yield return null;
        }
    }
}
