using UnityEngine;

public class StartDialogueByName : Triggerable
{
    public string DialogueName;
    public Triggerable[] activateObjects;
    public Triggerable[] deactivateObjects;
    private DialogueManager _dialogueManager;
    
    void Start()
    {
        _dialogueManager = GameObject.FindGameObjectWithTag("DialogueManager").GetComponent<DialogueManager>();
    }

    public override void Trigger(TriggerAction action)
    {
        _dialogueManager.CurrentDialogueName = DialogueName;
        _dialogueManager.TriggerFrom(action, this);
    }

    public void TriggerCallback()
    {
        foreach (var callbackObject in activateObjects)
            callbackObject.Trigger(TriggerAction.Activate);
        foreach (var callbackObject in deactivateObjects)
            callbackObject.Trigger(TriggerAction.Deactivate);
    }
}
