using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class DialogueLevelManager : MonoBehaviour
{
    public const string DialogueRailwaySceneName = "RailwayScene";
    public const string DialogueFightSceneName = "TrainScene";
    public const string DialogueIntroSceneName = "IntroScene";

    public Dictionary<string, string[]> DialogueSceneOrder = new Dictionary<string, string[]>
    {
        {
            DialogueRailwaySceneName, new[]
            {
                "intro",
                "buyTickets",
                "shitHappens",
                "timeOut",
                "onTrain"
            }
        },
        
        {
            DialogueFightSceneName, new[]
            {
                "intro",
                "end",
            }
        }
    };

    public string DialoguePath = "Assets/Materials/Text";

    public
        Dictionary<string, Dictionary<string, DialogueElement[]>> DialogueSystem =
            new Dictionary<string, Dictionary<string, DialogueElement[]>>();

    public DialogueManager dialogueManager;
    public int UIManagerDialogue = -1;

    void Start()
    {
        var directories = string.Join("|", new DirectoryInfo(".").GetDirectories().Select(d => d.Name).ToArray());
        Debug.Log($"{directories}");
        foreach (var fileInfo in new DirectoryInfo(DialoguePath).GetFiles("*.json", SearchOption.AllDirectories))
        {
            var parts = fileInfo.Name.Split('.');
            var sceneName = parts[0];
            var dialogueName = parts[1];
            if (!DialogueSystem.TryGetValue(sceneName, out var dialoguesInScene))
            {
                dialoguesInScene = new Dictionary<string, DialogueElement[]>();
                DialogueSystem[sceneName] = dialoguesInScene;
            }
                

            var content = File.ReadAllText(fileInfo.FullName);
            var dialogues = JsonUtility.FromJson<Dialogue>(content);
            dialoguesInScene[dialogueName] = dialogues.dialogue;

            Debug.Log($"Process successful: {sceneName} {dialogueName}");

        }

        dialogueManager.Trigger(TriggerAction.Activate);
    }

    [Serializable]
    public class Dialogue
    {
        public DialogueElement[] dialogue;
    }

    [Serializable]
    public class DialogueElement
    {
        public string name;
        public string text;
    }
}