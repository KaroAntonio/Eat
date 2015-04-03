using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	void MainMenuStart(){
		Application.LoadLevel("nomsTown");
	}

	void MainMenuQuit(){
		Application.Quit();
	}

	// Use this for initialization
	void Start () {
		Time.timeScale = 1.0f;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
