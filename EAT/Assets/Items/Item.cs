using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {

	enum State {HELD, RESTING};

	State state = State.RESTING;


	Component halo;

	// Use this for initialization
	void Start () {
		halo = GetComponent("Halo");
		halo.GetType().GetProperty("enabled").SetValue(halo, false, null);

	}
	
	// Update is called once per frame
	void Update () {


		switch (state) {
			case State.HELD:
				if (!Input.GetMouseButton (0)) {
					state = State.RESTING;
					rigidbody.isKinematic = false;
					transform.parent = null;
				}
				break;
		}
		
	}

	void OnTriggerStay(Collider other) {

		if (other.gameObject.name != "FPC") {
						return;
		}

		Debug.Log ("Item Triggered");
		Debug.Log (other.gameObject.name);

		//ILLUMINATE Food
		halo.GetType().GetProperty("enabled").SetValue(halo, true, null);
		
		//GRAB FOOD
		if (Input.GetMouseButtonDown (0)) {

			state = State.HELD;

			rigidbody.isKinematic = true;	

			transform.parent = other.transform;
			transform.position = 
				new Vector3 (
					other.transform.position.x - 1.5f, 
					other.transform.position.y + 2.2f,
					other.transform.position.z + 1.5f
						);

		}
	}

	void OnTriggerExit(Collider other) {
		halo.GetType().GetProperty("enabled").SetValue(halo, false, null);
	}
}
