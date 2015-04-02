using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using RAIN.Core;
using RAIN.Perception;
using RAIN.Entities.Aspects;


public class Chase : MonoBehaviour {
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
	
	// Update is called once per frame
	void Update () {
		if(seePlayer()){
			// update player warrent
			PlayerVars.fugitiveTime = Time.timeSinceLevelLoad;
		}
	}
}
