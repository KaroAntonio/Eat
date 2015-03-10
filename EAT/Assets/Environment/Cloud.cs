using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Cloud : MonoBehaviour {

	public List <GameObject> items = new List<GameObject>();
	public float spawnSpeed;

	// Use this for initialization
	void Start () {
		InvokeRepeating("spawnItem", 0 , spawnSpeed);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void spawnItem() {

		Vector3 spawnPos = transform.position;

		int i = Random.Range (0, items.Count);

		GameObject newItem = (GameObject) Instantiate (
			items[i], 
			spawnPos, 
			Quaternion.identity);

		newItem.name = items [i].name;

		newItem.rigidbody.AddForce(transform.up * 200f + 
		                           transform.right * Random.Range (-500,500) + 
		                           transform.forward * Random.Range(-500,500));

		newItem.rigidbody.MoveRotation (newItem.rigidbody.rotation * Quaternion.Euler(transform.right * Random.Range (-500,500) * Time.deltaTime));
	}
}
