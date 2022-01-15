using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject panelMain, panelLoading;
    public Slider slider;

    private void Start()
    {
        panelMain.SetActive(true);
        panelLoading.SetActive(false);
        Audio.instance.PlaySong(Songs.Menu);
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
