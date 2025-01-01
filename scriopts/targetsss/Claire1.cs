using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Claire1 :Interactable
{
    public SpriteRenderer emoteSpriteRenderer;
    // Start is called before the first frame update
    void Start()
    {
         
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        // Check if the colliding object has the tag "Player"
        if (collision.gameObject.CompareTag("Player") && x == 0)
        {
            emoteSpriteRenderer.enabled = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            emoteSpriteRenderer.enabled = false;
        }
    }
    public override void Interact()
    {
       
    }
}
