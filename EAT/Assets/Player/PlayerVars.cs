/*
	PlayerVars.cs

	Global variables for the Player

	This script contains the global variables for the player and an update function
	for variables that change on their own over time.
*/

using UnityEngine;
using System.Collections;

public class PlayerVars : MonoBehaviour {
	public const float MAX_HUNGER = 1000;
	public const float HUNGER_DEC_TIME = 0.1f; // time interval in seconds between energy decrements
	public const int HUNGER_DEC = 1; // amount by which energy is decremented
	private static float hungerTime; // real time since startup at which "energy" was last decremented
	public static float hunger; // referred to as "hunger" in group discussions
						// decrements over time
	
	// Use this for initialization
	void Start () {
		hungerTime = Time.time;
		hunger = MAX_HUNGER * 0.5f;
	}

	// Update is called once per frame
	void Update () {
		float newHungerTime = Time.time;
		/*	if the real time since the last decrement is at or above
			the ENERGY_DEC_TIME interval, decrement energy by ENERGY_DEC*/
		if(newHungerTime - hungerTime >= HUNGER_DEC_TIME){
			hungerTime = newHungerTime;
			if(hunger > 0){
				hunger -= HUNGER_DEC;
			} else{
			// death
			
			}
//			Debug.Log(hunger);
		}
	}
}
