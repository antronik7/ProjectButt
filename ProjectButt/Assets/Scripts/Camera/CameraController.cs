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
    [SerializeField]
    bool followIfPlayerHigher = false;

    bool followPlayer = true;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void FixedUpdate()
    {
        if (!followPlayer)
            return;

        if(!followIfPlayerHigher)
        {
            if (GameManager.instance.playerY + cameraOffset >= transform.position.y)
                return;
        }

        Vector3 velocity = Vector3.zero;
        Vector3 positionCamera = new Vector3(0f, targetToFollow.position.y + cameraOffset, -10f);
        positionCamera = Vector3.SmoothDamp(transform.position, positionCamera, ref velocity, cameraFollowDelay);
        
        transform.position = positionCamera;
    }

    public void SetFollowPlayer(bool value)
    {
        followPlayer = value;
    }
}
