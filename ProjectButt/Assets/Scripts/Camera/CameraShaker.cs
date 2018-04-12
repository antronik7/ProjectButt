using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShaker : MonoBehaviour {

    public static CameraShaker instance = null;              //Static instance of GameManager which allows it to be accessed by any other script.

    float previousMagnitude = 0;
    Transform cameraTransform;
    Vector3 originalPosition = Vector3.zero;

    //Awake is always called before any Start functions
    private void Awake()
    {
        //Check if instance already exists
        if (instance == null)
            //if not, set instance to this
            instance = this;
        //If instance already exists and it's not this:
        else if (instance != this)
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        cameraTransform = Camera.main.transform;
        originalPosition = cameraTransform.localPosition;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void startCameraShake(float duration, float speed, float magnitude)
    {
        if (magnitude >= previousMagnitude)
            StopCoroutine("ShakeCamera");

        StartCoroutine(ShakeCamera(duration, speed, magnitude));
        previousMagnitude = magnitude;
    }

    IEnumerator ShakeCamera(float duration, float speed, float magnitude)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float x = (Mathf.PerlinNoise(Time.time * speed, 0f) * magnitude) - (magnitude / 2f);
            float y = (Mathf.PerlinNoise(0f, Time.time * speed) * magnitude) - (magnitude / 2f);
            cameraTransform.localPosition = new Vector3(originalPosition.x + x, originalPosition.y + y, originalPosition.z);
            yield return null;
        }
        cameraTransform.localPosition = originalPosition;
    }
}
