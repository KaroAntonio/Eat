/*
	Buff.cs
	Robert Smiley

	Represents changes in player stats.
	Access through PlayerVars.buff

	Contains modifier variables for stats, as well as time and duration

	Modifier variables can be positive (for buffs) or negative (for debuffs)
	These variables should be applied to the player stats in the character
	controller IF AND ONLY IF the buff is not expired.

	Time and duration track how much time the buff is in effect for. Time is
	a timestamp of when the buff was applied. Duration is how long it lasts.
	A buff is expired if: 

		Time.timeSicneLevelLoad - buff.time > duration
*/
using UnityEngine;
using System.Collections;

public class Buff {
	public float time;
	public float duration;
	public float speed;

	public Buff(){
		time = Time.timeSinceLevelLoad;
		duration = 0.0f;
		speed = 0.0f;
	}

	public Buff(float d){
		time = Time.timeSinceLevelLoad;
		duration = d;
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
