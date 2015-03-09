/*
	PlayerVars.cs

	Global variables for the Player

	This script contains the global variables for the player and an update function
	for variables that change on their own over time.
*/

using UnityEngine;
using System.Collections;

public class PlayerVars : MonoBehaviour {
	public const int MAX_ENERGY = 1000;
	public const float ENERGY_DEC_TIME = 0.1f; // time interval in seconds between energy decrements
	public const int ENERGY_DEC = 1; // amount by which energy is decremented
	private static float energyTime; // real time since startup at which "energy" was last decremented
	public static int energy; // referred to as "hunger" in group discussions
						// decrements over time
	
	// Use this for initialization
	void Start () {
		energyTime = Time.realtimeSinceStartup;
		energy = MAX_ENERGY;
	}

	// Update is called once per frame
	void Update () {
		float newEnergyTime = Time.realtimeSinceStartup;
		/*	if the real time since the last decrement is at or above
			the ENERGY_DEC_TIME interval, decrement energy by ENERGY_DEC*/
		if(newEnergyTime - energyTime >= ENERGY_DEC_TIME){
			energyTime = newEnergyTime;
			if(energy > 0){
				energy -= ENERGY_DEC;
			}
			Debug.Log(energy);
		}
	}
}
