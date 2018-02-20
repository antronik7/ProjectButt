using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField]
    float maxJumpForce = 1f;
    [SerializeField]
    float minJumpForce = 0.25f;
    [SerializeField]
    float initialMovementSpeed = 1f;
    [SerializeField]
    LayerMask groundLayer;
    [SerializeField]
    LayerMask wallLayer;
    [SerializeField]
    float raycastLength = 0.6f;

    float currentMovementSpeed;
    float currentDirection;
    Rigidbody2D rBody;

	// Use this for initialization
	void Start () {
        rBody = GetComponent<Rigidbody2D>();
        rBody.velocity = new Vector3(initialMovementSpeed, rBody.velocity.y, 0f);
        currentMovementSpeed = initialMovementSpeed;
        currentDirection = Mathf.Sign(currentMovementSpeed);
    }
	
	// Update is called once per frame
	void Update () {
        rBody.velocity = new Vector3(currentMovementSpeed, rBody.velocity.y, 0f);

        if(WallCheck())
        {
            currentDirection *= -1;
            currentMovementSpeed *= -1;
            rBody.velocity = new Vector3(currentMovementSpeed, rBody.velocity.y, 0f);
        }

        if (Input.GetKeyDown(KeyCode.Space) && GroundCheck())
        {
            rBody.velocity = new Vector3(rBody.velocity.x, maxJumpForce, 0f); 
        }

        if(Input.GetKeyUp(KeyCode.Space) && rBody.velocity.y > minJumpForce)
        {
            rBody.velocity = new Vector3(rBody.velocity.x, minJumpForce, 0f);
        }

        Debug.DrawRay(transform.position, Vector3.right * raycastLength, Color.red);
    }
      
    bool GroundCheck()
    {
        if (Physics2D.Raycast(transform.position, Vector3.down, raycastLength, groundLayer))
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
