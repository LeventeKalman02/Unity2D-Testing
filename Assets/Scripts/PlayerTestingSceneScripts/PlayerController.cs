using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody2D rb;
    
    [SerializeField] private float moveSpeed = 150f;
    [SerializeField] private float jumpSpeed = 5f;

    //variables for jumping
    private bool canJump = true;

    //using raycasting to check for ground, basically just a laser set under the player to detect the ground
    //raycasting to detect if player is on the ground
    [SerializeField] private float rayLength;
    [SerializeField] private float rayPosOffset;

    Vector3 rayPositionCenter;
    Vector3 rayPositionRight;
    Vector3 rayPositionLeft;

    RaycastHit2D[] GroundHitsCenter;
    RaycastHit2D[] GroundHitsRight;
    RaycastHit2D[] GroundHitsLeft;

    RaycastHit2D[][] allRayCastHits = new RaycastHit2D[3][];

    private void Awake()
    {
        //get the rigidbody component
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        Jump();
    }
    /*
     * jumping with raycasting doesnt work
     * give a second look and find a different way to detect ground
     * 
     */
    private void Jump()
    {
        //setting the position of the rays
        rayPositionCenter = transform.position + new Vector3(0, rayLength * 0.5f, 0);
        rayPositionRight = transform.position + new Vector3(rayPosOffset, rayLength * 0.5f, 0);
        rayPositionLeft = transform.position + new Vector3(-rayPosOffset, rayLength * 0.5f, 0);

        //check to see if the ray is detecting the ground
        GroundHitsCenter = Physics2D.RaycastAll(rayPositionCenter, -Vector2.up, rayLength);
        GroundHitsRight = Physics2D.RaycastAll(rayPositionRight, -Vector2.up, rayLength);
        GroundHitsLeft = Physics2D.RaycastAll(rayPositionLeft, -Vector2.up, rayLength);

        allRayCastHits[0] = GroundHitsCenter;
        allRayCastHits[1] = GroundHitsRight;
        allRayCastHits[2] = GroundHitsLeft;

        //stores result of groundCheck
        canJump = GroundCheck(allRayCastHits);

        //for debugging to see rays in game
        Debug.DrawRay(rayPositionCenter, -Vector2.up * rayLength, Color.red);
        Debug.DrawRay(rayPositionRight, -Vector2.up * rayLength, Color.red);
        Debug.DrawRay(rayPositionLeft, -Vector2.up * rayLength, Color.red);
    }

    //iterate through all raycastHits to check for ground and return a bool
    private bool GroundCheck(RaycastHit2D[][] groundHits)
    {
        foreach (RaycastHit2D[] hitList in groundHits)
        {
            foreach (RaycastHit2D hit in hitList)
            {
                if (hit.collider != null)
                {
                    if (hit.collider.tag != "PlayerCollider")
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    private void Movement()
    {
        //move player to the left
        if(Input.GetAxisRaw("Horizontal") > 0)
        {
            rb.velocity = new Vector2(moveSpeed * Time.fixedDeltaTime, rb.velocity.y);
        }
        //move player to the right
        else if(Input.GetAxisRaw("Horizontal") < 0)
        {
            rb.velocity = new Vector2(-moveSpeed * Time.fixedDeltaTime, rb.velocity.y);
        }
        else
        {
            rb.velocity = new Vector2(0, rb.velocity.y);
        }
        //jumping
        if (Input.GetKey(KeyCode.Space) && canJump == true)
        {
            rb.velocity = new Vector2(rb.velocity.x, 0);
            rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
        }
    }
}
