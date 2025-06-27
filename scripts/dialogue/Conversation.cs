using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Conversation", menuName = "Dialogue/Conversation")]
public class Conversation : ScriptableObject
{
    public DialogueLine[] lines;
    public DialogueChoice[] choices; // Choices appear AFTER all lines are shown
    public Conversation nextConversationOnEnd; // If no choices, or after a choice with no specific next convo
    public bool endConversationAfterThis = false; // If this is a true end point
}