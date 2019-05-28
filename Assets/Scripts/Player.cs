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

    private SpriteRenderer rend;
    public CharacterController2D controller;
    private Animator anim;
    private Vector3 motion;

    // Start is called before the first frame update
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

        if (!controller.isGrounded)
        {
            // Apply gravity
            motion.y += gravity * Time.deltaTime;
        }

        // If space is pressed
        if (Input.GetButtonDown("Jump"))
        {
            // Make the player jump
            Jump();
        }

        // Climb up or down depending on the Y value
        Climb(inputV);
        // Move left or right depending on the X value
        Move(inputH);
        // Aply movement with motion
        controller.move(motion * Time.deltaTime);
    }

    public void Move(float inputH)
    {
        motion.x = inputH * movementSpeed; // move left and right
        anim.SetBool("IsRunning", inputH != 0);
        rend.flipX = inputH < 0;
    }
    public void Climb(float inputV)
    {

    }
    public void Hurt()
    {

    }
    public void Jump()
    {
        motion.y = jumpHeight;
    }
}

