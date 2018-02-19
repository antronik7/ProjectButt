using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField]
    float maxJumpForce = 1f;
    [SerializeField]
    float minJumpForce = 0.25f;
    [SerializeField]
    LayerMask groundLayer;
    [SerializeField]
    float groundRaycastLength = 0.6f;


    Rigidbody2D rBody;

	// Use this for initialization
	void Start () {
        rBody = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Space) && GroundCheck())
        {
            rBody.velocity = new Vector3(rBody.velocity.x, maxJumpForce, 0f); 
        }

        if(Input.GetKeyUp(KeyCode.Space) && rBody.velocity.y > minJumpForce)
        {
            rBody.velocity = new Vector3(rBody.velocity.x, minJumpForce, 0f);
        }

        Debug.DrawRay(transform.position, Vector3.down * groundRaycastLength, Color.red);
    }
      
    bool GroundCheck()
    {
        if (Physics2D.Raycast(transform.position, Vector3.down, groundRaycastLength, groundLayer))
        {
            return true;
            Debug.Log("wtf");
        }
        Debug.Log("sa marche pas");
        return false;
    }
}
