// BirdBehavior.cs
using UnityEngine;
using System.Collections; // Required for Coroutines

public class BirdFLyAway : MonoBehaviour
{
    // Public variables configurable in the Unity Editor
    [Header("Animation Settings")]
    [Tooltip("The name of the 'Idle' animation state in the Animator.")]
    public string idleAnimationName = "idle"; // Ensure this matches your Animator's state name
    [Tooltip("The name of the 'FlyAway' animation state in the Animator.")]
    public string flyAwayAnimationName = "birdyellow"; // Ensure this matches your Animator's state name

    [Header("Player Detection")]
    [Tooltip("The tag of the Player GameObject.")]
    public string playerTag = "Player"; // Make sure your player GameObject has this tag
    [Tooltip("The distance at which the bird will start to fly away.")]
    public float proximityThreshold = 0.3f; // Distance in Unity units

    // Private variables
    private Animator birdAnimator; // Reference to the Animator component
    private GameObject playerGameObject; // Reference to the player GameObject
    private bool hasFled = false; // Flag to ensure the bird flees only once

    void Awake()
    {
        // Get the Animator component attached to this GameObject
        birdAnimator = GetComponent<Animator>();
        if (birdAnimator == null)
        {
            Debug.LogError("BirdBehavior: Animator component not found on this GameObject. Please add an Animator.", this);
        }

        // Find the player GameObject by tag.
        // It's generally better to find this once in Awake/Start if the player is static.
        playerGameObject = GameObject.FindWithTag(playerTag);
        if (playerGameObject == null)
        {
            Debug.LogWarning($"BirdBehavior: Player GameObject with tag '{playerTag}' not found. Make sure your player is tagged correctly.", this);
        }
    }

    void Start()
    {
        // Ensure the idle animation is playing at the start
        if (birdAnimator != null)
        {
            birdAnimator.Play(idleAnimationName);
        }
    }

    void Update()
    {
        // Only proceed if the bird hasn't fled yet and player is found
        if (!hasFled && playerGameObject != null)
        {
            // Calculate the distance between the bird and the player
            float distanceToPlayer = Vector3.Distance(transform.position, playerGameObject.transform.position);

            // Check if the player is within the proximity threshold
            if (distanceToPlayer < proximityThreshold)
            {
                FlyAway(); // Trigger the fly away action
            }
        }
    }

    /**
     * <summary>
     * Triggers the "fly away" animation and sets the flag to prevent re-triggering.
     * </summary>
     */
    void FlyAway()
    {
        // Set the flag to true to prevent this method from being called again
        hasFled = true;

        if (birdAnimator != null)
        {
            // Play the "fly away" animation
            birdAnimator.Play(flyAwayAnimationName);
            Debug.Log($"Bird is playing '{flyAwayAnimationName}' animation.");

            // Start a coroutine to wait for the animation to finish and then destroy the GameObject
            StartCoroutine(WaitForAnimationAndDestroy(flyAwayAnimationName));
        }
    }

    /**
     * <summary>
     * Coroutine to wait for a specified animation to complete before destroying the GameObject.
     * </summary>
     * <param name="animationName">The name of the animation state to wait for.</param>
     */
    private IEnumerator WaitForAnimationAndDestroy(string animationName)
    {
        // Wait until the current animation state is the one we're interested in
        // and its normalized time is less than 1 (meaning it's still playing)
        // This is important because Play() doesn't instantly make the animation active in the current frame.
        // It takes one frame for the state transition to complete.
        while (birdAnimator != null && !birdAnimator.GetCurrentAnimatorStateInfo(0).IsName(animationName))
        {
            yield return null; // Wait for the next frame
        }

        // Get the length of the "FlyAway" animation clip
        // This assumes the animation state 'FlyAway' directly corresponds to a single clip.
        // If 'FlyAway' is a blend tree or has multiple clips, this might need adjustment.
        float animationLength = birdAnimator.GetCurrentAnimatorStateInfo(0).length;

        Debug.Log($"Waiting for '{animationName}' animation to complete. Length: {animationLength} seconds.");

        // Wait for the duration of the animation clip
        yield return new WaitForSeconds(animationLength);

        // After the animation is over, destroy the bird GameObject
        Debug.Log($"'{animationName}' animation finished. Destroying bird GameObject.");
        Destroy(gameObject); // 'gameObject' refers to the GameObject this script is attached to
    }

    // You could also use an Animation Event within your "FlyAway" animation clip
    // to call a public function on this script, like this:
    /*
    public void OnFlyAwayAnimationEnd()
    {
        Debug.Log("Animation Event: FlyAway animation finished. Destroying bird GameObject.");
        Destroy(gameObject);
    }
    */
    // If using Animation Event, remove the StartCoroutine(WaitForAnimationAndDestroy) call
    // from FlyAway() and ensure the Animation Event is set up correctly in Unity.
}
