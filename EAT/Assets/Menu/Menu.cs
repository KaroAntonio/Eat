using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

	void MenuStart(){
		Application.LoadLevel("test");
	}

	// menu quit button
	// ignored if in unity editor
	void MenuQuit(){
		Application.Quit();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
