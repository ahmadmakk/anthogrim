using System.Collections;
using UnityEngine;
using TMPro;

public class adam1 : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro; // Reference to your TextMeshPro component
    public AudioClip[] typingSounds; // Array of audio clips for each letter sound
    public float typingSpeed = 0.05f; // Speed of typing effect

    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        StartCoroutine(TypeText("Lived a life where I found myself in misery,\r\nA life awarded to me as mockery.\r\nTreated badly, an awful way to express it,\r\nBarred with shame and heed in a pit.\r\n"));
    }

    private IEnumerator TypeText(string text)
    {
        textMeshPro.text = ""; // Clear existing text
        foreach (char letter in text.ToCharArray())
        {
            textMeshPro.text += letter; // Append each letter
            PlayTypingSound(); // Play the typing sound
            yield return new WaitForSeconds(typingSpeed); // Wait before typing the next letter
        }
    }

    private void PlayTypingSound()
    {
        if (typingSounds.Length > 0)
        {
            int index = Random.Range(0, typingSounds.Length);
            audioSource.PlayOneShot(typingSounds[index]);
        }
    }
}
