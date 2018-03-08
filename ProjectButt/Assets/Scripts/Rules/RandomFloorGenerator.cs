using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomFloorGenerator : MonoBehaviour {

    [SerializeField]
    GameObject[] blocks1;
    [SerializeField]
    float ratioBlock1 = 0;
    [SerializeField]
    GameObject[] blocks2;
    [SerializeField]
    float ratioBlock2 = 0;
    [SerializeField]
    GameObject[] blocks3;
    [SerializeField]
    float ratioBlock3 = 0;

    int currentIndexBlock1 = 0;
    int currentIndexBlock2 = 0;
    int currentIndexBlock3 = 0;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void GenerateOneFloor()
    {

    }
}
