using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public String SceneName;
    public DialogueCharacter[] Characters;

    private Dictionary<String, DialogueCharacter> _characters;
    // Start is called before the first frame update
    void Start()
    {
        _characters = Characters.ToDictionary(c => c.Name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
