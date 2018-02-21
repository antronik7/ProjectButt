using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    enum PlayerState
    {
        Running,
        Jumping,
        GroundPounding
    }

    [SerializeField]
    float maxJumpForce = 1f;
    [SerializeField]
    float minJumpForce = 0.25f;
    [SerializeField]
    float initialMovementSpeed = 1f;
    [SerializeField]
    float groundPoundForce = 1f;
    [SerializeField]
    LayerMask groundLayer;
    [SerializeField]
    LayerMask wallLayer;
    [SerializeField]
    float raycastLength = 0.6f;

    float currentMovementSpeed;
    float currentDirection;
    PlayerState playerState;

    Rigidbody2D rBody;
    float gravityScale;
    Animator animator;

	// Use this for initialization
	void Start () {
        rBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        playerState = PlayerState.Jumping;
        rBody.velocity = new Vector3(initialMovementSpeed, rBody.velocity.y, 0f);
        gravityScale = rBody.gravityScale;
        currentMovementSpeed = initialMovementSpeed;
        currentDirection = Mathf.Sign(currentMovementSpeed);
    }
	
	// Update is called once per frame
	void Update () {
        if(playerState == PlayerState.Running)
        {
            Walk();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(playerState == PlayerState.Running)
            {
                Jump(maxJumpForce);
            }
            else if (playerState == PlayerState.Jumping)
            {
                StartGroundPound();
            }
        }

        if(Input.GetKeyUp(KeyCode.Space))
        {
            if(playerState != PlayerState.GroundPounding && rBody.velocity.y > minJumpForce)
            {
                Jump(minJumpForce);
            }
        }

        Debug.DrawRay(transform.position, Vector3.right * raycastLength, Color.red);
        Debug.Log(playerState);
    }

    private void FixedUpdate()
    {
        if (GroundCheck())
        {
            playerState = PlayerState.Running;
        }

        if (WallCheck())
        {
            currentDirection *= -1;
            currentMovementSpeed *= -1;
            rBody.velocity = new Vector3(currentMovementSpeed, rBody.velocity.y, 0f);
        }
    }

    void Walk()
    {
        rBody.velocity = new Vector3(currentMovementSpeed, rBody.velocity.y, 0f);
    }

    void Jump(float jumpForce)
    {
        playerState = PlayerState.Jumping;
        rBody.velocity = new Vector3(rBody.velocity.x, jumpForce, 0f);
    }

    void StartGroundPound()
    {
        playerState = PlayerState.GroundPounding;
        rBody.velocity = Vector3.zero;
        rBody.gravityScale = 0;
        animator.SetTrigger("GroundPound");
    }

    void EndGroundPound()
    {
        animator.SetTrigger("StopGroundPound");
        rBody.gravityScale = gravityScale;
        rBody.velocity = new Vector3(0f, groundPoundForce * -1, 0f);
    }

    bool GroundCheck()
    {
        if (rBody.velocity.y <= 0 && Physics2D.Raycast(transform.position, Vector3.down, raycastLength, groundLayer))
        {
            return true;
        }

        return false;
    }

    bool WallCheck()
    {
        if (Physics2D.Raycast(transform.position, Vector3.right * currentDirection, raycastLength, wallLayer))
        {
            return true;
        }

        return false;
    }
}
