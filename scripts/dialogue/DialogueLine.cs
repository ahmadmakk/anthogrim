using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    public string speakerName; // Optional: if different characters can speak in one convo
    public Sprite speakerPortrait; // Optional: to show a character image
    [TextArea(3, 10)]
    public string sentence;
}