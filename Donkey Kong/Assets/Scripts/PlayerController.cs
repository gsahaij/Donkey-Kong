using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Physics Constraints
    [SerializeField]
    private float runSpeed = 35f;
    [SerializeField] 
    private float jumpHeight = 50f;

    // Basic Components
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer spriteRenderer;

    // Used to track which way the player should go
    private float horizontalAxis = 0f;

    // Tracks if the player is touching the ground
    private bool isGrounded;

    // Ground Collider Transforms
    public Transform groundCheckM;
    public Transform groundCheckL;
    public Transform groundCheckR;
    void Awake()
    {
        // Get the components from the player
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        // Check the speed the player should be at
        CalculateHorizontalSpeed();
        // Allow player to jump
        Jump();
    }

    void FixedUpdate()
    {
        // Check if the player is on the ground
        isGrounded = CheckGround();
        // Then change animation state based on that
        AnimationState();
        // Lastly allow the player to move
        Move();
    }

    void AnimationState()
    {
        // On Ground and Not Moving (Idle)
        if(isGrounded && horizontalAxis == 0) { animator.Play("mario_idle"); }

        // On Ground and Moving (Run)
        if(isGrounded && Mathf.Abs(horizontalAxis) > 0) { animator.Play("mario_run"); }
        
        // Not On Ground (Jump)
        if(!isGrounded) { animator.Play("mario_jump"); }
    }

    void CalculateHorizontalSpeed()
    {
        // Horizontal Axis calculations
        horizontalAxis = Input.GetAxisRaw("Horizontal") * runSpeed;

        // Sprite flipping based on which direction was held
        if(isGrounded)
        {
            if (horizontalAxis > 0) { spriteRenderer.flipX = true; }
            else if (horizontalAxis < 0) { spriteRenderer.flipX = false; }
        }
    }

    bool CheckGround()
    {
        // Using Layers to Linecast from the player's position to the ground check's position to see if there is a layer called Ground
        // Three checks are used for precision
        if(Physics2D.Linecast(transform.position, groundCheckM.transform.position, 1 << LayerMask.NameToLayer("Ground")) ||
           Physics2D.Linecast(transform.position, groundCheckL.transform.position, 1 << LayerMask.NameToLayer("Ground")) ||
           Physics2D.Linecast(transform.position, groundCheckR.transform.position, 1 << LayerMask.NameToLayer("Ground")))
        {
            return true;
        }
        return false;
    }

    void Jump()
    {
        // Allow the player to Jump if they're holding the Jump button and they're on the ground
        if(isGrounded && Input.GetButtonDown("Jump")) 
        {
            // Calculate the new y velocity for the rigidbody
            Vector2 newVelocity = new Vector2(rb.velocity.x, jumpHeight * Time.fixedDeltaTime);
            rb.velocity = newVelocity;
        }
    }

    void Move()
    {
        // Allow the player to Move if they're on the ground
        if (isGrounded)
        {
            // Calculate the new x velocity for the rigidbody
            Vector2 newVelocity = new Vector2(horizontalAxis * Time.fixedDeltaTime, rb.velocity.y);
            rb.velocity = newVelocity;
        }
    }
}
