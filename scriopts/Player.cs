using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{

    //interaction
    public GameObject interactButton, goup, godown, goleft, goright;
   
    private Interactable currentInteractable;
    public int x = 0;


    //movement
    public Rigidbody rb;
    public float forceAmount = 10f;
    public Animator animator;
    public bool moveUp, moveDown, moveRight, moveLeft,controlesactive;

    private PlayerControls inputActions;


    // Start is called before the first frame update
    void Start()
    {   //hide interactbuuton
        interactButton.SetActive(false);
        godown.SetActive(false);
        goup.SetActive(false);
        goleft.SetActive(false);
        controlesactive = true;
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
    public void activatecontrolles()
    {

        x = x + 1;
        godown.SetActive(true);
        goright.SetActive(false);
        moveRight = false;
        if (x == 2)
        {
            transform.position = new Vector3(transform.position.x, 0.63f, transform.position.z);
            godown.SetActive(false);
            moveDown = false;
            interactButton.SetActive(true);
        }
        if (x > 2)
        {
            
            godown.SetActive(true);
            goright.SetActive(true);
            goup.SetActive(true);
            goleft.SetActive(true);
          
        }
        



    }
    public void deactivatecontrolles()
    {
        x++;
        if (controlesactive)
        {
            controlesactive = false;
            godown.SetActive(false);
            goright.SetActive(false);
            goup.SetActive(false);
            goleft.SetActive(false);
        }
       else
        {
            godown.SetActive(true);
            goright.SetActive(true);
            goup.SetActive(true);
            goleft.SetActive(true);
            controlesactive = true;
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
