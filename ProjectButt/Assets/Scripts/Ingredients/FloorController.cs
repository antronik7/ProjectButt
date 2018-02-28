using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (GameManager.instance.playerY < transform.position.y)
        {
            GameManager.instance.AddScore(1);
            this.enabled = false;
        }
            
	}
}
