using UnityEngine;
using System.Collections;

public class Pom : MonoBehaviour {



	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (transform.forward * 85f * Time.deltaTime);
		transform.Rotate (transform.right * 85f * Time.deltaTime);
		transform.Rotate (transform.up * 85f * Time.deltaTime);
	}
}
