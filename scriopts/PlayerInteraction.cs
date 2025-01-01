using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject interactButton; // UI Button to interact
    private Interactable currentInteractable;

    [Header("Animator")]
    public Animator playerAnimator;

    private bool playerInRange = false;

    void Start()
    {
        interactButton.SetActive(false); // Hide the interact button at start
    }

    private void Update()
    {
        // If player is in range and the interact button is pressed, handle interaction
        if (playerInRange && currentInteractable != null && Input.GetButtonDown("Interact")) // Or use your UI button callback
        {
            OnInteractButtonClicked();
        }
    }

    private void OnTriggerEnter(Collider collider)
    {
        // Check if the player is near an interactable object
        Interactable interactable = collider.GetComponent<Interactable>();
        if (interactable != null)
        {
            currentInteractable = interactable;
            playerInRange = true;
            interactButton.SetActive(true); // Show interaction button
            Debug.Log("Interactable detected: " + interactable.gameObject.name);
        }
    }

    private void OnTriggerExit(Collider collider)
    {
        // Clear interactable when the player leaves its range
        if (currentInteractable != null && collider.gameObject == currentInteractable.gameObject)
        {
            currentInteractable = null;
            playerInRange = false;
            interactButton.SetActive(false); // Hide interaction button
            Debug.Log("Exited interaction with: " + collider.gameObject.name);
        }
    }

    public void OnInteractButtonClicked()
    {
        // Handle interaction
        if (currentInteractable != null)
        {
            currentInteractable.Interact(); // Call the Interact method
            Debug.Log("Interacted with: " + currentInteractable.gameObject.name);
        }
        else
        {
            Debug.Log("noooooooooooInteracted with: ");
        }

    }

    public Animator GetAnimator()
    {
        return playerAnimator;
    }
}