using UnityEngine;
using System.Collections;

public class Chicken : MonoBehaviour {

	void OnTriggerEnter(Collider other) {
				if ((other.name == "Magnet") || (other.name == "Magnet")) {
						gameObject.rigidbody.isKinematic = false;	
				} 
		}
}
