using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {
	private GameObject menuCanvas;

	void MenuMainMenu(){
		Application.LoadLevel("menu");
	}

	void MenuPause(){
		Debug.Log ("Pause Toggle");
		//IF PAUSED
		if(Time.timeScale == 0.0f){
			MouseLook.activated = true;
			menuCanvas.SetActive(false);
			Time.timeScale = 1.0f;
		
		} else{
			MouseLook.activated = false;
			menuCanvas.SetActive(true);
			Time.timeScale = 0.0f;
		}
	}

	// Use this for initialization
	void Start () {
		menuCanvas = GameObject.Find("Menu");
		menuCanvas.SetActive(false);
		Time.timeScale = 1.0f;
		MouseLook.activated = true;
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetButtonDown("Cancel")){
			MenuPause();
		}
	}
}
