using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] float runSpeed = 1f;
    [SerializeField] float jumpSpeed = 5f;
    private Vector2 moveInput;
    bool playerHasHorizontalSpeed = false;

    private Rigidbody2D myRigidbody;
    private Animator myAnimator;
    private CapsuleCollider2D myCollider;

    void Start()
    {
        myRigidbody = this.GetComponent<Rigidbody2D>();
        myAnimator = this.GetComponent<Animator>();
        myCollider = this.GetComponent<CapsuleCollider2D>();
    }

    void Update()
    {
        playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        Run();
        FlipSprite();
    }

    void OnMove(InputValue value) 
    { 
        moveInput = value.Get<Vector2>();
    }
    void OnJump(InputValue value)
    {
        if (value.isPressed && myCollider.IsTouchingLayers(LayerMask.GetMask("Ground")) )
        {
            myRigidbody.velocity += new Vector2(0, jumpSpeed);
        }
    }

    void Run()
    {
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

}
