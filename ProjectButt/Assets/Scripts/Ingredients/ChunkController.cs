using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkController : MonoBehaviour {

    [SerializeField]
    float minForce = 1f;
    [SerializeField]
    float maxForce = 3f;
    [SerializeField]
    float minRotation = 1f;
    [SerializeField]
    float maxRotation = 3f;
    [SerializeField]
    float lifeTime = 5f;

    // Use this for initialization
    void Start () {
        Vector2 randomVector = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        randomVector.Normalize();

        GetComponent<Rigidbody2D>().AddForce(randomVector * Random.Range(minForce, maxForce));
        GetComponent<Rigidbody2D>().AddTorque(Random.Range(minRotation, maxRotation));
        Invoke("DestroyChunk", lifeTime);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    void DestroyChunk()
    {
        Destroy(gameObject);
    }
}
