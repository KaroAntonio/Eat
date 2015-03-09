using UnityEngine;
using System.Collections;
using Pathfinding;

public class TheFuzz : MonoBehaviour {

	public enum State {WANDERING, CHASING};

	float nextDirectionTime = 0.0f;
	float wanderingPeriod = 5.0f;
	float wanderSpeed = 1.50f;

	public static State state = State.CHASING;

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

		Debug.Log (other.name);

		switch (state) {
			case State.WANDERING:
				Debug.Log ("Fuzzy says Ouch");
				//IF FUZZY RUNS INTO SOMETHING
			transform.Rotate( 0,180,0);
				break;
		}
	}
}
