using UnityEngine;
using TMPro; // Essential for TextMeshProUGUI
using Ink.Runtime; // Required for Ink dialogue system
using System.Collections; // Required for Coroutines

/// <summary>
/// Manages the display and progression of dialogue using Ink.
/// Implements a singleton pattern to ensure only one instance exists.
/// Handles letter-by-letter typing effect and dialogue advancement.
/// </summary>
[RequireComponent(typeof(AudioSource))] // Ensure an AudioSource is present for typing sounds
public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue UI")]
    [Tooltip("The panel GameObject that contains all dialogue UI elements.")]
    [SerializeField] private GameObject dialoguePanel;
    [Tooltip("The TextMeshProUGUI component where dialogue text will be displayed.")]
    [SerializeField] private TextMeshProUGUI dialogueText;

    [Header("Typing Effect")]
    [Tooltip("Speed at which characters appear (e.g., 0.05 for fast, 0.1 for slower).")]
    [SerializeField] private float typingSpeed = 0.04f;
    [Tooltip("Array of audio clips to play for each typed character.")]
    [SerializeField] private AudioClip[] typingSounds;

    private Story currentStory;         // The current Ink story being played
    private static DialogueManager instance; // Singleton instance
    private bool dialogueIsPlaying;     // Flag to indicate if dialogue is currently active
    private Coroutine typingCoroutine;  // Reference to the currently running typing coroutine
    private AudioSource audioSource;    // Reference to the AudioSource component for typing sounds

    /// <summary>
    /// Ensures only one instance of DialogueManager exists and gets AudioSource.
    /// </summary>
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.LogWarning("DialogueManager: Found more than one Dialogue Manager in the scene. Destroying this duplicate.");
            Destroy(gameObject); // Destroy the duplicate instance
            return;
        }
        instance = this;
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("DialogueManager: AudioSource component not found. Typing sounds will not play. Please add an AudioSource to this GameObject.");
        }
    }

    /// <summary>
    /// Provides access to the singleton instance of DialogueManager.
    /// </summary>
    /// <returns>The single instance of DialogueManager.</returns>
    public static DialogueManager GetInstance()
    {
        return instance;
    }

    /// <summary>
    /// Initializes dialogue state and hides the dialogue panel.
    /// </summary>
    private void Start()
    {
        dialogueIsPlaying = false;
        // Ensure the dialogue panel is hidden at start
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }
        else
        {
            Debug.LogError("DialogueManager: DialoguePanel is not assigned in the inspector!");
        }
    }

    /// <summary>
    /// Checks for input to advance dialogue if dialogue is playing.
    /// </summary>
    private void Update()
    {
        // Return immediately if dialogue is not active
        if (!dialogueIsPlaying)
        {
            return;
        }

        // Safety checks for InputManager and dialogueText
        if (InputManager.GetInstance() == null)
        {
            Debug.LogError("DialogueManager: InputManager.GetInstance() returned null. Please ensure an InputManager exists in the scene.");
            return;
        }
        if (dialogueText == null)
        {
            Debug.LogError("DialogueManager: DialogueText (TextMeshProUGUI) is not assigned in the inspector!");
            ExitDialogueMode(); // Exit dialogue if essential UI is missing
            return;
        }

        // Check if the submit button was pressed (keyboard/gamepad) to advance the story.
        // If a typing coroutine is running, pressing submit should skip the typing.
        // Otherwise, it should advance to the next line.
        if (InputManager.GetInstance().GetSubmitPressed())
        {
            if (typingCoroutine != null)
            {
                // If text is currently typing, complete it instantly
                StopCoroutine(typingCoroutine);
                dialogueText.text = currentStory.currentText; // Set full text instantly
                typingCoroutine = null; // Clear the coroutine reference
            }
            else
            {
                // If text is not typing, continue to the next line of dialogue
                ContinueStory();
            }
        }
    }

    /// <summary>
    /// Enters dialogue mode, initializing the Ink story and showing the UI.
    /// </summary>
    /// <param name="inkJSON">The TextAsset containing the Ink JSON.</param>
    public void EnterDialogueMode(TextAsset inkJSON)
    {
        if (inkJSON == null)
        {
            Debug.LogError("DialogueManager: InkJSON TextAsset is null. Cannot start dialogue.");
            return;
        }

        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;

        // Show the dialogue panel
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(true);
        }
        else
        {
            Debug.LogError("DialogueManager: DialoguePanel is null when trying to enter dialogue mode.");
            return; // Prevent further errors if panel is missing
        }

        // Reset text at the start of a new dialogue
        if (dialogueText != null)
        {
            dialogueText.text = "";
        }

        ContinueStory(); // Display the first line of dialogue
    }

    /// <summary>
    /// Exits dialogue mode, hiding the UI and resetting state.
    /// </summary>
    private void ExitDialogueMode()
    {
        dialogueIsPlaying = false;
        // Stop any active typing coroutine
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        // Hide the dialogue panel
        if (dialoguePanel != null)
        {
            dialoguePanel.SetActive(false);
        }
        // Clear the dialogue text
        if (dialogueText != null)
        {
            dialogueText.text = "";
        }
        currentStory = null; // Clear the story reference
    }

    /// <summary>
    /// Continues the Ink story to the next line or exits if the story ends.
    /// </summary>
    private void ContinueStory()
    {
        if (currentStory == null)
        {
            Debug.LogError("DialogueManager: currentStory is null. Cannot continue dialogue.");
            ExitDialogueMode();
            return;
        }

        if (dialogueText == null)
        {
            Debug.LogError("DialogueManager: DialogueText (TextMeshProUGUI) is null. Cannot display dialogue.");
            ExitDialogueMode();
            return;
        }

        // Stop any previous typing coroutine before starting a new line
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }

        if (currentStory.canContinue)
        {
            // Start the typing effect for the new line
            string nextLine = currentStory.Continue();
            typingCoroutine = StartCoroutine(TypeText(nextLine));
        }
        else
        {
            // If the story cannot continue, exit dialogue mode
            ExitDialogueMode();
        }
    }

    /// <summary>
    /// Coroutine to display text character by character with a typing effect.
    /// </summary>
    /// <param name="text">The full string of text to type out.</param>
    private IEnumerator TypeText(string text)
    {
        dialogueText.text = ""; // Clear existing text before typing
        foreach (char letter in text.ToCharArray())
        {
            dialogueText.text += letter; // Append each letter
            PlayTypingSound();           // Play the typing sound
            yield return new WaitForSeconds(typingSpeed); // Wait before typing the next letter
        }
        typingCoroutine = null; // Typing finished, clear the coroutine reference
    }

    /// <summary>
    /// Plays a random typing sound from the array.
    /// </summary>
    private void PlayTypingSound()
    {
        if (audioSource != null && typingSounds != null && typingSounds.Length > 0)
        {
            int index = Random.Range(0, typingSounds.Length);
            audioSource.PlayOneShot(typingSounds[index]);
        }
    }

    /// <summary>
    /// Public method to be called directly by a UI button's OnClick() event to advance dialogue.
    /// </summary>
    public void OnAdvanceDialogueButtonClicked()
    {
        Debug.Log("UI Button Advance Dialogue Clicked!"); // Debug to confirm button click
        if (dialogueIsPlaying)
        {
            // If text is currently typing, complete it instantly
            if (typingCoroutine != null)
            {
                StopCoroutine(typingCoroutine);
                dialogueText.text = currentStory.currentText; // Set full text instantly
                typingCoroutine = null; // Clear the coroutine reference
            }
            else
            {
                // If text is not typing, continue to the next line of dialogue
                ContinueStory();
            }
        }
    }
}
