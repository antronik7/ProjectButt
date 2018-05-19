using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSawController : MonoBehaviour {

    [SerializeField]
    bool backAndForth = true;
    [SerializeField]
    float speed = 10f;
    [SerializeField]
    Transform[] movingPositions;

    int currentIndexPositions = 0;
    bool indexUp = true;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, movingPositions[currentIndexPositions].position, step);

        if(transform.position == movingPositions[currentIndexPositions].position)
        {
            if(backAndForth)
            {
                if (indexUp)
                    ++currentIndexPositions;
                else
                    --currentIndexPositions;

                if (currentIndexPositions >= movingPositions.Length)
                {
                    currentIndexPositions -= 2;
                    indexUp = false;
                }
                else if (currentIndexPositions < 0)
                {
                    currentIndexPositions = 1;
                    indexUp = true;
                }
            }
            else
            {
                ++currentIndexPositions;

                if (currentIndexPositions >= movingPositions.Length)
                    currentIndexPositions = 0;
            }
        }
    }
}
