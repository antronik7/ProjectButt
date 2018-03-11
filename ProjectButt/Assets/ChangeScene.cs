using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour {

    public string nomScene;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void changerDeScene()
    {
        SceneManager.LoadScene(nomScene);
    }
}
