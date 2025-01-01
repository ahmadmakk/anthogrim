using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class roomdoor : Interactable
{
    

    public override void Interact()
    {
        x++;
        // Disable the sprite renderer to hide the sprite
       player.transform.position = new Vector3(0.513f, -0.674f, -1f);

    }
}
