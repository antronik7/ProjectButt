using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallOfDeathController : MonoBehaviour {

    [SerializeField]
    float speed = 1;

    float currentSpeed;
    bool move = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        // Movement
        if (!move)
            return;

        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, transform.position + Vector3.down, step);
    }

    public void SetWallCanMove(bool canMove)
    {
        move = canMove;
    }

    public void ModifySpeed(float value)
    {
        currentSpeed += value;
    }
}
