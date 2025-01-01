using UnityEngine;

public class coin : MonoBehaviour
{
    public int x = 0;  // Variable to track the number of times the coin is collected
    //public AudioClip collectSound;  // Optional: sound to play upon collection

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CollectCoin(other.gameObject);
        }
    }

    void CollectCoin(GameObject playerObject)
    {
        x = x + 1;

        // Get the Player script from the player GameObject
        Player player = playerObject.GetComponent<Player>();
        
            player.activatecontrolles();
        

        // Play collection sound if assigned
        //if (collectSound != null)
        {
         //   AudioSource.PlayClipAtPoint(collectSound, transform.position);
        }

        // Destroy the coin if x is greater than 1
        if (x > 1)
        {
            Destroy(gameObject);
        }
        if (x == 1)
        {
            transform.position=new Vector3(1.655f,0.384f,0f);
        }
    }
}
