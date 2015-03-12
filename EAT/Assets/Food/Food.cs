using UnityEngine;
using System.Collections;

public class Food : MonoBehaviour {

	FoodShepherd fs;
	public GameObject shepherd;
	public float foodValue;
	//BUFF
	//DEBUFF

	Component halo;

	void Start () {

		fs = (FoodShepherd) shepherd.GetComponent(typeof(FoodShepherd)); 
		halo = GetComponent("Halo");

		//INITIALIZE Halo to OFF
		halo.GetType().GetProperty("enabled").SetValue(halo, false, null);
	}

	void Update () {
		//SPIN
		//transform.Rotate (transform.forward * 85f * Time.deltaTime);
		//transform.Rotate (transform.right * 85f * Time.deltaTime);
		//transform.Rotate (transform.up * 85f * Time.deltaTime);
	}

	void OnTriggerStay(Collider other) {

		if (other.name != "FPC")
						return;
		//ILLUMINATE Food
		halo.GetType().GetProperty("enabled").SetValue(halo, true, null);

		//GRAB FOOD
		if (Input.GetMouseButtonDown (0)) {
				Debug.Log ("EATEN");

				//EAT
				Destroy(gameObject);
				PlayerVars.hunger += foodValue;

				//CHECK if you got caught 
				if (shepherd.name != "BlindShepherd") {

					float angle = Vector3.Angle(shepherd.transform.forward, transform.position - shepherd.transform.position);
					
					if (angle < 70f) {
						
						fs.head = fs.transform.FindChild ("base/spine/spine_up/head");
						RaycastHit hit = new RaycastHit();
						float distance = Vector3.Distance(transform.position,fs.head.position);
						Vector3 direction = (transform.position - fs.head.position);

						Debug.DrawRay(fs.head.position, direction, Color.magenta);

						if (Physics.Raycast(fs.head.transform.position , direction ,out hit, distance)) {
							Debug.Log (hit.collider.ToString());

							//CAUGHT: Fuzzy tries to hit you for the value of the food stolen
						if ((hit.collider.transform.position == transform.position) || (hit.collider.gameObject.name == "FPC")) {
								Debug.Log ("CAUGHT!");
								
								fs.displayStolenFoodText();
								
								TheFuzz.state = TheFuzz.State.CHASING;
								TheFuzz.hungerPenalty += foodValue;
							
							} else {
								Debug.Log ("OBSCURED");
							}
							
						} else {
							Debug.Log ("VISIBLE");
						}
						
						Debug.Log ("I SEEE YOU");
					} else {
						Debug.Log ("I AM BLISSFULLY UNAWARE");
					}	
				}
		}
	}

	void OnTriggerExit(Collider other) {
		halo.GetType().GetProperty("enabled").SetValue(halo, false, null);
		
	}

}
