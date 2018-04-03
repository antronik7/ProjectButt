using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawController : MonoBehaviour {

    [SerializeField]
    int damage = 1;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnCollisionEnter2D(Collision2D coll)
    {
        coll.gameObject.GetComponent<PlayerController>().DamagePlayer(damage);
    }
}
