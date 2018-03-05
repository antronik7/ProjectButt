using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour {

    public enum Transition
    {
        LeftToRight,
        RightToLeft,
        TopToBottom,
        BottomToTop
    }

    public static UIController instance = null;

    [SerializeField]
    Text scoreText;
    [SerializeField]
    Animator transitionAnimator;

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
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void setScoreText(int value)
    {
        scoreText.text = value.ToString();
    }

    public void StartTransition(Transition transitionType)
    {
        switch (transitionType)
        {
            case Transition.LeftToRight:
                transitionAnimator.SetTrigger("LeftToRight");
                break;
            case Transition.RightToLeft:
                transitionAnimator.SetTrigger("RightToLeft");
                break;
            case Transition.TopToBottom:
                transitionAnimator.SetTrigger("TopToBottom");
                break;
            case Transition.BottomToTop:
                transitionAnimator.SetTrigger("BottomToTop");
                break;
            default:
                break;
        }
    }
}
