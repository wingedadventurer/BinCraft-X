using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    // change if 
    private const int SCENE_BUILD_INDEX_GAME = 1;

    public void StartGame()
    {
        SceneManager.LoadScene("SceneGame", LoadSceneMode.Single);
    }

    public void QuitGame()
    {
        Debug.Log("quit");
        Application.Quit();
    }
}
