using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderController : PlayerController
{
    // Logic :: If touching ground/no longer climbing then stop climbing
    private float verticalAxis = 0f;
    private float ladderSpeed = 8f;
    private bool touchingLadder;
    private bool climbingLadder;
    private bool ladderBelow;
    private float stickyX;
    private Rigidbody2D rb2;

    void Awake()
    {
        rb2 = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckLadder();
    }

    void FixedUpdate()
    {
        Climb();
    }

    void CheckLadder()
    {
        verticalAxis = Input.GetAxisRaw("Vertical"); // -1, 0 or 1
        
        if(touchingLadder && Mathf.Abs(verticalAxis) > 0f)
        {
            climbingLadder = true;
        }
    }

    void Climb()
    {
        if(climbingLadder)
        {
            rb2.gravityScale = 0f;
            float newSpeed = ladderSpeed * Time.fixedDeltaTime;
            transform.position = new Vector3(stickyX, transform.position.y);
            rb2.velocity = new Vector2(0f, verticalAxis * newSpeed);
            climbingLadder = !CheckGround();

        } 
        else
        {
            rb2.gravityScale = 0.7f; // default gravity scale for mario
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Ladder"))
        {
            stickyX = collision.gameObject.transform.position.x;
            if (collision.gameObject.transform.position.y < transform.position.y)
            {
                ladderBelow = true;
            } 
            else
            {
                ladderBelow = false;
            }
            touchingLadder = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.CompareTag("Ladder"))
        {
            touchingLadder = false;
            climbingLadder = false;
        }
    }
}
