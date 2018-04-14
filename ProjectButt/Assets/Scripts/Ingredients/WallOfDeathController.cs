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

        float maximumY = Camera.main.transform.position.y + 1.7025f + cameraOrthoSize + outOfBoundsMaximum;//VARIABLE VARIABLE VARIABLE

        if (transform.position.y > maximumY)//Broken
        {
            return;
        }
        else
        {
            float multiplier = 1;

            if (goingUp)
                goUp(multiplier);
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

    void goUp(float multiplier)
    {
        float step = wallRules[currentFloorRulesIndex].speedGoingUp * Time.deltaTime;

        if (doingAnimation)
            step = speedGoingUpAnim;

        step *= multiplier;
        transform.position = Vector3.MoveTowards(transform.position, moveTowardPosition, step);
        if (transform.position.y == moveTowardPosition.y)
        {
            IEnumerator coroutine;
            coroutine = goDown(multiplier);
            StartCoroutine(coroutine);
        }
    }

    IEnumerator goDown(float multiplier)
    {
        float delayGoingDown = wallRules[currentFloorRulesIndex].delayGoingDown / multiplier;
        float delayGoingUp = wallRules[currentFloorRulesIndex].delayGoingUp / multiplier;
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

        if(multiplier == 1)
            CameraShaker.instance.startCameraShake(durationScreenShake, 15f, 0.25f);

        yield return new WaitForSeconds(delayGoingUp);
        goingUp = true;
    }

    IEnumerator waitingOutbounds()
    {
        goingUp = false;
        StopCoroutine("goDown");
        yield return new WaitForSeconds(outOfBoundsDelay);
        float maximumY = Camera.main.transform.position.y + 1.7025f + cameraOrthoSize + outOfBoundsMaximum;//VARIABLE VARIABLE VARIABLE
        transform.position = new Vector3(transform.position.x, maximumY - 1, transform.position.z);
        moveTowardPosition = new Vector3(transform.position.x, transform.position.y + 1, transform.position.z);
        goingUp = true;
    }
}
