using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomFloorGenerator : MonoBehaviour {

    [SerializeField]
    int numberBlocks = 11;
    [SerializeField]
    float firstFloorY = 0;
    [SerializeField]
    float floorsDistance = 2.5f;
    [SerializeField]
    float firstBlockX = -2.5f;
    [SerializeField]
    float blockDistance = 0.5f;
    [SerializeField]
    BlockController[] blocks1;
    [SerializeField]
    int ratioBlock1 = 0;
    [SerializeField]
    BlockController[] blocks2;
    [SerializeField]
    int ratioBlock2 = 0;
    [SerializeField]
    BlockController[] blocks3;
    [SerializeField]
    int ratioBlock3 = 0;

    int currentIndexBlock1 = 0;
    int currentIndexBlock2 = 0;
    int currentIndexBlock3 = 0;
    float currentFloorY;

    void Awake () {
        
	}

    // Use this for initialization
    void Start()
    {
        currentFloorY = firstFloorY;
        GenerateOneFloor();
        GenerateOneFloor();
        GenerateOneFloor();
    }

    // Update is called once per frame
    void Update () {
		
	}

    // Should do an array of array...
    public void GenerateOneFloor()
    {
        int blocksLeft = numberBlocks;

        while(blocksLeft > 0)
        {
            int totalRatio = ratioBlock1;
            if (blocksLeft >= 2)
                totalRatio += ratioBlock2;
            if (blocksLeft >= 3)
                totalRatio += ratioBlock3;

            int randomRatio = Random.Range(1, totalRatio + 1);

            float blockX = firstBlockX + ((numberBlocks - blocksLeft) * blockDistance);

            if(randomRatio > 0 && randomRatio <= ratioBlock1)
            {
                blocks1[currentIndexBlock1].PlaceBlock(blockX, currentFloorY);
                currentIndexBlock1++;
                blocksLeft -= 1;
            }
            else if (randomRatio > ratioBlock1 && randomRatio <= ratioBlock1 + ratioBlock2)
            {
                blocks2[currentIndexBlock2].PlaceBlock(blockX + 0.25f, currentFloorY);
                currentIndexBlock2++;
                blocksLeft -= 2;
            }
            else if (randomRatio > ratioBlock1 + ratioBlock2 && randomRatio <= ratioBlock1 + ratioBlock2 + ratioBlock3)
            {
                blocks3[currentIndexBlock3].PlaceBlock(blockX + 0.5f, currentFloorY);
                currentIndexBlock3++;
                blocksLeft -= 3;
            }
            else
            {
                blocksLeft = 0;
            }
        }

        currentFloorY -= floorsDistance;
    }
}
