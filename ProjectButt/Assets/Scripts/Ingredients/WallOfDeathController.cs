using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallOfDeathController : MonoBehaviour {

    [SerializeField]
    float speed = 1;
    [SerializeField]
    float outOfBoundsMultiplayer = 2;
    [SerializeField]
    float outOfBoundsMaximum = 3;

    float currentSpeed;
    float cameraOrthoSize;
    bool move = false;

	// Use this for initialization
	void Start () {
        cameraOrthoSize = Camera.main.orthographicSize;
    }
	
	// Update is called once per frame
	void Update () {
        // Movement
        if (!move)
            return;

        float step = speed * Time.deltaTime;

        if (transform.position.y > Camera.main.transform.position.y + cameraOrthoSize)
            step *= outOfBoundsMultiplayer;
        transform.position = Vector3.MoveTowards(transform.position, transform.position + Vector3.down, step);

        if (transform.position.y > Camera.main.transform.position.y + cameraOrthoSize + outOfBoundsMaximum)
            transform.position = new Vector3(transform.position.x, Camera.main.transform.position.y + cameraOrthoSize + outOfBoundsMaximum, transform.position.z);
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
