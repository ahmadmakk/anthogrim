using UnityEngine;
using System.Collections; // Required for Coroutines
using System.Collections.Generic; // Required for List

/// <summary>
/// This script manages the spawning of bird prefabs at various points on the map.
/// It has a "camera-culling" effect, meaning birds only spawn at locations
/// that are currently not visible by the main camera, simulating birds landing
/// out of sight.
/// </summary>
public class BirdSpawner : MonoBehaviour
{
    [Header("Bird Prefab")]
    [Tooltip("The bird prefab to be instantiated.")]
    [SerializeField] private GameObject birdPrefab;

    [Header("Spawn Settings")]
    [Tooltip("Array of Transform points where birds can potentially spawn. " +
             "You can create empty GameObjects and drag them here.")]
    [SerializeField] private Transform[] spawnPoints;
    [Tooltip("Maximum distance from a spawn point within which birds can randomly appear.")]
    [SerializeField] private float spawnRadius = 2f;
    [Tooltip("Minimum time between consecutive bird spawns.")]
    [SerializeField] private float minSpawnInterval = 5f;
    [Tooltip("Maximum time between consecutive bird spawns.")]
    [SerializeField] private float maxSpawnInterval = 15f;
    [Tooltip("Number of birds to try and keep active in the scene at any time.")]
    [SerializeField] private int maxBirdsInScene = 5;

    [Header("Camera Visibility Settings")]
    [Tooltip("Reference to the main camera in the scene. If left null, Camera.main will be used.")]
    [SerializeField] private Camera mainCamera;
    [Tooltip("Minimum distance a spawn point must be from the camera to be considered for spawning, " +
             "even if it's technically off-screen. Helps prevent spawning too close.")]
    [SerializeField] private float minSpawnDistanceToCamera = 5f;

    private List<GameObject> activeBirds = new List<GameObject>(); // To keep track of spawned birds
    private Coroutine spawnCoroutine; // Reference to the spawning coroutine

    /// <summary>
    /// Called when the script instance is being loaded.
    /// Gets a reference to the main camera if not already set.
    /// </summary>
    private void Awake()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main; // Automatically find the main camera if not assigned
        }

        if (mainCamera == null)
        {
            Debug.LogError("BirdSpawner: No main camera found in the scene or assigned. Bird spawning may not work as expected.");
            enabled = false;
            return;
        }

        if (birdPrefab == null)
        {
            Debug.LogError("BirdSpawner: Bird Prefab is not assigned. Disabling spawner.");
            enabled = false;
            return;
        }

        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogWarning("BirdSpawner: No spawn points assigned. Please assign Transforms to the Spawn Points array.");
            enabled = false;
            return;
        }
    }

    /// <summary>
    /// Starts the spawning process.
    /// </summary>
    private void Start()
    {
        // Start the continuous spawning coroutine
        spawnCoroutine = StartCoroutine(SpawnBirdsRoutine());
    }

    /// <summary>
    /// Coroutine for continuous bird spawning.
    /// </summary>
    private IEnumerator SpawnBirdsRoutine()
    {
        while (true) // Loop indefinitely
        {
            // Remove any null entries (destroyed birds) from the list
            activeBirds.RemoveAll(bird => bird == null);

            if (activeBirds.Count < maxBirdsInScene)
            {
                TrySpawnBird();
            }

            // Wait for a random interval before trying to spawn again
            float randomInterval = Random.Range(minSpawnInterval, maxSpawnInterval);
            yield return new WaitForSeconds(randomInterval);
        }
    }

    /// <summary>
    /// Attempts to spawn a bird at a random, valid spawn point.
    /// </summary>
    private void TrySpawnBird()
    {
        // Shuffle spawn points to ensure fair random selection
        List<Transform> shuffledSpawnPoints = new List<Transform>(spawnPoints);
        ShuffleList(shuffledSpawnPoints);

        foreach (Transform spawnPoint in shuffledSpawnPoints)
        {
            // Calculate a random position within the spawnRadius around the spawnPoint
            Vector3 randomOffset = Random.insideUnitCircle * spawnRadius;
            Vector3 spawnPosition = spawnPoint.position + randomOffset;
            spawnPosition.z = spawnPoint.position.z; // Keep original Z if 2D

            // Check if the spawn position is NOT visible by the camera AND is far enough
            if (!IsVisibleFromCamera(spawnPosition, mainCamera) &&
                Vector3.Distance(spawnPosition, mainCamera.transform.position) >= minSpawnDistanceToCamera)
            {
                GameObject newBird = Instantiate(birdPrefab, spawnPosition, Quaternion.identity);
                activeBirds.Add(newBird); // Add to our tracking list
                Debug.Log($"Bird spawned at: {spawnPosition} (Not visible by camera)");
                return; // Only spawn one bird per interval
            }
            // else
            // {
            //     Debug.Log($"Spawn point {spawnPoint.name} at {spawnPosition} is visible or too close, skipping.");
            // }
        }
        // Debug.Log("No suitable off-screen spawn point found this interval.");
    }

    /// <summary>
    /// Checks if a given world position is currently visible within the camera's viewport.
    /// Also checks if the position is in front of the camera (z > 0).
    /// </summary>
    /// <param name="worldPosition">The world coordinates to check.</param>
    /// <param name="camera">The camera to check visibility against.</param>
    /// <returns>True if the position is visible, false otherwise.</returns>
    private bool IsVisibleFromCamera(Vector3 worldPosition, Camera camera)
    {
        if (camera == null) return false;

        Vector3 viewportPoint = camera.WorldToViewportPoint(worldPosition);

        // Check if the point is within the viewport (0 to 1 for X and Y)
        // and if it's in front of the camera (Z > 0).
        bool onScreen = viewportPoint.x > 0 && viewportPoint.x < 1 &&
                        viewportPoint.y > 0 && viewportPoint.y < 1 &&
                        viewportPoint.z > 0;

        return onScreen;
    }

    /// <summary>
    /// Shuffles a list using the Fisher-Yates algorithm.
    /// </summary>
    private void ShuffleList<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            T temp = list[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    /// <summary>
    /// Stops the spawning coroutine when the GameObject is destroyed.
    /// </summary>
    private void OnDestroy()
    {
        if (spawnCoroutine != null)
        {
            StopCoroutine(spawnCoroutine);
        }
    }

    // Optional: Draw Gizmos in the Editor for easier setup
    private void OnDrawGizmos()
    {
        if (spawnPoints != null)
        {
            Gizmos.color = Color.green;
            foreach (Transform point in spawnPoints)
            {
                if (point != null)
                {
                    Gizmos.DrawWireSphere(point.position, spawnRadius);
                    Gizmos.DrawLine(point.position, point.position + Vector3.up * 0.5f); // Small line to indicate "up"
                }
            }
        }

        // Draw min spawn distance sphere around camera (if camera is assigned)
        if (mainCamera != null && minSpawnDistanceToCamera > 0)
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(mainCamera.transform.position, minSpawnDistanceToCamera);
        }
    }
}
