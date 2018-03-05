using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartScreenController : MonoBehaviour {

    [SerializeField]
    GameObject player;
    [SerializeField]
    float activateJumpDelay = 1;
    [SerializeField]
    float changeSceneY = 0;
    [SerializeField]
    string nextScene;
    [SerializeField]
    float nextSceneDelay = 1;
    [SerializeField]
    UIController.Transition transitionType;

    Transform playerTransform;
    PlayerController playerScript;

    //Awake is always called before any Start functions
    void Awake()
    {
        playerTransform = player.GetComponent<Transform>();
        playerScript = player.GetComponent<PlayerController>();
    }

    // Use this for initialization
    void Start () {
        Invoke("EnablePlayerJump", nextSceneDelay);
    }
	
	// Update is called once per frame
	void Update () {
        if (playerTransform.position.y < activateJumpDelay)
        {
            Invoke("ChangeScene", nextSceneDelay);
            UIController.instance.StartTransition(transitionType);
        }
	}

    void EnablePlayerJump()
    {
        playerScript.enabled = true;
    }

    void ChangeScene()
    {
        SceneManager.LoadScene(nextScene);
    }
}
