using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FoodShepherd : MonoBehaviour {

	enum State {Watching, Walking, Giving, Shouting};

	public GameObject food;
	public List <GameObject> vices = new List<GameObject>();
	public List <GameObject> rewards = new List<GameObject>();
	public List <GameObject> phobias = new List<GameObject>();
	public GameObject distraction;

	public Transform head;

	// Use this for initialization
	void Start () {
		head = transform.FindChild ("base/spine/spine_up/head");
	}
	
	// Update is called once per frame
	void Update () {
		if (distraction != null) {
						//transform.LookAt (distraction.transform);
						var lookPos = distraction.transform.position - transform.position;
						lookPos.y = 0;
						var rotation = Quaternion.LookRotation (lookPos);
						transform.rotation = Quaternion.Slerp (transform.rotation, rotation, Time.deltaTime);
				}

	}

	void OnTriggerStay(Collider other) {


		if (other.gameObject.name != "FPC") {
			//return;
		}

		if (vices.Count != 0) {
			foreach (GameObject vice in vices) {
				if (other.gameObject.name == vice.name) {
					Debug.Log ("My Favourite!");
					Destroy(other.gameObject);

					//Spawn Reward
					Vector3 spawnPos = transform.position;
					spawnPos.y += 2f;
					int i = Random.Range (0, rewards.Count);
					GameObject newReward = (GameObject) Instantiate (
						rewards[i], 
						spawnPos, 
						Quaternion.identity);

					newReward.name = rewards [i].name;
					newReward.rigidbody.AddForce(transform.up * 200f + 
					                           transform.right * Random.Range (-500,500) + 
					                           transform.forward * Random.Range(-500,500));
				}
			}
		}

	}
}
