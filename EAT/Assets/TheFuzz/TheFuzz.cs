using UnityEngine;
using System.Collections;
using Pathfinding;
using System.Collections.Generic;

public class TheFuzz : MonoBehaviour {

	public enum State {WANDERING, CHASING};

	float nextDirectionTime = 0.0f;
	float wanderingPeriod = 5.0f;
	float wanderSpeed = 1.50f;
	public static float hungerPenalty;

	public List <GameObject> vices = new List<GameObject>();

	public static State state = State.WANDERING;

	//PATHFINDING
	float chasingSpeed = 3.0f;
	private Vector3 target;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {


		switch (state) {
			case State.WANDERING:
				//CHOOSE a random direction to walk in every x seconds
				if (Time.time > nextDirectionTime) {
					ChooseDirection();
					nextDirectionTime += wanderingPeriod;
				}
				transform.Translate(Vector3.forward * Time.deltaTime * wanderSpeed);
				
				break;
			case State.CHASING:
				
				target = GameObject.Find("FPC").transform.position;
				target.y = transform.position.y;	

				transform.LookAt(target);

				transform.Translate(Vector3.forward * Time.deltaTime * chasingSpeed);

				//The Fuzz Catches You
				if (Vector3.Distance (transform.position, target) < 2f) {
					Debug.Log ("TAG");
					PlayerVars.hunger -= hungerPenalty;
					state = State.WANDERING;
					break;
				}
			
				break;
		}
		
	}

	void ChooseDirection () {
		//direction = new Quaternion(Random.Range(0, 360), 1 , Random.Range(0, 360), 0);
		//transform.rotation = direction;
		transform.Rotate( 0,Random.Range (0, 360),0);
	}
	
	void OnTriggerEnter(Collider other) {

		switch (state) {
			case State.WANDERING:
				//IF FUZZY RUNS INTO SOMETHING, Turn around
				transform.Rotate( 0,180,0);
				break;
		}
	}

	void OnTriggerStay(Collider other) {

		if (other.gameObject.name != "FPC") {
			//return;
		}

		switch (state) {
		case State.CHASING:
			//IF FUZZY Finds a vice
			if (vices.Count != 0) {
				foreach (GameObject vice in vices) {
					if (other.gameObject.name == vice.name) {
						Debug.Log ("My Favourite!");
						Destroy(other.gameObject);
						
						//LOSE INTEREST in Player
						state = State.WANDERING;
					}
				}
			}
			break;
		}

		

		
	}
}
