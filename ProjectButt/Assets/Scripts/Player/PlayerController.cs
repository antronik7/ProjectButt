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
    bool allowHoldJump = true;
    [SerializeField]
    float maxJumpForce = 1f;
    [SerializeField]
    float minJumpForce = 0.25f;
    [SerializeField]
    float initialMovementSpeed = 1f;
    [SerializeField]
    float groundPoundForce = 1f;
    [SerializeField]
    int minimumPlayerDamage = 1;
    [SerializeField]
    int maximumPlayerDamage = 3;
    [SerializeField]
    float percentageDamageMinimum = 0.2f;
    [SerializeField]
    float percentageDamageMaximum = 0.8f;
    [SerializeField]
    LayerMask groundLayer;
    [SerializeField]
    LayerMask wallLayer;
    [SerializeField]
    float raycastLength = 0.6f;

    float currentMovementSpeed;
    float currentDirection;
    float startJumpHeight = 0;
    float startGPHeight = 0;
    float maxJumpY = 0;
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

        float g = rBody.gravityScale * Physics2D.gravity.magnitude;
        float v0 = maxJumpForce / rBody.mass; // converts the jumpForce to an initial velocity
        maxJumpY = (v0 * v0) / (2 * g) - 0.04f;
        Debug.Log(maxJumpY);
    }
	
	// Update is called once per frame
	void Update () {
        // States
        if (playerState == PlayerState.Running)
        {
            Walk();
        }


        // Inputs
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(playerState == PlayerState.Running)
            {
                startJumpHeight = transform.position.y;
                Jump(maxJumpForce);
            }
            else if (playerState == PlayerState.Jumping)
            {
                StartGroundPound();
            }
        }

        if(allowHoldJump && Input.GetKeyUp(KeyCode.Space))
        {
            if(playerState != PlayerState.GroundPounding && rBody.velocity.y > minJumpForce)
            {
                Jump(minJumpForce);
            }
        }

        //Debug.DrawRay(transform.position, Vector3.right * raycastLength, Color.red);
        //Debug.Log(playerState);
    }

    private void FixedUpdate()
    {
        if (GroundCheck())
        {
            if (playerState == PlayerState.GroundPounding)
            {
                playerState = PlayerState.Running;
                RaycastHit2D hitResult = Physics2D.Raycast(transform.position, Vector3.down, raycastLength, groundLayer);// FUNCTION FUNCTION FUNCTION
                hitResult.transform.GetComponent<BlockController>().DamageBlock(CalculateDamage());
            }
            else
            {
                playerState = PlayerState.Running;
            }
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
        startGPHeight = transform.position.y;
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

    int CalculateDamage()
    {
        float heightGP = startGPHeight - startJumpHeight;
        float percentage = heightGP / maxJumpY;

        int damage;
        if (percentage <= percentageDamageMinimum)
            damage = minimumPlayerDamage;
        else if (percentage >= percentageDamageMaximum)
            damage = maximumPlayerDamage;
        else
        {
            int microDamage = maximumPlayerDamage - minimumPlayerDamage;
            float microPercent = (percentage - percentageDamageMinimum) / (percentageDamageMaximum - percentageDamageMinimum);
            damage = ((int)(microDamage * microPercent)) + minimumPlayerDamage;
        }

        return damage;
    }
}
