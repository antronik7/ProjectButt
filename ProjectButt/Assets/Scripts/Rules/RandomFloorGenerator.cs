using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomFloorGenerator : MonoBehaviour {

    enum blockTypes
    {
        Block,
        Metal,
        Saw
    }

    class floorRules
    {
        public int minimumLevel;
        public int ratioBlock;
        public int ratioMetal;
        public int ratioSaw;
        public int maxNbrMetal;
        public int maxNbrSaw;
    }

    [SerializeField]
    floorRules[] floorsRules;
    [SerializeField]
    int numberBlocks = 11;
    [SerializeField]
    float firstFloorY = -5f;
    [SerializeField]
    float floorsDistance = 5f;
    [SerializeField]
    float firstBlockX = -5f;
    [SerializeField]
    float blockDistance = 1f;
    [SerializeField]
    BlockController[] blocks1;
    [SerializeField]
    BlockController[] blocks2;
    [SerializeField]
    BlockController[] blocks3;

    int currentIndexBlock1 = 0;
    int currentIndexBlock2 = 0;
    int currentIndexBlock3 = 0;

    float currentFloorY;
    int currentFloorRulesIndex = 0;

    blockTypes[] previousFloorBlocks;

    //Awake is always called before any Start functions
    void Awake()
    {
        for (int i = 0; i < numberBlocks; ++i)
        {
            previousFloorBlocks[i] = blockTypes.Block;
        }
    }

    // Use this for initialization
    void Start()
    {
        currentFloorY = firstFloorY;
    }

    // Update is called once per frame
    void Update () {
        if (GameManager.instance.playerY < (GameManager.instance.floor - 1) * (floorsDistance * -1))
        {
            int nextFloorRulesIndex = ++currentFloorRulesIndex;

            while (nextFloorRulesIndex < floorsRules.Length && GameManager.instance.floor >= floorsRules[nextFloorRulesIndex].minimumLevel)
            {
                currentFloorRulesIndex = nextFloorRulesIndex;
                ++nextFloorRulesIndex;
            }

            GameManager.instance.AddFloor();
        }

	}

    public void GenerateOneFloor()
    {
        int totalRatio = floorsRules[currentFloorRulesIndex].ratioBlock + floorsRules[currentFloorRulesIndex].ratioMetal + floorsRules[currentFloorRulesIndex].ratioSaw;

        if (totalRatio <= 0)
            return;

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
