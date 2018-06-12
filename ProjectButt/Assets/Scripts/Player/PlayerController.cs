using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    enum PlayerState
    {
        Running,
        Jumping,
        GroundPounding,
        Grounded,
        CrashingTroughBlocks,
        GroundedRecovory,
        WallJumping,
        Turning
    }

    [Header("HP")]
    [SerializeField]
    int playerHP = 1;

    [Header("Run")]
    [SerializeField]
    float initialMovementSpeed = 1f;

    [Header("Jump")]
    [SerializeField]
    bool allowHoldJump = true;
    [SerializeField]
    float maxJumpForce = 1f;
    [SerializeField]
    float minJumpForce = 0.25f;
    [SerializeField]
    float wallJumpFallSpeed = -0.1f;
    [SerializeField]
    float maxWallJumpFallSpeed = -1f;
    [SerializeField]
    float wallJumpForce = 1f;
    [SerializeField]
    float fallMultiplier = 2.5f;
    [SerializeField]
    float lowJumpMultiplier = 2f;


    [Header("Ground Pound")]
    [SerializeField]
    float groundPoundForce = 1f;
    [SerializeField]
    float timeFreezeInAir = 0.25f;
    [SerializeField]
    int minimumPlayerDamage = 1;
    [SerializeField]
    int maximumPlayerDamage = 3;
    [SerializeField]
    float percentageDamageMinimum = 0.2f;
    [SerializeField]
    float percentageDamageMaximum = 0.8f;
    [SerializeField]
    float GroundedRecoverForce = 1.5f;
    [SerializeField]
    float ImpactSleepDuration = 0.05f;

    [Header("Camera Shake")]
    [SerializeField]
    float cameraShakeTime = 1f;
    [SerializeField]
    float cameraShakeSpeed = 1f;
    [SerializeField]
    float cameraShakeMagnitude = 1f;

    [Header("Physic Layers")]
    [SerializeField]
    LayerMask groundLayer;
    [SerializeField]
    LayerMask wallLayer;
    [SerializeField]
    float raycastLength = 0.6f;

    [Header("Death")]
    [SerializeField]
    float deathDelay = 1.0f;
    [SerializeField]
    float deadCameraShakeTime = 1f;
    [SerializeField]
    float deadCameraShakeSpeed = 1f;
    [SerializeField]
    float deadCameraShakeMagnitude = 2f;

    float currentMovementSpeed;
    float currentDirection;
    int currentPlayerHP;
    float previousVelocityY = 0;
    float startJumpHeight = 0;
    float startGPHeight = 0;
    float maxJumpY = 0;
    PlayerState playerState;
    bool disableGameplay = false;
    bool enableRunning = true;
    bool enableJumping = true;
    bool againstWall = false;

    Rigidbody2D rBody;
    Collider2D myCollider;
    float gravityScale;
    Animator animator;
    SpriteRenderer spriteRender;

	// Use this for initialization
	void Start () {
        rBody = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<Collider2D>();
        animator = GetComponent<Animator>();
        spriteRender = GetComponent<SpriteRenderer>();

        playerState = PlayerState.CrashingTroughBlocks;
        gravityScale = rBody.gravityScale;
        currentPlayerHP = playerHP;
        currentMovementSpeed = initialMovementSpeed;
        currentDirection = Mathf.Sign(currentMovementSpeed);

        float g = rBody.gravityScale * Physics2D.gravity.magnitude;
        float v0 = maxJumpForce / rBody.mass; // converts the jumpForce to an initial velocity
        maxJumpY = (v0 * v0) / (2 * g);
        Debug.Log(maxJumpY);
    }
	
	// Update is called once per frame
	void Update () {

        if(rBody.velocity.y < 0)
        {
            rBody.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        else if (rBody.velocity.y > 0 && !(Input.GetKey(KeyCode.Space) || Input.GetMouseButton(0)))
        {
            rBody.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }

        if (disableGameplay)
            return;

        // States
        if (enableRunning && playerState == PlayerState.Running)
        {
            Walk();
        }

        // Inputs
        if (enableJumping && (Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)))
        {
            if(playerState == PlayerState.Running || playerState == PlayerState.GroundedRecovory || playerState == PlayerState.Turning)
            {
                startJumpHeight = transform.position.y;

                if (playerState == PlayerState.GroundedRecovory)
                {
                    Walk();
                }
                else if (playerState == PlayerState.Turning)
                {
                    Walk();
                }
                else
                {
                    Walk();
                }

                Jump(maxJumpForce);
            }
            else if (playerState == PlayerState.WallJumping)
            {
                againstWall = false;
                InverseDirection();
                Walk();
                Jump(wallJumpForce);
            }
            else if (playerState == PlayerState.Jumping)
            {
                StartGroundPound();
            }
        }

        //if(allowHoldJump && (Input.GetKeyUp(KeyCode.Space) || Input.GetMouseButtonUp(0)))
        //{
        //    if(playerState != PlayerState.GroundPounding && playerState != PlayerState.WallJumping && rBody.velocity.y > minJumpForce)
        //    {
        //        Jump(minJumpForce);
        //    }
        //}

        if (!Input.GetKey(KeyCode.Space) && !Input.GetMouseButton(0))
        {
            if(playerState == PlayerState.Grounded)
            {
                rBody.velocity = new Vector3(0f, GroundedRecoverForce, 0f);
                playerState = PlayerState.GroundedRecovory;
                animator.SetTrigger("StopGrounded");
            }
        }

        //Debug.Log(playerState);
        //Debug.DrawRay(transform.position, Vector3.right * raycastLength, Color.red);
        //Debug.Log(playerState);
    }

    private void FixedUpdate()
    {
        if (disableGameplay)
            return;

        bool grounded = GroundCheck();

        if (playerState == PlayerState.WallJumping)
            rBody.velocity = new Vector2(rBody.velocity.x, Mathf.Max(maxWallJumpFallSpeed, rBody.velocity.y + wallJumpFallSpeed));

        if (grounded && rBody.velocity.y <= 0)
        {
            if (playerState == PlayerState.GroundPounding)
            {
                LaunchGroundPoundAoE();
            }
            else if (playerState == PlayerState.CrashingTroughBlocks)
            {
                playerState = PlayerState.Grounded; // FUNCTION FUNCTION FUNCTION
                animator.SetTrigger("Grounded");
            }
            else if (playerState != PlayerState.WallJumping && playerState != PlayerState.Turning && playerState != PlayerState.Grounded && playerState != PlayerState.CrashingTroughBlocks && playerState != PlayerState.Running)
            {
                StartWalking();
            }
        }
        else
        {
            if (playerState == PlayerState.Running)
            {
                rBody.velocity = Vector3.zero;
                playerState = PlayerState.Jumping;
                animator.SetTrigger("Jumping");
                Debug.Log("NONONONONONON");
            }
        }

        if (againstWall)
        {
            if (grounded)
            {
                InverseDirection();
                againstWall = false;
                playerState = PlayerState.Turning;
                animator.SetTrigger("Turning");
            }
            else if(playerState != PlayerState.WallJumping)
            {
                playerState = PlayerState.WallJumping;
                animator.SetTrigger("WallSliding");
            }
        }

        previousVelocityY = rBody.velocity.y;
    }

    void Walk()
    {
        rBody.velocity = new Vector3(currentMovementSpeed, rBody.velocity.y, 0f);
    }

    void Jump(float jumpForce)
    {
        playerState = PlayerState.Jumping;
        animator.SetTrigger("Jumping");
        rBody.velocity = new Vector3(rBody.velocity.x, jumpForce, 0f);
        Debug.Log("Jumping");
    }

    void StartGroundPound()
    {
        startGPHeight = transform.position.y;
        playerState = PlayerState.GroundPounding;
        rBody.velocity = Vector3.zero;
        rBody.gravityScale = 0;
        animator.SetTrigger("GroundPound");
        Invoke("EndGroundPound", timeFreezeInAir);
    }

    void EndGroundPound()
    {
        rBody.gravityScale = gravityScale;
        rBody.velocity = new Vector3(0f, groundPoundForce * -1, 0f);
    }

    public void StartWalking()
    {
        rBody.velocity = Vector3.zero;
        playerState = PlayerState.Running;
        animator.SetTrigger("Running");
    }

    bool GroundCheck()
    {
        if (rBody.velocity.y <= 0)
        {
            if(Physics2D.Raycast(transform.position, Vector3.down, raycastLength, groundLayer))// LOOP LOOP LOOP
            {
                return true;
            }

            Vector3 positionRay = new Vector3(transform.position.x - 0.4375f, transform.position.y, transform.position.z);

            if (Physics2D.Raycast(positionRay, Vector3.down, raycastLength, groundLayer))
            {
                return true;
            }

            positionRay = new Vector3(transform.position.x + 0.4375f, transform.position.y, transform.position.z);

            if (Physics2D.Raycast(positionRay, Vector3.down, raycastLength, groundLayer))
            {
                return true;
            }
        }

        return false;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.layer == 9)
            againstWall = true;
    }

    bool WallCheck()
    {
        if (Physics2D.Raycast(transform.position, Vector3.right * currentDirection, raycastLength * 0.61f, wallLayer))// CHANGER CHANGER CHANGER
        {
            return true;
        }

        return false;
    }

    void LaunchGroundPoundAoE()
    {
        int nbrBlocksDestroyed = 0;

        RaycastHit2D[] blocks = Physics2D.BoxCastAll(new Vector2(transform.position.x, transform.position.y - raycastLength), new Vector2(0.875f, 0.1f), 0, Vector2.zero, 0, groundLayer);// VARIABLE VARIABLE VARIABLE
        int nbrBlocksHit = blocks.Length;
        for (int i = 0; i < nbrBlocksHit; ++i)
        {
            BlockController block = blocks[i].transform.GetComponent<BlockController>();

            if(block != null)// MAYBE USE TAG
            {
                block.DamageBlock(CalculateDamage(), ImpactSleepDuration, previousVelocityY);
                if (block.GetCurrentHp() <= 0)
                {
                    ++nbrBlocksDestroyed;
                }
            }
        }

        if (nbrBlocksDestroyed == nbrBlocksHit)
        {
            playerState = PlayerState.CrashingTroughBlocks;
        }
        else
        {
            playerState = PlayerState.Grounded;// FUNCTION FUNCTION FUNCTION
            animator.SetTrigger("Grounded");
        }

        if (nbrBlocksHit > 0)
            CameraShaker.instance.startCameraShake(cameraShakeTime, cameraShakeSpeed, cameraShakeMagnitude);
    }

    void InverseDirection()
    {
        currentDirection *= -1;
        currentMovementSpeed *= -1;
        spriteRender.flipX = !spriteRender.flipX;
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

    public void DamagePlayer(int damageValue)
    {
        currentPlayerHP -= damageValue;

        if (currentPlayerHP <= 0)
            KillPlayer();
    }

    void KillPlayer()
    {
        GameManager.instance.PlayerGotKill();
        DisablePlayerGameplay();
        rBody.velocity = Vector3.zero;
        rBody.gravityScale = 0;
        myCollider.enabled = false;
        CameraShaker.instance.startCameraShake(deadCameraShakeTime, deadCameraShakeSpeed, deadCameraShakeMagnitude);
        Invoke("PlayerDeath", deathDelay);
    }

    void PlayerDeath()
    {
        rBody.gravityScale = 3;// VARIABLE VARIABLE VARIABLE
        rBody.velocity = new Vector3(0f, 10f, 0f); // VARIABLE VARIABLE VARIABLE
        animator.SetTrigger("Dead");
    }

    public void EnablePlayerGameplay()
    {
        disableGameplay = false;
    }

    public void DisablePlayerGameplay()
    {
        disableGameplay = true;
    }

    public void EnablePlayerRunning()
    {
        enableRunning = true;
    }

    public void DisablePlayerRunning()
    {
        enableRunning = false;
        rBody.velocity = Vector3.zero;
    }

    public void EnablePlayerJumping()
    {
        enableJumping = true;
    }

    public void DisablePlayerJumping()
    {
        enableJumping = false;
    }

    public void SetVelocity(float velocity)
    {
        rBody.velocity = new Vector3(0f, velocity, 0f);
    }
}
