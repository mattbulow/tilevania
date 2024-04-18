using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField] float runSpeed = 1f;
    [SerializeField] float jumpSpeed = 5f;
    [SerializeField] float climbSpeed = 1f;
    [SerializeField] float deathSpeed = 5f;
    private Vector2 moveInput;
    bool playerHasHorizontalSpeed = false;
    bool playerHasVerticalSpeed = false;
    private float defaultGravity;
    private float defaultClimbAnimationSpeed;
    private bool isAlive = true;

    private Rigidbody2D myRigidbody;
    private Animator myAnimator;
    private Collider2D myFeetCollider;
    private Collider2D myBodyCollider;
    [SerializeField] private GameObject stateDrivenCameras;
    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform gun;

    void Start()
    {
        myRigidbody = this.GetComponent<Rigidbody2D>();
        myAnimator = this.GetComponent<Animator>();
        myFeetCollider = this.GetComponent<BoxCollider2D>();
        myBodyCollider = this.GetComponent<CapsuleCollider2D>();

        defaultGravity = myRigidbody.gravityScale;
        defaultClimbAnimationSpeed = myAnimator.speed;

    }

    void Update()
    {
        if (isAlive)
        {
            playerHasHorizontalSpeed = Mathf.Abs(myRigidbody.velocity.x) > 0.1;
            playerHasVerticalSpeed = Mathf.Abs(myRigidbody.velocity.y) > 0.1;
            Move();
        }  

        if (transform.position.y < -20)
        {
            myRigidbody.simulated = false;
        }

    }

    // This method is called by the input manager
    void OnMove(InputValue value) 
    { 
        moveInput = value.Get<Vector2>();
    }

    // This method is called by the input manager
    void OnJump(InputValue value)
    {
        if (value.isPressed && myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Ground")) )
        {
            myRigidbody.velocity += new Vector2(0, jumpSpeed);
        }
    }

    void OnFire(InputValue value)
    {
        GameObject bulletObj = Instantiate(bullet, gun.position,bullet.transform.rotation);
        bulletObj.transform.localScale = 
            new Vector2(
            bulletObj.transform.localScale.x*Mathf.Sign(this.transform.localScale.x), 
            bulletObj.transform.localScale.y);
    }

    void Move()
    {
        ClimbLadder();

        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, myRigidbody.velocity.y);
        myRigidbody.velocity = playerVelocity;
        myAnimator.SetBool("isRunning", playerHasHorizontalSpeed);

        FlipSprite();
    }
    
    void FlipSprite()
    {
        // flip sprite based on player velocity and/or input 
        if (moveInput.x < 0)
        {
            transform.localScale = new Vector2(-1, 1);
        }
        else if (moveInput.x > 0)
        {
            transform.localScale = new Vector2(1, 1);
        }
    }

    void ClimbLadder()
    {
        
        // if player is touching ladder layer, then set vertical velocity based on y-axis input
        if (myFeetCollider.IsTouchingLayers(LayerMask.GetMask("Climbing")))
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Player collided with: " + collision.transform.name + " on Layer: " + LayerMask.LayerToName(collision.gameObject.layer));
        if (collision.gameObject.layer == LayerMask.NameToLayer("Enemy") ||
            collision.gameObject.layer == LayerMask.NameToLayer("Hazard"))
        {
            Die();
            // set cameras to follow enemy that killed the player
            CinemachineVirtualCamera[] virtualCameras = stateDrivenCameras.GetComponentsInChildren<CinemachineVirtualCamera>();
            foreach (var camera in virtualCameras)
            {
                camera.Follow = null;
            }
        }
    }

    void Die()
    {
        if (isAlive)
        {
            myRigidbody.velocity = (new Vector2(0, deathSpeed));
            myAnimator.speed = 0;
            this.transform.Rotate(0, 0, 180);
            myBodyCollider.enabled = false;
            myFeetCollider.enabled = false;
            Invoke("ProcessPlayerDeath", 1f);
        }
        isAlive = false;

    }
    void ProcessPlayerDeath()
    {
        FindObjectOfType<GameSession>().ProcessPlayerDeath();
    }

}
