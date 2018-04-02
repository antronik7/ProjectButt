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
        previousFloorBlocks = new blockTypes[numberBlocks];
        for (int i = 0; i < numberBlocks; ++i)
        {
            previousFloorBlocks[i] = blockTypes.Block;// Maybe a list
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
        int ratioBlock = floorsRules[currentFloorRulesIndex].ratioBlock;
        int ratioMetal = floorsRules[currentFloorRulesIndex].ratioBlock + floorsRules[currentFloorRulesIndex].ratioMetal;
        int ratioSaw = ratioMetal + floorsRules[currentFloorRulesIndex].ratioSaw;
        int totalRatio = ratioSaw;

        if (totalRatio <= 0)
            return;

        //Create random floor
        blockTypes[] floorBlocks = new blockTypes[numberBlocks];

        for (int i = 0; i < numberBlocks; i++)
        {
            int randomBlockType = Random.Range(1, totalRatio + 1);

            if (randomBlockType > ratioBlock && randomBlockType <= floorsRules[currentFloorRulesIndex].ratioMetal)
                floorBlocks[i] = blockTypes.Metal;
            else if (randomBlockType > floorsRules[currentFloorRulesIndex].ratioMetal && randomBlockType <= floorsRules[currentFloorRulesIndex].ratioSaw) //Add rules for maximum and separating saw
                floorBlocks[i] = blockTypes.Saw;
            else
                floorBlocks[i] = blockTypes.Block;
        }

        //Check if there is at least a passage from the previous floor
        int nbrPassages = 0;
        int[] possiblePassages = new int[numberBlocks];

        for (int i = 0; i < numberBlocks && nbrPassages <= 0; i++)
        {
            if(previousFloorBlocks[i] == blockTypes.Block && floorBlocks[i] != blockTypes.Saw)
            {
                possiblePassages[nbrPassages] = i;
                ++nbrPassages;
            }
        }

        // Replace one dangerous block with a safe block
        if(nbrPassages <= 0)
        {
            int randomBlock = Random.Range(0, nbrPassages);

            int safeRatio = ratioMetal;
            int randomBlockType = Random.Range(1, safeRatio + 1);

            if (randomBlockType > ratioBlock && randomBlockType <= ratioMetal)
                floorBlocks[randomBlock] = blockTypes.Metal;
            else
                floorBlocks[randomBlock] = blockTypes.Block;
        }

        //Check if there is the minimum amount of normal blocks
        int nbrDestructableBlock = 0;
        for (int i = 0; i < numberBlocks && nbrDestructableBlock <= 0; ++i)
        {
            if (floorBlocks[i] == blockTypes.Block)
                ++nbrDestructableBlock;
        }

        if(nbrDestructableBlock <= 0)
        {
            int randomBlock = Random.Range(0, numberBlocks);
            floorBlocks[randomBlock] = blockTypes.Block;
        }

        //Saved this floor
        System.Array.Copy(floorBlocks, previousFloorBlocks, numberBlocks);

        //Place the block from the pool
        blockTypes blockType;
        int nbrSameBlock = 0;
        bool checkForSameBlock = true;

        for (int i = 0; i < numberBlocks; i++)
        {
            nbrSameBlock = 1;
            checkForSameBlock = true;
            blockType = floorBlocks[i];

            if(blockType == blockTypes.Metal)
            {
                while(checkForSameBlock && nbrSameBlock < 3 && i + nbrSameBlock < numberBlocks)
                {

                }
            }
            else if (blockType == blockTypes.Saw)
            {

            }
            else
            {

            }
        }


        while (blocksLeft > 0)
        {
            int randomRatio = Random.Range(1, totalRatio + 1);

            float blockX = firstBlockX + ((numberBlocks - blocksLeft) * blockDistance);

            if(randomRatio > 0 && randomRatio <= ratioBlock1)
            {
                blocks1[currentIndexBlock1].ResetBlock();
                blocks1[currentIndexBlock1].PlaceBlock(blockX, currentFloorY);

                currentIndexBlock1++;
                blocksLeft -= 1;
            }
            else if (randomRatio > ratioBlock1 && randomRatio <= ratioBlock1 + ratioBlock2)
            {
                blocks2[currentIndexBlock2].ResetBlock();
                blocks2[currentIndexBlock2].PlaceBlock(blockX + 0.5f, currentFloorY);

                currentIndexBlock2++;
                blocksLeft -= 2;
            }
            else if (randomRatio > ratioBlock1 + ratioBlock2 && randomRatio <= ratioBlock1 + ratioBlock2 + ratioBlock3)
            {
                blocks3[currentIndexBlock3].ResetBlock();
                blocks3[currentIndexBlock3].PlaceBlock(blockX + 1f, currentFloorY);

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

        currentFloorY -= floorsDistance;
    }
}
