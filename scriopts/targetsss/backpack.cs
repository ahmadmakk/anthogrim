using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class backpack : Interactable
{
   
    public SpriteRenderer backpackSpriteRenderer;

    public override void Interact()
    {
        x++;
        // Disable the sprite renderer to hide the sprite
        backpackSpriteRenderer.enabled =true;
    }
}
