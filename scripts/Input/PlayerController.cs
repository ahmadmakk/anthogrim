// PlayerController.cs
using UnityEngine;

/// <summary>
/// Controls player movement and animation based on input.
/// Requires a Rigidbody2D and Animator component.
/// </summary>
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [Tooltip("The speed at which the player moves.")]
    public float moveSpeed = 5f;

    // --- Components ---
    private Rigidbody2D rb;         // Reference to the player's Rigidbody2D
    private Animator animator;      // Reference to the player's Animator
    private InputManager inputManager; // Reference to the singleton InputManager

    // --- Internal State ---
    private Vector2 movementInput;      // Raw input vector from InputManager
    private Vector2 currentMovement;    // Processed cardinal movement vector (e.g., (1,0), (0,-1))
    private float lastMoveX;            // Last horizontal direction the player was facing/moving
    private float lastMoveY;            // Last vertical direction the player was facing/moving
    private bool isMoving;              // True if the player is currently moving

    /// <summary>
    /// Called when the script instance is being loaded.
    /// Retrieves and validates required components.
    /// </summary>
    private void Awake()
    {
        // Get Rigidbody2D component and ensure it exists
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("PlayerController: Rigidbody2D component not found on " + gameObject.name + ". Disabling script.");
            enabled = false; // Disable script if essential component is missing
            return;
        }

        // Get Animator component and ensure it exists
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("PlayerController: Animator component not found on " + gameObject.name + ". Disabling script.");
            enabled = false;
            return;
        }
    }

    /// <summary>
    /// Called on the frame when a script is enabled just before any Update methods are called the first time.
    /// Initializes references and default states.
    /// </summary>
    private void Start()
    {
        // Get the singleton instance of the InputManager
        inputManager = InputManager.GetInstance();
        if (inputManager == null)
        {
            Debug.LogError("PlayerController: InputManager instance not found. Make sure an InputManager exists in the scene and is initialized.");
            enabled = false;
            return;
        }

        // Initialize last move direction for idle animation (e.g., facing down by default)
        lastMoveX = 0f;
        lastMoveY = -1f;
    }

    /// <summary>
    /// Called once per frame. Handles input and updates animator parameters.
    /// </summary>
    private void Update()
    {
        if (inputManager == null || animator == null) return; // Safety check if Start or Awake failed

        HandleInput();
        UpdateAnimatorParameters();
    }

    /// <summary>
    /// Called at a fixed framerate. Handles physics-related movement.
    /// </summary>
    private void FixedUpdate()
    {
        if (rb == null) return; // Safety check

        MovePlayer();
    }

    /// <summary>
    /// Reads input from the InputManager and processes it into a cardinal movement vector.
    /// Prioritizes horizontal movement if both horizontal and vertical inputs are present.
    /// </summary>
    private void HandleInput()
    {
        movementInput = inputManager.GetMoveDirection(); // Get raw input from InputManager
        currentMovement = Vector2.zero; // Reset current movement for this frame

        // Determine cardinal movement direction
        float horizontalInput = movementInput.x;
        float verticalInput = movementInput.y;

        // Prioritize horizontal input if significant
        if (Mathf.Abs(horizontalInput) > 0.01f) // Check for significant horizontal input (e.g., not near zero)
        {
            currentMovement.x = Mathf.Sign(horizontalInput); // Set to -1 or 1
            currentMovement.y = 0f; // Ensure no vertical movement if horizontal is prioritized
        }
        // Otherwise, check for significant vertical input
        else if (Mathf.Abs(verticalInput) > 0.01f) // Check for significant vertical input
        {
            currentMovement.y = Mathf.Sign(verticalInput); // Set to -1 or 1
            currentMovement.x = 0f; // Ensure no horizontal movement
        }
        // If no significant input, currentMovement remains Vector2.zero, meaning player is idle.

        // Determine if the player is moving based on the processed movement vector
        isMoving = currentMovement.sqrMagnitude > 0.01f; // Using sqrMagnitude for performance
    }

    /// <summary>
    /// Applies movement to the player's Rigidbody2D.
    /// </summary>
    private void MovePlayer()
    {
        // Calculate the new position based on current movement, speed, and fixed delta time
        Vector2 newPosition = rb.position + currentMovement * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(newPosition); // Move the Rigidbody2D to the new position
    }

    /// <summary>
    /// Updates the Animator's parameters to control player animations.
    /// </summary>
    private void UpdateAnimatorParameters()
    {
        if (animator == null) return; // Safety check

        animator.SetBool("IsMoving", isMoving); // Tell the animator if the player is moving

        if (isMoving)
        {
            // Set MoveX and MoveY for blend tree parameters (e.g., walking animations)
            animator.SetFloat("MoveX", currentMovement.x);
            animator.SetFloat("MoveY", currentMovement.y);

            // Update lastMoveX and lastMoveY to store the cardinal direction the player is currently facing/moving
            // This is useful for idle animations that depend on the last direction of movement.
            if (Mathf.Abs(currentMovement.x) > 0.01f)
            {
                lastMoveX = currentMovement.x;
                lastMoveY = 0f; // When moving horizontally, reset vertical last direction
            }
            else if (Mathf.Abs(currentMovement.y) > 0.01f)
            {
                lastMoveY = currentMovement.y;
                lastMoveX = 0f; // When moving vertically, reset horizontal last direction
            }
        }
        else // Player is idle
        {
            // Set LastMoveX and LastMoveY for idle animations (e.g., standing still facing a direction)
            // These values retain the last direction the player was moving before stopping.
            animator.SetFloat("LastMoveX", lastMoveX);
            animator.SetFloat("LastMoveY", lastMoveY);
        }
    }

    // --- Public Getter Methods ---
    // These methods can be used by other scripts to query the player's movement state.

    /// <summary>
    /// Gets the current cardinal movement direction of the player.
    /// This will be a normalized vector like (1,0), (0,-1), or (0,0) if idle.
    /// </summary>
    /// <returns>A Vector2 representing the current movement direction.</returns>
    public Vector2 GetCurrentMovementDirection()
    {
        return currentMovement;
    }

    /// <summary>
    /// Checks if the player is currently considered to be moving.
    /// </summary>
    /// <returns>True if the player is moving, false otherwise.</returns>
    public bool IsPlayerActuallyMoving()
    {
        return isMoving;
    }

    /// <summary>
    /// Gets the last horizontal direction the player was facing or moving.
    /// Useful for idle animations that depend on the last faced direction.
    /// </summary>
    /// <returns>A float representing the last horizontal facing direction (-1, 0, or 1).</returns>
    public float GetLastFacingX()
    {
        return lastMoveX;
    }

    /// <summary>
    /// Gets the last vertical direction the player was facing or moving.
    /// Useful for idle animations that depend on the last faced direction.
    /// </summary>
    /// <returns>A float representing the last vertical facing direction (-1, 0, or 1).</returns>
    public float GetLastFacingY()
    {
        return lastMoveY;
    }
}