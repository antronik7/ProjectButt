using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;

    [SerializeField]
    Transform player;
    [SerializeField]
    WallOfDeathController wallOfDeath;
    [SerializeField]
    CameraController myCamera;
    [SerializeField]
    RandomFloorGenerator floorGenerator;
    [SerializeField]
    float wallOfDeathSpeedModificator = 0.1f;

    [HideInInspector]
    public int score = 0;
    [HideInInspector]
    public int floor = 1;
    [HideInInspector]
    public float playerY = 0;

    //Awake is always called before any Start functions
    void Awake()
    {
        //Check if instance already exists
        if (instance == null)
            //if not, set instance to this
            instance = this;
        //If instance already exists and it's not this:
        else if (instance != this)
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        playerY = player.position.y;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        playerY = player.position.y;
    }

    public void AddScore(int scoerToAdd)
    {
        if (scoerToAdd > 0)
        {
            score += scoerToAdd;
            UIController.instance.setScoreText(score);
        }
    }

    public void AddFloor()
    {
        ++floor;
        if (floor % 10 == 0)
            wallOfDeath.ModifySpeed(wallOfDeathSpeedModificator);

        floorGenerator.GenerateOneFloor();
    }

    public void PlayerGotKill()
    {
        wallOfDeath.SetWallCanMove(false);
        myCamera.SetFollowPlayer(false);
    }
}
