using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : Triggerable
{
    public String SceneName;
    public DialogueCharacter[] Characters;
    public DialogueLevelManager DialogueLevelManager;
    public LogicController logicController;
    public string[] Order;
    public string CurrentDialogueName;
    public int CurrentDialogueNumber;
    public bool DialogueIsActive;

    public Image LeftImage;
    public Image RightImage;
    public Text CharacterName;
    public Text CharacterText;

    private Dictionary<String, DialogueCharacter> _characters;

    void Start()
    {
        Order = DialogueLevelManager.DialogueSceneOrder[SceneName];
        _characters = Characters.ToDictionary(c => c.Name);
        CurrentDialogueName = Order[0];
        CurrentDialogueNumber = 0;
        ClearDialogue();
    }

    // Update is called once per frame
    void Update()
    {
        if (DialogueIsActive)
        {
            var dialogue = DialogueLevelManager.DialogueSystem[SceneName][CurrentDialogueName];
            // Debug.Log($"Dialog is active: {SceneName} {CurrentDialogueName} {CurrentDialogueNumber}");
            // Debug.Log($"{string.Join("|", DialogueLevelManager.DialogueSystem.Keys)}");
            // Debug.Log($"{string.Join("|", DialogueLevelManager.DialogueSystem[SceneName].Keys)}");
            // Debug.Log($"{string.Join("|", DialogueLevelManager.DialogueSystem[SceneName][CurrentDialogueName].Select(s => s.name))}");

            if (Input.GetKeyUp(KeyCode.Space))
                CurrentDialogueNumber++;
            if (CurrentDialogueNumber < dialogue.Length)
                SetCharacter(dialogue[CurrentDialogueNumber]);
            else
            {
                ClearDialogue();
                DialogueIsActive = false;
                logicController.StartLogic();
                if (invocationObject != null)
                {
                    invocationObject.TriggerCallback();
                    invocationObject = null;
                }
            }
        }
    }

    private StartDialogueByName invocationObject;
    public void TriggerFrom(TriggerAction action, StartDialogueByName gameObject)
    {
        invocationObject = gameObject;
        Trigger(action);
    }

    void SetCharacter(DialogueLevelManager.DialogueElement dialogueElement)
    {
        if (CurrentDialogueNumber % 2 == 0)
        {
            RightImage.gameObject.SetActive(false);
            LeftImage.gameObject.SetActive(true);
            LeftImage.sprite = _characters[dialogueElement.name].MainFace;
            // LeftImage.transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
        else
        {
            // LeftImage.transform.localRotation = Quaternion.Euler(0, 0, 0);
            LeftImage.gameObject.SetActive(false);
            RightImage.gameObject.SetActive(true);
            RightImage.sprite = _characters[dialogueElement.name].MainFace;
        }

        CharacterName.text = dialogueElement.name;
        CharacterText.text = dialogueElement.text;
    }

    void ClearDialogue()
    {
        LeftImage.gameObject.SetActive(false);
        RightImage.gameObject.SetActive(false);
        CharacterName.text = "";
        CharacterText.text = "";
        CurrentDialogueNumber = 0;
    }

    public override void Trigger(TriggerAction action)
    {
        Debug.Log($"{action} {DialogueIsActive} {CurrentDialogueName}");
        if (action == TriggerAction.Activate && !DialogueIsActive)
        {
            logicController.StopLogic();
            DialogueIsActive = true;
        }
        else if (action == TriggerAction.Deactivate && DialogueIsActive)
        {
            logicController.StartLogic();
            DialogueIsActive = false;
        }
    }
}