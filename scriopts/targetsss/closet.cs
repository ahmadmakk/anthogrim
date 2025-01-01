using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class closet : Interactable

{
    
    public RuntimeAnimatorController newAnimatorController;
    // Start is called before the first frame update
    public override void Interact()
    {
        x++;
        Player Playerscript = player.GetComponent<Player>();
        PlayerInteraction playerInteraction = player.GetComponent<PlayerInteraction>();
        if (playerInteraction != null)
        { Playerscript.activatecontrolles();
            Animator playerAnimator = playerInteraction.GetAnimator();
            
            if (playerAnimator != null)
            {
                playerAnimator.runtimeAnimatorController = newAnimatorController;
                Debug.Log("Player's animator has been changed.");
            }
            else
            {
                Debug.LogWarning("Player does not have an Animator component.");
            }
        }
        else
        {
            Debug.LogWarning("PlayerInteraction script not found on player.");
        }
    }
}
