using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shortable : Interactable
{
    public GameObject paperPrefab;
    public Canvas mainCanvas;
    
    private GameObject instantiatedPaper;

    // This method would be called when the player interacts with the character
    public override void Interact()
    {
        Player Playerscript = player.GetComponent<Player>();
        // Check if the paper is already instantiated
        if (instantiatedPaper == null)
        {
            Playerscript.deactivatecontrolles();
            // Instantiate the paper prefab as a child of the Canvas
            instantiatedPaper = Instantiate(paperPrefab, mainCanvas.transform);

            // Optionally, set its position, size, etc.
            instantiatedPaper.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
        }
        else
        {
            Playerscript.deactivatecontrolles();
            Destroy(instantiatedPaper);
          
        }
    }
    
  
}
