using UnityEngine;
using System.Collections;

public class FuzzySpawn : MonoBehaviour {

	public GameObject fuzzler;

	void Start () { }
	void Update () { }

	public void spawnFuzzler() {

		Vector3 spawnPos = transform.position;

		GameObject newFuzzler = (GameObject) Instantiate (
			fuzzler, 
			spawnPos, 
			Quaternion.identity);

		newFuzzler.name = fuzzler.name;
	}
}
