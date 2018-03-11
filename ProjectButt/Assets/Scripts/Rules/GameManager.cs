using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager instance = null;

    [HideInInspector]
    public int score = 0;

    [HideInInspector]
    public float playerY = 0;

    [SerializeField]
    Transform player;

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

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);

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
}
