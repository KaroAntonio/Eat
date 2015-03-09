using UnityEngine;
using System.Collections;

public class FoodShepherd : MonoBehaviour {

	enum State {Watching, Walking, Giving, Shouting};

	public GameObject food;
	public GameObject vice;
	public GameObject reward;

	public Transform head;

	// Use this for initialization
	void Start () {
		head = transform.FindChild ("base/spine/spine_up/head");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerStay(Collider other) {
		
		if (other.gameObject.name != "FPC") {
			//return;
		}

		if (vice != null) {
			if (other.gameObject.name == vice.name) {
				Debug.Log ("My Favourite!");
				Destroy(other.gameObject);

				//Spawn Reward
				Vector3 spawnPos = transform.position;
				spawnPos.y += 2f;
				GameObject newReward = (GameObject) Instantiate (reward, spawnPos, Quaternion.identity);
				newReward.rigidbody.AddForce(transform.up * 200f + (new Vector3(Random.Range(0,360),0,Random.Range(0,360))));

			}
		}

	}
}
