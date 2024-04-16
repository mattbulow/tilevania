using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] float runSpeed = 1f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 1f;
    private Vector2 moveInput;
    bool playerHasHorizontalSpeed = false;
    bool playerHasVerticalSpeed = false;
    private float defaultGravity;
    private float defaultClimbAnimationSpeed;

    private Rigidbody2D myRigidbody;
    private Animator myAnimator;
    private Collider2D myCollider;

    void Start()
    {
        myRigidbody = this.GetComponent<Rigidbody2D>();
        myAnimator = this.GetComponent<Animator>();
        myCollider = this.GetComponent<BoxCollider2D>();


        defaultGravity = myRigidbody.gravityScale;
        defaultClimbAnimationSpeed = myAnimator.speed;
    }

    void Update()
    {
        playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        playerHasVerticalSpeed = Mathf.Abs(myRigidbody.velocity.y) > 0.1;
        Run();
        FlipSprite();
    }

    // This method is called by the input manager
    void OnMove(InputValue value) 
    { 
        moveInput = value.Get<Vector2>();
    }

    // This method is called by the input manager
    void OnJump(InputValue value)
    {
        if (value.isPressed && myCollider.IsTouchingLayers(LayerMask.GetMask("Ground")) )
        {
            myRigidbody.velocity += new Vector2(0, jumpSpeed);
        }
    }

    void Run()
    {
        ClimbLadder();

        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, myRigidbody.velocity.y);
        myRigidbody.velocity = playerVelocity;
        myAnimator.SetBool("isRunning", playerHasHorizontalSpeed);

    }
    
    void FlipSprite()
    {
        // flip sprite based on player velocity and/or input 
        if (playerHasHorizontalSpeed) 
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x), 1);
        }
        
    }

    void ClimbLadder()
    {
        
        // if player is touching ladder layer, then set vertical velocity based on y-axis input
        if (myCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
        {
            myRigidbody.gravityScale = 0;
            Vector2 playerVelocity = new Vector2(myRigidbody.velocity.x, moveInput.y * climbSpeed);
            myRigidbody.velocity = playerVelocity;
            if (playerHasVerticalSpeed)
            {
                myAnimator.speed = defaultClimbAnimationSpeed;
                myAnimator.SetBool("isClimbing", true);
                Debug.Log("Player has vertical speed and is touching ladder layer");
            }
            else
            {
                if (myAnimator.GetBool("isClimbing"))
                {
                    myAnimator.speed = 0;
                }
            }
        }
        else
        {
            myRigidbody.gravityScale = defaultGravity;
            myAnimator.SetBool("isClimbing", false);
            myAnimator.speed = defaultClimbAnimationSpeed;
        }           
    }


}
