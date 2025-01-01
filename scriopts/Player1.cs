using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player1 : MonoBehaviour
{

    //interaction
    public GameObject interactButton, goup, godown, goleft, goright;

    private Interactable currentInteractable;
    public int x = 0;


    //movement
    public Rigidbody rb;
    public float forceAmount = 10f;
    public Animator animator;
    private bool moveUp, moveDown, moveRight, moveLeft;

    // Start is called before the first frame update
    void Start()
    {   //hide interactbuuton
       
        if (rb == null)
        {
            rb = GetComponent<Rigidbody>();  // Initialize Rigidbody if not assigned
            UnityEngine.Debug.Log("Rigidbody not assigned in the inspector. Getting Rigidbody component.");
        }
        if (animator == null)
        {
            animator = GetComponent<Animator>();  // Initialize Animator if not assigned
            UnityEngine.Debug.Log("Animator not assigned in the inspector. Getting Animator component.");
        }
        UnityEngine.Debug.Log("Start method called. Rigidbody initialized.");
    }

    // Update is called once per frame
    void Update()
    {
        if (moveUp)
        {
            rb.AddForce(Vector3.up * forceAmount * Time.deltaTime, ForceMode.Force);
            animator.SetBool("isMovingUp", true);
            UnityEngine.Debug.Log("Moving Up");
        }
        else
        {
            animator.SetBool("isMovingUp", false);
        }

        if (moveDown)
        {
            rb.AddForce(Vector3.down * forceAmount * Time.deltaTime, ForceMode.Force);
            animator.SetBool("isMovingDown", true);
            UnityEngine.Debug.Log("Moving Down");
        }
        else
        {
            animator.SetBool("isMovingDown", false);
        }

        if (moveRight)
        {
            rb.AddForce(Vector3.right * forceAmount * Time.deltaTime, ForceMode.Force);
            animator.SetBool("isMovingRight", true);
            UnityEngine.Debug.Log("Moving Right");
        }
        else
        {
            animator.SetBool("isMovingRight", false);
        }

        if (moveLeft)
        {
            rb.AddForce(Vector3.left * forceAmount * Time.deltaTime, ForceMode.Force);
            animator.SetBool("isMovingLeft", true);
            UnityEngine.Debug.Log("Moving Left");
        }
        else
        {
            animator.SetBool("isMovingLeft", false);
        }
    }
  
    public void OnPointerDown(string direction)
    {
        switch (direction)
        {
            case "Up": moveUp = true; UnityEngine.Debug.Log("Pointer Down: Up"); break;
            case "Down": moveDown = true; UnityEngine.Debug.Log("Pointer Down: Down"); break;
            case "Right": moveRight = true; UnityEngine.Debug.Log("Pointer Down: Right"); break;
            case "Left": moveLeft = true; UnityEngine.Debug.Log("Pointer Down: Left"); break;
        }
    }

    public void OnPointerUp(string direction)
    {
        switch (direction)
        {
            case "Up": moveUp = false; UnityEngine.Debug.Log("Pointer Up: Up"); break;
            case "Down": moveDown = false; UnityEngine.Debug.Log("Pointer Up: Down"); break;
            case "Right": moveRight = false; UnityEngine.Debug.Log("Pointer Up: Right"); break;
            case "Left": moveLeft = false; UnityEngine.Debug.Log("Pointer Up: Left"); break;
        }
    }

}