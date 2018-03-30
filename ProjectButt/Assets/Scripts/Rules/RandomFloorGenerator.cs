using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomFloorGenerator : MonoBehaviour {

    [SerializeField]
    int numberBlocks = 11;
    [SerializeField]
    float firstFloorY = -2.5f;
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
    [SerializeField]
    SpikeController[] spikes;
    [SerializeField]
    int spikePercentage = 25;
    [SerializeField]
    int maximumSpikesPerFloor = 2;
    [SerializeField]
    FloorController[] floorControllers;

    int currentIndexBlock1 = 0;
    int currentIndexBlock2 = 0;
    int currentIndexBlock3 = 0;
    int currentIndexSpikes = 0;
    int currentIndexFloorController = 0;
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
        GenerateOneFloor();
    }

    // Update is called once per frame
    void Update () {
		
	}

    // Should do an array of array...
    public void GenerateOneFloor()
    {
        int blocksLeft = numberBlocks;
        int nbrSpikes = 0;

        floorControllers[currentIndexFloorController].ResetFloor();
        floorControllers[currentIndexFloorController].PlaceFloor(currentFloorY);

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
                blocks1[currentIndexBlock1].ResetBlock();
                blocks1[currentIndexBlock1].PlaceBlock(blockX, currentFloorY);

                if (nbrSpikes < maximumSpikesPerFloor && Random.Range(1, 101) <= spikePercentage)
                {
                    ++nbrSpikes;
                    spikes[currentIndexSpikes].ResetSpike();
                    spikes[currentIndexSpikes].PlaceSpike(blockX, currentFloorY);
                    blocks1[currentIndexBlock1].AddSpike(spikes[currentIndexSpikes]);

                    ++currentIndexSpikes;
                    if (currentIndexSpikes >= spikes.Length)
                        currentIndexSpikes = 0;
                }

                currentIndexBlock1++;
                blocksLeft -= 1;
            }
            else if (randomRatio > ratioBlock1 && randomRatio <= ratioBlock1 + ratioBlock2)
            {
                blocks2[currentIndexBlock2].ResetBlock();
                blocks2[currentIndexBlock2].PlaceBlock(blockX + 0.5f, currentFloorY);

                for (int i = 0; i < 2; i++)
                {
                    if (nbrSpikes < maximumSpikesPerFloor && Random.Range(1, 101) <= spikePercentage)
                    {
                        ++nbrSpikes;
                        spikes[currentIndexSpikes].ResetSpike();
                        spikes[currentIndexSpikes].PlaceSpike(blockX + (i * blockDistance), currentFloorY);
                        blocks2[currentIndexBlock2].AddSpike(spikes[currentIndexSpikes]);

                        ++currentIndexSpikes;
                        if (currentIndexSpikes >= spikes.Length)
                            currentIndexSpikes = 0;
                    }
                }

                currentIndexBlock2++;
                blocksLeft -= 2;
            }
            else if (randomRatio > ratioBlock1 + ratioBlock2 && randomRatio <= ratioBlock1 + ratioBlock2 + ratioBlock3)
            {
                blocks3[currentIndexBlock3].ResetBlock();
                blocks3[currentIndexBlock3].PlaceBlock(blockX + 1f, currentFloorY);

                for (int i = 0; i < 3; i++)
                {
                    if (nbrSpikes < maximumSpikesPerFloor && Random.Range(1, 101) <= spikePercentage)
                    {
                        ++nbrSpikes;
                        spikes[currentIndexSpikes].ResetSpike();
                        spikes[currentIndexSpikes].PlaceSpike(blockX + (i * blockDistance), currentFloorY);
                        blocks3[currentIndexBlock3].AddSpike(spikes[currentIndexSpikes]);

                        ++currentIndexSpikes;
                        if (currentIndexSpikes >= spikes.Length)
                            currentIndexSpikes = 0;
                    }
                }

                currentIndexBlock3++;
                blocksLeft -= 3;
            }
            else
            {
                blocksLeft = 0;
            }

            if (currentIndexBlock1 >= blocks1.Length)
                currentIndexBlock1 = 0;

            if (currentIndexBlock2 >= blocks2.Length)
                currentIndexBlock2 = 0;

            if (currentIndexBlock3 >= blocks3.Length)
                currentIndexBlock3 = 0;
        }

        ++currentIndexFloorController;
        currentFloorY -= floorsDistance;

        if (currentIndexFloorController >= floorControllers.Length)
            currentIndexFloorController = 0;
    }
}
