using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : SceneLoader
{
    public string nextScene;
    
    public void StartGame()
    {
        StartLoading(nextScene);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}