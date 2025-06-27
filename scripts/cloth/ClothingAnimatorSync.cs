// ClothingAnimatorSync.cs
using UnityEngine;

public class ClothingAnimatorSync : MonoBehaviour
{
    public Animator clothingAnimator;       // Animator for this clothing item
    public PlayerController playerController; // Reference to the player

    private void Start()
    {
        // It's generally better to assign these in the Inspector if possible,
        // but GetComponent/GetComponentInParent is a fallback.
        if (clothingAnimator == null)
        {
            clothingAnimator = GetComponent<Animator>();
        }
        if (playerController == null)
        {
            playerController = GetComponentInParent<PlayerController>();
        }

        // Error handling
        if (clothingAnimator == null)
        {
            Debug.LogError("ClothingAnimatorSync: Animator component not found on " + gameObject.name);
            enabled = false;
            return;
        }
        if (playerController == null)
        {
            Debug.LogError("ClothingAnimatorSync: PlayerController not found in parent of " + gameObject.name);
            enabled = false;
            return;
        }
    }

    private void Update()
    {
        // Null checks are good, though Start() already has them.
        // If Start() disables the script, Update won't run.
        // if (playerController == null || clothingAnimator == null)
        //     return;

        // Get data from PlayerController using the new getters
        bool isPlayerMoving = playerController.IsPlayerActuallyMoving();
        Vector2 currentMoveDirection = playerController.GetCurrentMovementDirection();

        clothingAnimator.SetBool("IsMoving", isPlayerMoving);

        if (isPlayerMoving)
        {
            clothingAnimator.SetFloat("MoveX", currentMoveDirection.x);
            clothingAnimator.SetFloat("MoveY", currentMoveDirection.y);
        }
        else
        {
            // When not moving, clothing should ideally face the player's last direction
            // Assuming your clothing animator also has LastMoveX and LastMoveY parameters
            clothingAnimator.SetFloat("MoveX", 0); // Or use LastMoveX if appropriate for your idle blend tree
            clothingAnimator.SetFloat("MoveY", 0); // Or use LastMoveY
           // clothingAnimator.SetFloat("LastMoveX", playerController.GetLastFacingX());
           // clothingAnimator.SetFloat("LastMoveY", playerController.GetLastFacingY());
        }
    }
}