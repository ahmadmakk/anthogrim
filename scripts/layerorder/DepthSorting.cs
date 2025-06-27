using UnityEngine;

/// <summary>
/// This script dynamically changes the GameObject's Sprite Renderer's
/// 'Sorting Layer' based on the player's Y-position relative to this object's Y-position.
/// It's designed to create a 2D depth effect where the player appears
/// to walk behind or in front of objects, by switching which predefined
/// Sorting Layer the object belongs to.
/// </summary>
[RequireComponent(typeof(SpriteRenderer))] // Ensure this GameObject has a SpriteRenderer
public class DepthSorting : MonoBehaviour
{
    [Header("Dynamic Sorting Layer Names")]
    [Tooltip("The name of the SORTING LAYER for this object when the player is IN FRONT of it (object appears BEHIND player).")]
    [SerializeField] private string sortingLayerNameWhenPlayerInFront = "underneufa"; // Example: The layer that draws behind the player

    [Tooltip("The name of the SORTING LAYER for this object when the player is BEHIND it (object appears IN FRONT of player).")]
    [SerializeField] private string sortingLayerNameWhenPlayerBehind = "top of neufa"; // Example: The layer that draws in front of the player

    private SpriteRenderer spriteRenderer; // Reference to this GameObject's SpriteRenderer
    private GameObject player;             // Reference to the Player GameObject

    /// <summary>
    /// Called when the script instance is being loaded.
    /// Retrieves components and sets up initial state.
    /// </summary>
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer == null)
        {
            Debug.LogError("DepthSorting: SpriteRenderer component not found on " + gameObject.name + ". This script requires a SpriteRenderer. Disabling script.");
            enabled = false;
            return;
        }

        // Find the player GameObject by its tag. Ensure your player has the "Player" tag.
        player = GameObject.FindGameObjectWithTag("Player");
        if (player == null)
        {
            Debug.LogError("DepthSorting: Player GameObject with 'Player' tag not found in scene. Disabling script on " + gameObject.name);
            enabled = false;
            return;
        }

        // --- Important Validation: Check if the specified Sorting Layers exist ---
        // Get the internal IDs for sorting layers to check if they exist.
        // GetSortingLayerID returns -1 if the layer does not exist.
        if (SortingLayer.NameToID(sortingLayerNameWhenPlayerInFront) == 0 && sortingLayerNameWhenPlayerInFront != "Default") // 0 is Default layer ID
        {
            Debug.LogError($"DepthSorting: Sorting Layer '{sortingLayerNameWhenPlayerInFront}' not found. Please create it in Project Settings -> Tags & Layers -> Sorting Layers. Disabling script on {gameObject.name}.");
            enabled = false;
            return;
        }
        if (SortingLayer.NameToID(sortingLayerNameWhenPlayerBehind) == 0 && sortingLayerNameWhenPlayerBehind != "Default")
        {
            Debug.LogError($"DepthSorting: Sorting Layer '{sortingLayerNameWhenPlayerBehind}' not found. Please create it in Project Settings -> Tags & Layers -> Sorting Layers. Disabling script on {gameObject.name}.");
            enabled = false;
            return;
        }

        // Set the initial sorting layer based on current player position
        UpdateSortingLayer();
    }

    /// <summary>
    /// LateUpdate is called after all Update functions have been called.
    /// This is ideal for sorting logic after all movement for the current frame is finalized.
    /// </summary>
    private void LateUpdate()
    {
        // Safety checks
        if (spriteRenderer == null || player == null) return;

        UpdateSortingLayer();
    }

    /// <summary>
    /// Compares player's Y position to this object's Y position and sets the
    /// SpriteRenderer's sortingLayerName accordingly.
    /// </summary>
    private void UpdateSortingLayer()
    {
        // If the player's Y position is greater than this object's Y position,
        // it means the player is "above" or "behind" this object in 2D space.
        // Therefore, this object should appear IN FRONT of the player.
        if (player.transform.position.y > transform.position.y)
        {
            spriteRenderer.sortingLayerName = sortingLayerNameWhenPlayerBehind;
        }
        else
        {
            // If the player's Y position is less than or equal to this object's Y position,
            // it means the player is "below" or "in front" of this object.
            // Therefore, this object should appear BEHIND the player.
            spriteRenderer.sortingLayerName = sortingLayerNameWhenPlayerInFront;
        }
    }
}
