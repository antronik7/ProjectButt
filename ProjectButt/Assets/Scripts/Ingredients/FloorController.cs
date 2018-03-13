using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorController : MonoBehaviour {

    [SerializeField]
    Vector3 positionPool;

    bool watchPlayer = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (!watchPlayer)
            return;

        if (GameManager.instance.playerY < transform.position.y)
        {
            GameManager.instance.AddFloor();
            watchPlayer = false;
        }
	}

    public void ResetFloor()
    {
        watchPlayer = true;
    }

    public void DestroyFloorController()
    {
        watchPlayer = false;
        transform.position = positionPool;
    }

    public void PlaceFloor(float y)
    {
        transform.position = new Vector3(0, y, transform.position.z);
    }
}
