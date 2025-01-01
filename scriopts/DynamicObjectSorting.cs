using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicObjectSorting : MonoBehaviour
{
    public Transform playerTransform;  // Assign the player transform in the Inspector

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Check if the player is below the object
        if (playerTransform.position.y > transform.position.y)
        {
            spriteRenderer.sortingOrder = 1;  // Player is behind the object
        }
        else
        {
            spriteRenderer.sortingOrder = -1;  // Player is in front of the object
        }
    }
}
