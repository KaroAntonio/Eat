using UnityEngine;
using System.Collections;

public class DeathScreen : MonoBehaviour {
	private GameObject deathScreen;
	private GameObject gui;


	void DeathScreenMainMenu(){
		Application.LoadLevel("Menu");
	}

	// Use this for initialization
	void Start () {
		deathScreen = GameObject.Find("DeathScreen");
		deathScreen.SetActive(false);
		gui = GameObject.Find("GUI");
	}
	
	// Update is called once per frame
	void Update () {
		if(PlayerVars.hunger == 0 && PlayerVars.allowDeath == true){
			deathScreen.SetActive(true);
			gui.SetActive(false);
		}
	}
}
