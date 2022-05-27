using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderController : PlayerController
{
    // Logic :: If touching ground/no longer climbing then stop climbing
    private float verticalAxis = 0f;
    private float ladderSpeed = 0.5f;
    private bool touchingLadder;
    private bool climbingLadder;
    private bool ladderBelow;
    private float ladderCenterX;
    private Rigidbody2D rb2;
    private GameObject ladderObj;

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
       
        if(touchingLadder)
        {
            ladderBelow = ladderObj.transform.position.y < transform.position.y;
            if (verticalAxis > 0f && !ladderBelow) { climbingLadder = true; } 
            else if (verticalAxis < 0f && ladderBelow){ climbingLadder = true; }
        }
    }

    void Climb()
    {
        if(climbingLadder)
        {
            rb2.velocity = Vector2.zero;
            rb2.gravityScale = 0f;
            rb2.isKinematic = true;
            transform.position = new Vector3(ladderCenterX, transform.position.y + (ladderSpeed * verticalAxis * Time.fixedDeltaTime));
            climbingLadder = !CheckGround();
        } 
        else
        {
            rb2.isKinematic = false;
            rb2.gravityScale = 0.7f; // default gravity scale for mario
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Ladder") && CheckGround())
        {
            ladderObj = collision.gameObject;
            ladderCenterX = ladderObj.transform.position.x;
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
