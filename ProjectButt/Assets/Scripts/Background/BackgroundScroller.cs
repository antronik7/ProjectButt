using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour {

    [SerializeField]
    float playerOffset = 12.8f;
    [SerializeField]
    Transform player;
    [SerializeField]
    Transform[] arrayBackgrounds;

    int index = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (player.position.y < arrayBackgrounds[index].position.y - playerOffset)
        {
            arrayBackgrounds[index].position = new Vector3(0f, arrayBackgrounds[index].position.y - (playerOffset * 3), 0f);
            ++index;

            if (index > arrayBackgrounds.Length - 1)
                index = 0;
        }
	}
}
