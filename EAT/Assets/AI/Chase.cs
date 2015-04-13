using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Core;
using RAIN.Perception;
using RAIN.Entities.Aspects;


public class Chase : MonoBehaviour {
	public const float DAMAGE_PLAYER = 100.0f;
	private RAINSenses senses;

	private bool seePlayer(){
		IList<RAINAspect> aspects = senses.SenseAll();
		foreach(RAINAspect i in aspects){
			if(i.AspectName == "aPlayer"){
				return true;
			}
		}
		return false;
	}

	// Use this for initialization
	void Start () {
		senses = GameObject.Find("AI").GetComponent<AIRig>().AI.Senses;
	}

	void OnTriggerStay(Collider other){
		if(other.name == "FPC" && seePlayer()){
			Debug.Log("Damage!");
			PlayerVars.hunger -= DAMAGE_PLAYER;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(seePlayer()){
			// update player warrent
			PlayerVars.fugitiveTime = Time.timeSinceLevelLoad;
		}
	}
}
