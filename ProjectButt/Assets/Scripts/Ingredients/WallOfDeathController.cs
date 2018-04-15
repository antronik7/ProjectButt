using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallOfDeathController : MonoBehaviour {

    [System.Serializable]
    class WallRules
    {
        public int minimumFloor = 1;
        public float speedGoingUp = 1;
        public float delayGoingDown = 1;
        public float delayGoingUp = 1;
    }

    [SerializeField]
    WallRules[] wallRules;

    [SerializeField]
    int nbrStepAnim = 4;
    [SerializeField]
    float speedGoingUpAnim = 1;
    [SerializeField]
    float delayGoingDownAnim = 1;
    [SerializeField]
    float delayGoingUpAnim = 1;
    [SerializeField]
    float durationScreenShake = 0.5f;

    [SerializeField]
    float outOfBoundsDelay = 5f;
    [SerializeField]
    float outOfBoundsMaximum = 3;

    int currentFloorRulesIndex = 0;
    Vector3 moveTowardPosition;
    float currentSpeed;
    float cameraOrthoSize;
    bool doingAnimation = false;
    int animationStep = 0;
    bool move = false;
    bool goingUp = true;

	// Use this for initialization
	void Start () {
        cameraOrthoSize = Camera.main.orthographicSize;
        moveTowardPosition = new Vector3(0, transform.position.y + 1, transform.position.z);
    }
	
	// Update is called once per frame
	void Update () {

        int nextWallRulesIndex = currentFloorRulesIndex + 1;

        while (nextWallRulesIndex < wallRules.Length && GameManager.instance.floor >= wallRules[nextWallRulesIndex].minimumFloor)
        {
            currentFloorRulesIndex = nextWallRulesIndex;
            ++nextWallRulesIndex;
        }

        // Movement
        if (!move)
            return;

        float maximumY = Camera.main.transform.position.y + cameraOrthoSize + outOfBoundsMaximum + 0.51f;//VARIABLE VARIABLE VARIABLE

        if (!doingAnimation && transform.position.y > maximumY)//Broken
        {
            StopAllCoroutines();
            StartCoroutine("waitingOutbounds");
            return;
        }
        else
        {
            if (goingUp)
                goUp();
        }
    }

    public void StartAnimation()
    {
        transform.position = new Vector3(0, transform.position.y, transform.position.z);
        move = true;
        doingAnimation = true;
    }

    public void StartWall()
    {
        move = true;
    }

    public void StopWall()
    {
        move = false;
    }

    void goUp()
    {
        float step = wallRules[currentFloorRulesIndex].speedGoingUp * Time.deltaTime;

        if (doingAnimation)
            step = speedGoingUpAnim;

        transform.position = Vector3.MoveTowards(transform.position, moveTowardPosition, step);
        if (transform.position.y == moveTowardPosition.y)
        {
            StartCoroutine("goDown");
        }
    }

    IEnumerator goDown()
    {
        float delayGoingDown = wallRules[currentFloorRulesIndex].delayGoingDown;
        float delayGoingUp = wallRules[currentFloorRulesIndex].delayGoingUp;
        if(doingAnimation)
        {
            ++animationStep;
            if (animationStep >= nbrStepAnim)
            {
                doingAnimation = false;
                move = false;
                goingUp = true;
            }

            delayGoingDown = delayGoingDownAnim;
            delayGoingUp = delayGoingUpAnim;
        }

        goingUp = false;
        yield return new WaitForSeconds(delayGoingDown);
        transform.position = new Vector3(transform.position.x, moveTowardPosition.y - 2, transform.position.z);
        moveTowardPosition = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);

        CameraShaker.instance.startCameraShake(durationScreenShake, 15f, 0.25f);

        yield return new WaitForSeconds(delayGoingUp);
        goingUp = true;
    }

    IEnumerator waitingOutbounds()
    {
        goingUp = false;
        float maximumY = Camera.main.transform.position.y + cameraOrthoSize + outOfBoundsMaximum + 0.5f;//VARIABLE VARIABLE VARIABLE
        transform.position = new Vector3(transform.position.x, maximumY, transform.position.z);
        moveTowardPosition = transform.position;
        yield return new WaitForSeconds(outOfBoundsDelay);
        StartCoroutine("goDown");
    }
}
