using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeController : MonoBehaviour {

    [SerializeField]
    int damage = 1;
    [SerializeField]
    SpriteRenderer spikeSprite;
    [SerializeField]
    Vector3 positionPool;


    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerEnter2D(Collider2D other)
    {
        other.GetComponent<PlayerController>().DamagePlayer(damage);
    }

    void DestroySpike()
    {
        transform.position = positionPool;
    }

    public void resetSpike()
    {
        spikeSprite.color = new Color(1f, 1f, 1f, 1f);
    }

    public void ChangeBlockVisual(float alpha)
    {
        spikeSprite.color = new Color(1f, 1f, 1f, alpha);
    }
}
