using UnityEngine;
using System.Collections;

public class Food : MonoBehaviour {

	FoodShepherd fs;
	public GameObject shepherd;
	public Transform head;
	public float foodValue;
	//BUFF
	//DEBUFF

	Component halo;

	// Use this for initialization
	void Start () {
		fs = (FoodShepherd) shepherd.GetComponent(typeof(FoodShepherd)); 
		halo = GetComponent("Halo");
		halo.GetType().GetProperty("enabled").SetValue(halo, false, null);
	}
	
	// Update is called once per frame
	void Update () {
		//SPIN
		transform.Rotate (transform.forward * 85f * Time.deltaTime);
		transform.Rotate (transform.right * 85f * Time.deltaTime);
		transform.Rotate (transform.up * 85f * Time.deltaTime);
	}

	void OnTriggerStay(Collider other) {

		//ILLUMINATE Food
		halo.GetType().GetProperty("enabled").SetValue(halo, true, null);

		//GRAB FOOD
		if (Input.GetMouseButtonDown (0)) {
				Debug.Log ("EATEN");

				//EAT
				Destroy(gameObject);

				//CHECK if you got caught 
				float angle = Vector3.Angle(shepherd.transform.forward, transform.position - shepherd.transform.position);
				if (angle < 90f) {
					
					RaycastHit hit = new RaycastHit();
					float distance = Vector3.Distance(transform.position,fs.head.position);
					Vector3 direction = (transform.position - fs.head.position);

					Debug.DrawRay(fs.head.position, direction, Color.magenta);

					if (Physics.Raycast(fs.head.transform.position , direction ,out hit, distance)) {
						Debug.Log (hit.collider.ToString());
						if (hit.collider.transform.position == transform.position) {
							Debug.Log ("CAUGHT!");
							
							//DO THIS BETTER
							TheFuzz.state = TheFuzz.State.CHASING;
						
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

	void OnTriggerExit(Collider other) {
		halo.GetType().GetProperty("enabled").SetValue(halo, false, null);
		
	}

}
