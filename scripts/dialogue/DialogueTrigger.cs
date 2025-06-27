// DialogueTrigger.cs
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles detecting the player within range and triggering dialogue
/// when the interact button is pressed.
/// </summary>
public class DialogueTrigger : MonoBehaviour
{
    [Header("Visual Cue")]
    [Tooltip("The GameObject that visually indicates dialogue can be triggered (e.g., an exclamation mark).")]
    [SerializeField] private GameObject visualCue;

    [Header("Ink JSON")]
    [Tooltip("The Ink JSON file containing the dialogue for this trigger.")]
    [SerializeField] private TextAsset inkJSON;

    private bool playerInRange; // True if the player is within the trigger collider

    /// <summary>
    /// Initializes the trigger state.
    /// </summary>
    private void Awake()
    {
        playerInRange = false;
        // Ensure the visual cue is hidden at the start
        if (visualCue != null)
        {
            visualCue.SetActive(false);
        }
        else
        {
            Debug.LogWarning("DialogueTrigger: Visual Cue GameObject is not assigned on " + gameObject.name);
        }
    }

    /// <summary>
    /// Checks for player interaction when in range.
    /// </summary>
    private void Update()
    {
        // Only proceed if InputManager instance exists
        if (InputManager.GetInstance() == null)
        {
            Debug.LogError("DialogueTrigger: InputManager.GetInstance() returned null. Make sure InputManager is in the scene.");
            return;
        }

        if (playerInRange)
        {
            // Show the visual cue when the player is in range
            if (visualCue != null)
            {
                visualCue.SetActive(true);
            }

            // Check if the interact button was pressed this frame
            if (InputManager.GetInstance().GetInteractPressed())
            {
                // Ensure there's Ink JSON assigned before trying to start dialogue
                if (inkJSON != null)
                {
                    // Debug.Log is helpful for development, can be removed for release.
                    // Debug.Log(inkJSON.text);

                    // Attempt to enter dialogue mode via the DialogueManager singleton
                    if (DialogueManager.GetInstance() != null)
                    {
                        DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
                    }
                    else
                    {
                        Debug.LogError("DialogueTrigger: DialogueManager.GetInstance() returned null. Make sure DialogueManager is in the scene.");
                    }
                }
                else
                {
                    Debug.LogWarning("DialogueTrigger: Ink JSON TextAsset is not assigned on " + gameObject.name + ". Cannot start dialogue.");
                }
            }
        }
        else
        {
            // Hide the visual cue when the player is out of range
            if (visualCue != null)
            {
                visualCue.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Called when another collider enters this trigger's collider.
    /// </summary>
    /// <param name="collider">The other Collider2D.</param>
    private void OnTriggerEnter2D(Collider2D collider)
    {
        // Check if the entering collider has the "Player" tag
        if (collider.gameObject.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    /// <summary>
    /// Called when another collider exits this trigger's collider.
    /// </summary>
    /// <param name="collider">The other Collider2D.</param>
    private void OnTriggerExit2D(Collider2D collider)
    {
        // Check if the exiting collider has the "Player" tag
        if (collider.gameObject.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}