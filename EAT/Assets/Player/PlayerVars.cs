/*
	PlayerVars.cs

	Global variables for the Player

	This script contains the global variables for the player and an update function
	for variables that change on their own over time.
*/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Entities;
using RAIN.Entities.Aspects;

public class PlayerVars : MonoBehaviour {
	public static bool allowDeath;
	public const float MAX_HUNGER = 1000;
	public const float HUNGER_DEC_TIME = 0.01f; // time interval in seconds between energy decrements
	public const int HUNGER_DEC = 1; // amount by which energy is decremented
	private static float hungerTime; // real time since startup at which "energy" was last decremented
	public static float hunger; // referred to as "hunger" in group discussions
						// decrements over time

	// buff variables
	public static float buffSpeed;
	public static float buffJump;
	public static Color buffFogColor;
	public static float buffFogIntensity;
	public static float buffScale;

	// buffs themselves
	public static List<Buff> buffs;

	// AI chase variables
	public static float fugitiveExpire; // time it takes without being seen for fugitive status to expire
	public static float fugitiveTime; // time player was last seen as a fugitive
	private RAINAspect aspect; // AI aspect seen by guards

	// Use this for initialization
	void Start () {
		hungerTime = Time.time;
		hunger = MAX_HUNGER * 0.5f;
		allowDeath = false;
		buffs = new List<Buff>();
		fugitiveTime = 0.0f;
		fugitiveExpire = 10.0f; // initial fugitive expire time
		aspect = aspect = GameObject.Find("AI Aspect").GetComponent<EntityRig>().Entity.GetAspect("aPlayer");
		aspect.IsActive = false; // we do not start as a fugitive
	}

	// Update is called once per frame
	void Update () {
//		Debug.Log(buffSpeed);
		float newHungerTime = Time.time;
		/*	if the real time since the last decrement is at or above
			the ENERGY_DEC_TIME interval, decrement energy by ENERGY_DEC*/
		if(newHungerTime - hungerTime >= HUNGER_DEC_TIME){
			hungerTime = newHungerTime;
			if(hunger > 0){
				hunger -= HUNGER_DEC;
			}
//			Debug.Log(hunger);
		}

		// iterate the linked list of buffs, and aggregate overall buff values
		// also removing expired buffs
		float calcBuffSpeed = 0.0f; // temporary variable to store calculations
		float calcBuffJump = 0.0f;
		float calcBuffFogIntensity = 0.0f;
		float calcBuffScale = 0.0f;
		int i = 0; // index value
		while(i < buffs.Count){ // make sure we are in Count range
			if((Time.timeSinceLevelLoad - buffs[i].time) > buffs[i].duration){
				buffs.RemoveAt(i);
				continue; // go back to top of loop
			}
			calcBuffSpeed += buffs[i].speed;
			calcBuffJump += buffs[i].jump;
			calcBuffScale += buffs[i].scale;
			// for fog we just use the strongest value for intensity
			if(buffs[i].fogIntensity > calcBuffFogIntensity){
				calcBuffFogIntensity = buffs[i].fogIntensity;
			}
			i++;
		}

		if(calcBuffSpeed <= 0){
			calcBuffSpeed = 1;
		}

		if(calcBuffJump <= 0){
			calcBuffJump = 1;
		}

		if(calcBuffScale <= 0){
			calcBuffScale = 1;
		}

		// set calculated values to be public
		buffSpeed = calcBuffSpeed;
		buffJump = calcBuffJump;
		buffFogIntensity = calcBuffFogIntensity;
		buffScale = calcBuffScale;
		if(buffs.Count != 0){
			buffFogColor = buffs[buffs.Count - 1].fogColor;
		}

		// check if a warrent has been issued...
		if((fugitiveTime != 0.0f) && ((fugitiveTime + fugitiveExpire) > Time.timeSinceLevelLoad)){
			aspect.IsActive = true;
		} else{
			aspect.IsActive = false;
		}
	}
}
