using UnityEngine;

public class interactable : MonoBehaviour
{
    public int x = 0;
    public bool interacted = false;
    public GameObject player;
    private SpriteRenderer closetSpriteRenderer;

    // Initialize closetSpriteRenderer in the Start or Awake method
    void Start()
    {
        closetSpriteRenderer = GetComponent<SpriteRenderer>();
    }

    // This method can be called to perform interaction logic
    public virtual void Interact()
    {
        // Implement your interaction logic here
        Debug.Log(gameObject.name + " interacted!");
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the colliding object has the tag "Player"
        if (collision.gameObject.CompareTag("Player") && x==0)
        {
            closetSpriteRenderer.enabled = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            closetSpriteRenderer.enabled = false;
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        // Check if the other collider has a specific tag or component
        if (other.CompareTag("Player"))
        {
            // Call the interact function
            Interact();
        }
    }
}
