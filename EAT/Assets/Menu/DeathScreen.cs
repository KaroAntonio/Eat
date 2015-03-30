using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DeathScreen : MonoBehaviour {
	private bool deathScreenDisplayed; // flag prevents reduntant displaying
	private GameObject deathScreen;
	private GameObject gui;
	private Text survivalTime;


	void DeathScreenMainMenu(){
		Application.LoadLevel("Menu");
	}

	// Use this for initialization
	void Start () {
		deathScreenDisplayed = false;
		survivalTime = GameObject.Find("SurvivalTime").GetComponent<Text>();
		deathScreen = GameObject.Find("DeathScreen");
		deathScreen.SetActive(false);
		gui = GameObject.Find("GUI");
	}
	
	// Update is called once per frame
	void Update () {
		if(PlayerVars.hunger == 0 && !deathScreenDisplayed && PlayerVars.allowDeath){
			deathScreen.SetActive(true);
			gui.SetActive(false);
			deathScreenDisplayed = true;
			survivalTime.text = "Survival Time: " +
				((int)(Time.timeSinceLevelLoad / 60)).ToString("000") + ":" +
				(Time.timeSinceLevelLoad % 60).ToString("00");
			Time.timeScale = 0.0f;
		}
	}
}
