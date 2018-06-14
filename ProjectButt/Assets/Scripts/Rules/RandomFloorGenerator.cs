using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomFloorGenerator : MonoBehaviour {

    enum BlockTypes
    {
        Block,
        Metal,
        Saw
    }

    [System.Serializable]
    class ObstaclesPoolElements
    {
        public GameObject obstacle;
        public int probability;
    }

    [System.Serializable]
    class FloorRules
    {
        public int minimumFloor = 1;
        public int ratioBlock = 1;
        public int ratioMetal = 0;
        public int ratioSaw = 0;
        public int maxNbrMetal = 0;
        public int maxNbrSaw = 1;
        public int maxLenghtSaw = 2;
        public int minDistanceBetweenSaw = 1;
        public int obstacleMaxProbability = 100;
        public ObstaclesPoolElements[] obstaclesPoolElements;
    }

    [SerializeField]
    FloorRules[] floorsRules;
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
    [SerializeField]
    BlockController[] metals1;
    [SerializeField]
    BlockController[] metals2;
    [SerializeField]
    BlockController[] metals3;
    [SerializeField]
    BlockController[] saws1;
    [SerializeField]
    BlockController[] saws2;
    [SerializeField]
    BlockController[] saws3;

    int currentIndexBlock1 = 0;
    int currentIndexBlock2 = 0;
    int currentIndexBlock3 = 0;
    int currentIndexMetal1 = 0;
    int currentIndexMetal2 = 0;
    int currentIndexMetal3 = 0;
    int currentIndexSaw1 = 0;
    int currentIndexSaw2 = 0;
    int currentIndexSaw3 = 0;

    float currentFloorY;
    int currentFloorRulesIndex = 0;

    BlockTypes[] previousFloorBlocks;

    //Awake is always called before any Start functions
    void Awake()
    {
        currentFloorY = firstFloorY;

        previousFloorBlocks = new BlockTypes[numberBlocks];
        for (int i = 0; i < numberBlocks; ++i)
        {
            previousFloorBlocks[i] = BlockTypes.Block;// Maybe a list
        }
    }

    // Use this for initialization
    void Start()
    {
        GenerateOneFloor();
        GenerateOneFloor();
        GenerateOneFloor();
    }

    // Update is called once per frame
    void Update () {
        if (GameManager.instance.playerY < (GameManager.instance.floor - 1) * (floorsDistance * -1))// Function in gamemanager
        {
            int nextFloorRulesIndex = currentFloorRulesIndex + 1;

            while (nextFloorRulesIndex < floorsRules.Length && GameManager.instance.floor + 1 >= floorsRules[nextFloorRulesIndex].minimumFloor)
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
        int ratioMetal = floorsRules[currentFloorRulesIndex].ratioMetal;
        int ratioSaw = floorsRules[currentFloorRulesIndex].ratioSaw;
        int totalRatio = ratioBlock + ratioMetal + ratioSaw;

        int currentRatioSaw = ratioSaw;

        if (totalRatio <= 0)
            return;

        //Create random floor
        BlockTypes[] floorBlocks = new BlockTypes[numberBlocks];
        bool canPlaceSaw = true;
        int currentLenghtSaw = 0;
        int currentNbrSaw = 0;
        int currentDistancePreviousSaw = floorsRules[currentFloorRulesIndex].minDistanceBetweenSaw;

        BlockTypes previousBlock = BlockTypes.Block;
        for (int i = 0; i < numberBlocks; i++)
        {
            bool placingSaw = false;// sortir declaration de la boucle
            totalRatio = ratioBlock + ratioMetal + currentRatioSaw;

            int randomBlockType = Random.Range(1, totalRatio + 1);
            BlockTypes blockToPlace;

            if (randomBlockType > ratioBlock && randomBlockType <= ratioMetal + ratioBlock)
                blockToPlace = BlockTypes.Metal;
            else if (randomBlockType > ratioMetal + ratioBlock && randomBlockType <= currentRatioSaw + ratioBlock + ratioMetal)
            {
                blockToPlace = BlockTypes.Saw;
                placingSaw = true;
                Debug.Log("wtf");
            }
            else
            {
                blockToPlace = BlockTypes.Block;
            }

            floorBlocks[i] = blockToPlace;

            if (placingSaw)
            {
                ++currentLenghtSaw;
                currentDistancePreviousSaw = 0;

                if(previousBlock == BlockTypes.Saw)
                {
                    if (currentLenghtSaw >= floorsRules[currentFloorRulesIndex].maxLenghtSaw)
                        currentRatioSaw = 0;
                }
                else
                {
                    ++currentNbrSaw;
                }
            }
            else
            {
                if (canPlaceSaw)
                {
                    if (currentNbrSaw >= floorsRules[currentFloorRulesIndex].maxNbrSaw)
                    {
                        canPlaceSaw = false;
                        currentRatioSaw = 0;
                    }
                    else
                    {
                        ++currentDistancePreviousSaw;

                        if (currentDistancePreviousSaw >= floorsRules[currentFloorRulesIndex].minDistanceBetweenSaw)
                            currentRatioSaw = ratioSaw;
                        else
                            currentRatioSaw = 0;
                    }
                }
            }

            previousBlock = blockToPlace;
        }

        //Check if there is at least a passage from the previous floor
        int nbrPassages = 0;
        int nbrPossiblePassages = 0;
        int[] possiblePassages = new int[numberBlocks];

        for (int i = 0; i < numberBlocks && nbrPassages <= 0; i++)
        {
            if(previousFloorBlocks[i] == BlockTypes.Block)
            {
                possiblePassages[nbrPossiblePassages] = i;
                ++nbrPossiblePassages;

                if(floorBlocks[i] != BlockTypes.Saw)
                    ++nbrPassages;
            }
        }

        // Replace one dangerous block with a safe block
        if(nbrPassages <= 0)
        {
            int randomBlock = Random.Range(0, nbrPossiblePassages);

            int safeRatio = ratioMetal;
            int randomBlockType = Random.Range(1, safeRatio + 1);

            if (randomBlockType > ratioBlock && randomBlockType <= ratioMetal)
                floorBlocks[possiblePassages[randomBlock]] = BlockTypes.Metal;
            else
                floorBlocks[possiblePassages[randomBlock]] = BlockTypes.Block;
        }

        //Check if there is the minimum amount of normal blocks
        int nbrDestructableBlock = 0;
        for (int i = 0; i < numberBlocks && nbrDestructableBlock <= 0; ++i)
        {
            if (floorBlocks[i] == BlockTypes.Block)
                ++nbrDestructableBlock;
        }

        if(nbrDestructableBlock <= 0)
        {
            int randomBlock = Random.Range(0, numberBlocks);
            floorBlocks[randomBlock] = BlockTypes.Block;
        }

        //Saved this floor
        System.Array.Copy(floorBlocks, previousFloorBlocks, numberBlocks);

        //Place the block from the pool
        BlockTypes blockType;
        int nbrSameBlock = 0;
        float blockX = 0;
        bool checkForSameBlock = true;

        for (int i = 0; i < numberBlocks; i++)
        {
            nbrSameBlock = 1;
            checkForSameBlock = true;
            blockType = floorBlocks[i];

            while (checkForSameBlock && nbrSameBlock < 3 && i + nbrSameBlock < numberBlocks)//Variable for different maximum size
            {
                if (floorBlocks[i + nbrSameBlock] == blockType)
                    ++nbrSameBlock;
                else
                    checkForSameBlock = false;
            }

            blockX = firstBlockX + ((i * blockDistance) + ((blockDistance/2) * (nbrSameBlock - 1)));

            if (blockType == BlockTypes.Metal)
            {
                if (nbrSameBlock == 2)
                {
                    metals2[currentIndexMetal2].ResetBlock();
                    metals2[currentIndexMetal2].PlaceBlock(blockX, currentFloorY);

                    currentIndexMetal2++;
                }
                else if (nbrSameBlock == 3)
                {
                    metals3[currentIndexMetal3].ResetBlock();
                    metals3[currentIndexMetal3].PlaceBlock(blockX, currentFloorY);

                    currentIndexMetal3++;
                }
                else
                {
                    metals1[currentIndexMetal1].ResetBlock();
                    metals1[currentIndexMetal1].PlaceBlock(blockX, currentFloorY);

                    currentIndexMetal1++;
                }
            }
            else if (blockType == BlockTypes.Saw)
            {
                if (nbrSameBlock == 2)
                {
                    saws2[currentIndexSaw2].ResetBlock();
                    saws2[currentIndexSaw2].PlaceBlock(blockX, currentFloorY);

                    currentIndexSaw2++;
                }
                else if (nbrSameBlock == 3)
                {
                    saws3[currentIndexSaw3].ResetBlock();
                    saws3[currentIndexSaw3].PlaceBlock(blockX, currentFloorY);

                    currentIndexSaw3++;
                }
                else
                {
                    saws1[currentIndexSaw1].ResetBlock();
                    saws1[currentIndexSaw1].PlaceBlock(blockX, currentFloorY);

                    currentIndexSaw1++;
                }
            }
            else
            {
                if (nbrSameBlock == 2)
                {
                    blocks2[currentIndexBlock2].ResetBlock();
                    blocks2[currentIndexBlock2].PlaceBlock(blockX, currentFloorY);

                    currentIndexBlock2++;
                }
                else if (nbrSameBlock == 3)
                {
                    blocks3[currentIndexBlock3].ResetBlock();
                    blocks3[currentIndexBlock3].PlaceBlock(blockX, currentFloorY);

                    currentIndexBlock3++;
                }
                else
                {
                    blocks1[currentIndexBlock1].ResetBlock();
                    blocks1[currentIndexBlock1].PlaceBlock(blockX, currentFloorY);

                    currentIndexBlock1++;
                }
            }

            i += nbrSameBlock - 1;

            if (currentIndexBlock1 >= blocks1.Length)
                currentIndexBlock1 = 0;

            if (currentIndexBlock2 >= blocks2.Length)
                currentIndexBlock2 = 0;

            if (currentIndexBlock3 >= blocks3.Length)
                currentIndexBlock3 = 0;

            if (currentIndexMetal1 >= metals1.Length)
                currentIndexMetal1 = 0;

            if (currentIndexMetal2 >= metals2.Length)
                currentIndexMetal2 = 0;

            if (currentIndexMetal3 >= metals3.Length)
                currentIndexMetal3 = 0;

            if (currentIndexSaw1 >= saws1.Length)
                currentIndexSaw1 = 0;

            if (currentIndexSaw2 >= saws2.Length)
                currentIndexSaw2 = 0;

            if (currentIndexSaw3 >= saws3.Length)
                currentIndexSaw3 = 0;
        }

        GenerateObstacle();

        GameManager.instance.AddFloorY(currentFloorY);
        currentFloorY -= floorsDistance;
    }

    void GenerateObstacle()
    {
        FloorRules floorRules = floorsRules[currentFloorRulesIndex];

        int ramdomNumber = Random.Range(1, floorRules.obstacleMaxProbability);

        int currentProbability = 0;
        int indexChosenObstacle = -1;

        for (int i = 0; i < floorRules.obstaclesPoolElements.Length; ++i)
        {
            int obstacleProbability = floorRules.obstaclesPoolElements[i].probability;

            if (obstacleProbability <= 0)
                continue;

            currentProbability += obstacleProbability;

            if (ramdomNumber <= currentProbability)
            {
                indexChosenObstacle = i;
                break;
            }
        }

        // We did not roll a number to place an obstacle or there was a problem, so we can stop using this function...
        if (indexChosenObstacle <= 0)
            return;

        Vector4 obstaclePosition = floorRules.obstaclesPoolElements[indexChosenObstacle].obstacle.transform.position;// function function function
        floorRules.obstaclesPoolElements[indexChosenObstacle].obstacle.transform.position = new Vector3(obstaclePosition.x, currentFloorY, obstaclePosition.z);
        floorRules.obstaclesPoolElements[indexChosenObstacle].obstacle.GetComponent<ObstacleInitializer>().Initialize();
    }
}
