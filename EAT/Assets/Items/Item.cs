using UnityEngine;
using System.Collections;

public class Item : MonoBehaviour {

	enum State {HELD, RESTING};

	State state = State.RESTING;

	Camera maincamera;

	Component halo;

	// Use this for initialization
	void Start () {

		halo = GetComponent("Halo");
		halo.GetType().GetProperty("enabled").SetValue(halo, false, null);
		maincamera = GameObject.Find("Main Camera").GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {


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

		if (other.gameObject.name == "Magnet") {
			transform.LookAt (other.transform);
			rigidbody.AddForce(transform.forward * Magnet.magneticForce);
		} else if (other.gameObject.name == "AntiMagnet") {
			transform.LookAt (other.transform);
			rigidbody.AddForce(transform.forward * AntiMagnet.magneticForce);
		}

		if (other.gameObject.name != "FPC") {
						return;
		}

		//ILLUMINATE Item
		halo.GetType().GetProperty("enabled").SetValue(halo, true, null);
		
		//GRAB Item
		if (Input.GetMouseButtonDown (0)) {

			state = State.HELD;

			rigidbody.isKinematic = true;	

			transform.parent = other.transform;
			transform.position = 
				maincamera.transform.position + 
					maincamera.transform.forward * 3;
			Debug.Log(other.transform.forward.ToString());
			transform.parent = maincamera.transform;

		}
	}

	void OnTriggerExit(Collider other) {
		halo.GetType().GetProperty("enabled").SetValue(halo, false, null);
	}
}
