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
    UIController.Transition transitionStartLevel;
  
    [HideInInspector]
    public int score = 0;
    [HideInInspector]
    public int floor = 1;
    [HideInInspector]
    public float playerY = 0;

    PlayerController playerController;
    BackgroundScroller backgroundScroller;

    bool stopGeneratingFloor = false;

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

        Application.targetFrameRate = 60;
        playerController = player.GetComponent<PlayerController>();
        backgroundScroller = GetComponent<BackgroundScroller>();
        playerY = player.position.y;
    }

    // Use this for initialization
    void Start () {
        StartCoroutine("RoundStartAnimation");
    }
	
	// Update is called once per frame
	void Update () {
        playerY = player.position.y;
    }

    IEnumerator RoundStartAnimation()
    {
        UIController.instance.StartTransition(transitionStartLevel);
        playerController.DisablePlayerRunning();
        playerController.DisablePlayerJumping();
        yield return new WaitForSeconds(0.5f);
        wallOfDeath.StartAnimation();
        yield return new WaitForSeconds(4.0f);
        wallOfDeath.StartWall();
        playerController.EnablePlayerRunning();
        playerController.EnablePlayerJumping();

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

        if (stopGeneratingFloor)
            return;

        floorGenerator.GenerateOneFloor();
    }

    public void PlayerGotKill()
    {
        wallOfDeath.StopWall();
        myCamera.SetFollowPlayer(false);
        backgroundScroller.DisableScrolling();
        stopGeneratingFloor = true;
        StartCoroutine("RestartLevel");
    }

    public void Sleep(float duration)
    {
        IEnumerator coroutine = Sleeping(duration);
        StartCoroutine(coroutine);
    }

    IEnumerator RestartLevel()
    {
        yield return new WaitForSeconds(3f);
        UIController.instance.StartTransition(UIController.Transition.LeftToRight);
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene("main");
    }

    IEnumerator Sleeping(float duration)
    {
        Time.timeScale = 0.0f;

        float sleepEndTime = Time.realtimeSinceStartup + duration;
        while (Time.realtimeSinceStartup < sleepEndTime)
        {
            yield return 0;
        }

        Time.timeScale = 1;
    }
}
