using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuScript : SceneLoader
{
    public string fightScene;
    
    public void StartGame()
    {
        StartLoading(fightScene);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}