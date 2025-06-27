using UnityEngine;
using UnityEngine.Events; // For custom actions

[System.Serializable]
public class DialogueChoice
{
    public string choiceText;
    public Conversation nextConversation; // What conversation this choice leads to
    public UnityEvent onChoiceSelected; // OPTIONAL: For triggering specific game events
}