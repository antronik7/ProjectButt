using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    [SerializeField]
    Transform targetToFollow;
    [SerializeField]
    float cameraOffset = -1f;
    [SerializeField]
    float cameraFollowDelay = 0.3f;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void FixedUpdate()
    {
        Vector3 velocity = Vector3.zero;
        Vector3 positionCamera = new Vector3(0f, targetToFollow.position.y + cameraOffset, -10f);
        positionCamera = Vector3.SmoothDamp(transform.position, positionCamera, ref velocity, cameraFollowDelay);
        
        transform.position = positionCamera;
    }
}
