using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float movementInputDirection;

    private bool isFacingRight = true;
    private bool isFalling = false;

    private bool isWalking = false;


    private Rigidbody2D rb;
    private Animator anim;
    
    private float movementSpeed = 0.0f;
    // public float maxMovementSpeed = 10.0f;
    public float jumpSpeed = 1.0f;
    public float dropSpeed = 0.0f;

    //LIMITS AND PROPERTIES
    public float MAX_MOVE_SPEED = 20.0f;
    public float ACCELERATION_RATE = 0.5f;


    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
        CheckInput();
        CheckPlayerDirection();
        CheckForJump();
        UpdateAnimations();
    }

    private void FixedUpdate()
    {
        if (Mathf.Abs(rb.velocity.y) < 0.001f)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0.0f);
        }
        CheckPlayerFall();
        ApplyMovement();
    }

    private void UpdateAnimations()
    {
        anim.SetBool("isWalking",isWalking);
    }

    private void CheckForJump()
    {
        float yVal = Input.GetAxisRaw("Vertical");
        if(Input.GetKeyDown(KeyCode.Space)){
            Debug.Log("SPACE ENTERED");
            if(isFalling == false){
                InitiateJump();
            }
            

        }else if(Input.GetKeyUp(KeyCode.Space))
        {
            Debug.Log("SPACE RELEASED -- within ELSE block");
            StopJump();
        }

    }

    private void InitiateJump()
    {
        rb.velocity = new Vector2(rb.velocity.x,jumpSpeed );
    }

    private void StopJump()
    {   
        rb.velocity = new Vector2(rb.velocity.x, 1.0f);
    }

    private void CheckPlayerDirection()
    {
        if(isFacingRight && movementInputDirection < 0)
        {
            FlipPlayer();
        }else if(!isFacingRight && movementInputDirection > 0)
        {
            FlipPlayer();
        }

        if(movementInputDirection == 0){
            //resetMovement speed
            movementSpeed = 0.0f;
        }

        Debug.Log("CHECK rb.velocity.x --> " + rb.velocity.x.ToString());
        if(rb.velocity.x !=0)
        {
            isWalking = true;
        }
        else
        {
            isWalking = false;
        }
        
    }

    private void CheckPlayerFall(){
        // Debug.Log("Check Fall-->" + Input.GetAxisRaw("Vertical").ToString());
        Debug.Log("Check Fall by RB-->" + rb.velocity.y.ToString());
        dropSpeed = rb.velocity.y;
        if(  rb.velocity.y < 0.0f){
            Debug.Log("isFalling = true");
            isFalling = true; 

        }else{
            Debug.Log("isFalling = false");
            isFalling = false;
        }
    }

    private void FlipPlayer()
    {
        isFacingRight = !isFacingRight;
        transform.Rotate(0.0f, 180.0f, 0.0f);  
    }
    

    private void CheckInput()
    {
        movementInputDirection = Input.GetAxisRaw("Horizontal");
        // Debug.Log("InputDirection-->" + movementInputDirection.ToString());
    }

    private void ApplyMovement()
    {
        // Debug.Log("Applying movement-->" + movementInputDirection.ToString());
        //Calculate movement speed
        if(movementSpeed < MAX_MOVE_SPEED && movementSpeed >= 0.0f){
            movementSpeed += ACCELERATION_RATE;
        }
        rb.velocity = new Vector2(movementSpeed * movementInputDirection, rb.velocity.y);
    }
}
