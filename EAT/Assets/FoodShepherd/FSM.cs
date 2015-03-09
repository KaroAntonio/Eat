using UnityEngine;
using System.Collections;

public class FSM : MonoBehaviour {

	enum State {Waving, Giving, Walking};

	State state = State.Waving;
	public Animator animator;

	Transform pom;

	// Use this for initialization
	void Start () {
		//animator.animation["SpellIncantation03"].wrapMode = WrapMode.Loop; 

		animator.SetBool("Giving", false);
		pom = transform.FindChild ("base/spine/spine_up/right_arm/right_foret_arm/right_hand/mount3/Pomegranate");
		Debug.Log (pom.ToString());
	}
	
	// Update is called once per frame
	void Update () {


	}

	void OnTriggerStay(Collider other) {
		if (state == State.Waving) {
			if(Input.GetMouseButtonDown(0)) {
				state = State.Giving;
				animator.SetBool("Giving", true);
				pom.parent = other.transform;
				pom.position = 
					new Vector3(
						other.transform.position.x, 
						other.transform.position.y + 1f,
						other.transform.position.z + 1f
						);
			}
		}
	}
}
