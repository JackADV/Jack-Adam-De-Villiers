using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Prime31;

[RequireComponent(typeof(CharacterController2D))]
public class Player : MonoBehaviour
{


    public float movementSpeed = 10f;
    public float gravity = 10f;
    public float jumpHeight = 8f;
    public float centreRadius = .5f;

    private SpriteRenderer rend;
    public CharacterController2D controller;
    private Animator anim;
    private Vector3 velocity;
    private bool isClimbing = false;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, centreRadius);
    }
    void Start()
    {
        controller = GetComponent<CharacterController2D>();
        anim = GetComponent<Animator>();
        rend = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float inputH = Input.GetAxis("Horizontal"); // get Horizontal input (A/D or Left and Right Arrows)
        float inputV = Input.GetAxis("Vertical");

        // If the character is not grounded and is not climbing
        if (!controller.isGrounded &&
            !isClimbing)
        {
            // Apply gravity
            velocity.y += gravity * Time.deltaTime;
        }

        // If space is pressed
        if (Input.GetButtonDown("Jump"))
        {
            // Make the player jump
            Jump();
        }
        anim.SetBool("IsGrounded", controller.isGrounded);
        anim.SetFloat("JumpY", velocity.y);
        // Climb up or down depending on the Y value
        Climb(inputV, inputH);
        // Move left or right depending on the X value
        Move(inputH);
        if (!isClimbing)
        {
            // Aply movement with motion
            controller.move(velocity * Time.deltaTime);
        }

    }

    public void Move(float inputH)
    {
        velocity.x = inputH * movementSpeed; // move left and right
        anim.SetBool("IsRunning", inputH != 0);
        if(inputH != 0)
        {
            rend.flipX = inputH < 0;
        }
        
    }
    public void Climb(float inputV, float inputH)
    {
        bool isOverLadder = false; // Is the player overlapping the ladder?
        Vector3 inputDir = new Vector3(inputH, inputV, 0);
        #region part 1 - detecting Ladders
        // get a list of all hit objects overlapping piont
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, centreRadius);
        // Loop through all hit objects
        foreach (var hit in hits)
        {
            if (hit.tag == "Ground")
            {
                isClimbing = false;
                isOverLadder = false;
                break;
            }

            // Check if tagged "Ladder"
            if (hit.tag == "Ladder")
            {
                // Player is Overlapping a Ladder
                isOverLadder = true;
                break; // Exit just the foreach loop (works on any loop)
            }
            
        }

        // If the player is overlapping and input vertical is made
        if (isOverLadder && inputV != 0)
        {
            anim.SetBool("IsClimbing", true);
            // The player is in climbing state
            isClimbing = true;
        }

        #endregion
        #region Part 2 - Translating the player

        // If player is climbing
        if (isClimbing)
        {
            velocity.y = 0;
            // Move player up and down on the ladder (additionally move left and right)
            transform.Translate(inputDir * movementSpeed * Time.deltaTime);
        }

        #endregion

        if (!isOverLadder)
        {
            anim.SetBool("IsClimbing", false);
            isClimbing = false;
        }
       
        anim.SetFloat("ClimbSpeed", inputDir.magnitude * movementSpeed);
    }
    public void Hurt()
    {

    }
    public void Jump()
    {
        velocity.y = jumpHeight;
    }
}

