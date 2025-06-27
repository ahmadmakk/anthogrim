// InputManager.cs
using UnityEngine;
using UnityEngine.InputSystem; // Required for the new Input System

/// <summary>
/// Manages all player input, providing a centralized access point for other scripts.
/// Uses a singleton pattern and handles "pressed this frame" logic for actions.
/// </summary>
[RequireComponent(typeof(PlayerInput))] // Ensures a PlayerInput component is present
public class InputManager : MonoBehaviour
{
    // --- Input States ---
    private Vector2 moveDirection = Vector2.zero; // Stores the current movement input vector

    // Interact Action States
    private bool interactButtonIsHeld = false;      // True if the interact button is currently held down
    private bool interactPressedThisFrame = false;  // True for one frame when interact button is pressed

    // Submit Action States (used for dialogue progression/UI navigation)
    private bool submitButtonIsHeld = false;        // True if the submit button is currently held down
    private bool submitPressedThisFrame = false;    // True for one frame when submit button is pressed
    private bool submitWasRegisteredByUI = false;   // Flag to prevent submit input from triggering both UI and game logic

    // --- Singleton Instance ---
    private static InputManager instance;

    /// <summary>
    /// Initializes the singleton instance and handles potential duplicates.
    /// </summary>
    private void Awake()
    {
        // Robust Singleton Pattern implementation
        if (instance != null && instance != this)
        {
            Debug.LogWarning("InputManager: Found more than one InputManager instance in the scene. Destroying this duplicate.");
            Destroy(gameObject); // Destroy the duplicate GameObject
            return;
        }
        instance = this;
        // Optional: Uncomment the line below if this InputManager should persist across scene loads.
        // DontDestroyOnLoad(this.gameObject);
    }

    /// <summary>
    /// Provides static access to the singleton instance of the InputManager.
    /// </summary>
    /// <returns>The single instance of InputManager.</returns>
    public static InputManager GetInstance()
    {
        return instance;
    }

    // --- Input Action Callbacks ---
    // These methods are typically hooked up in the PlayerInput component's "Events" section
    // under the corresponding Input Action (e.g., "Move" action calls "MovePressed").

    /// <summary>
    /// Callback for the "Move" input action. Reads the movement vector.
    /// </summary>
    /// <param name="context">The callback context provided by the Input System.</param>
    public void MovePressed(InputAction.CallbackContext context)
    {
        if (context.performed) // When the action is actively performed (e.g., key held)
        {
            moveDirection = context.ReadValue<Vector2>();
        }
        else if (context.canceled) // When the action is no longer performed (e.g., key released)
        {
            moveDirection = Vector2.zero;
        }
    }

    /// <summary>
    /// Gets the current raw movement input direction.
    /// </summary>
    /// <returns>A Vector2 representing the movement input.</returns>
    public Vector2 GetMoveDirection()
    {
        return moveDirection;
    }

    /// <summary>
    /// Callback for the "Interact" input action.
    /// </summary>
    
    public void InteractButtonPressed(InputAction.CallbackContext context)
    {
        if (context.performed) // When the interact button is first pressed down
        {
            interactButtonIsHeld = true;
            interactPressedThisFrame = true; // Mark as pressed for this frame
        }
        else if (context.canceled) // When the interact button is released
        {
            interactButtonIsHeld = false;
        }
    }

    /// <summary>
    /// Checks if the "Interact" button was pressed down this specific frame.
    /// This flag is reset at the end of the frame.
    /// </summary>
    /// <returns>True if the interact button was pressed this frame, false otherwise.</returns>
    public bool GetInteractPressed()
    {
        return interactPressedThisFrame;
    }

    /// <summary>
    /// Checks if the "Interact" button is currently held down.
    /// </summary>
    /// <returns>True if the interact button is held, false otherwise.</returns>
    public bool IsInteractButtonHeld()
    {
        return interactButtonIsHeld;
    }

    /// <summary>
    /// Callback for the "Submit" input action.
    /// </summary>
    
    public void SubmitButtonPressed(InputAction.CallbackContext context)
    {
        if (context.performed) // When the submit button is first pressed down
        {
            submitButtonIsHeld = true;
            // Only set submitPressedThisFrame if it hasn't been consumed by UI this frame
            if (!submitWasRegisteredByUI)
            {
                submitPressedThisFrame = true;
            }
        }
        else if (context.canceled) // When the submit button is released
        {
            submitButtonIsHeld = false;
        }
    }

    /// <summary>
    /// Gets the raw held state of the "Submit" button.
    /// </summary>
    /// <returns>True if the submit button is currently held down, false otherwise.</returns>
    public bool GetSubmitButtonState()
    {
        return submitButtonIsHeld;
    }

    /// <summary>
    /// Checks if the "Submit" button was pressed down this specific frame.
    /// This flag is reset at the end of the frame.
    /// </summary>
    /// <returns>True if the submit button was pressed this frame and not consumed by UI, false otherwise.</returns>
    public bool GetSubmitPressed()
    {
        return submitPressedThisFrame;
    }

    /// <summary>
    /// Call this method from UI elements (e.g., an EventSystem) when the "Submit" action
    /// is consumed by a UI interaction. This prevents the same input from triggering
    /// both UI navigation and game logic (like advancing dialogue) in the same frame.
    /// </summary>
    public void RegisterSubmitPressed()
    {
        Debug.Log("lolo");
        submitPressedThisFrame = false;     // Clear the "pressed this frame" flag
        submitWasRegisteredByUI = true;     // Mark that UI consumed it for this frame
    }

    /// <summary>
    /// Called after all Update functions have been called.
    /// Resets "pressed this frame" flags to ensure they are only true for one frame.
    /// </summary>
    private void LateUpdate()
    {
        interactPressedThisFrame = false;
        submitPressedThisFrame = false;
        submitWasRegisteredByUI = false; // Reset for the next frame
    }
}
