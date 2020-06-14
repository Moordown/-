using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DialogueLevelManager : LevelManager
{
    public const string DialogueRailwaySceneName = "RailwayScene";
    public const string DialogueFightSceneName = "FightScene";
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
        }
    };

    public string DialoguePath = "Assets/Materials/Text";

    public
        Dictionary<string, Dictionary<string, DialogueElement[]>> DialogueSystem =
            new Dictionary<string, Dictionary<string, DialogueElement[]>>();

    public UIManager UiManager;
    public int UIManagerDialogue = -1;

    void Start()
    {
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
            // Debug.Log($"{sceneName} {dialogueName}");
        }

        UiManager.Trigger(TriggerAction.Activate);
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

    public override void StartLogic()
    {
        Debug.Log($"StartLogic: {UIManagerDialogue}");
        UIManagerDialogue++;
        if (UIManagerDialogue < UiManager.Order.Length)
        {
            UiManager.CurrentDialogueName = UiManager.Order[UIManagerDialogue];
            UiManager.Trigger(TriggerAction.Activate);
        }
    }
}