using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextScene : SceneLoader
{
    public string NewSceneName;
    // Start is called before the first frame update
    void Start()
    {
        StartLoading(NewSceneName);
    }
}
