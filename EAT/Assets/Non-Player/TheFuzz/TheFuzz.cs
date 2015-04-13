using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TheFuzz : MonoBehaviour
{

		public enum State
		{
				WANDERING,
				CHASING}
		;

		float nextDirectionTime = 0.0f;
		float wanderingPeriod = 5.0f;
		float wanderSpeed = 1.50f;
		public static float hungerPenalty;
		public List <GameObject> vices = new List<GameObject> ();
		public TextList textList;
		public GameObject text;
		float textSpeed = 10f;
		private int textindex = 0;
		private CharacterController control;
		public static State state = State.WANDERING;

		//PATHFINDING
		float chasingSpeed = 3.0f;
		private Vector3 target;

		// Use this for initialization
		void Start ()
		{

				control = GetComponent<CharacterController> ();

				//TEXT Display Initiation
				if ((text != null) && (textList != null)) {
						textList.addText ();
						Debug.Log (textList.texts.Count);
						if (textList.texts.Count > 0) {
								((SmartTextMesh)text.GetComponent ("SmartTextMesh")).UnwrappedText = textList.texts [textindex % textList.texts.Count];
								InvokeRepeating ("updateText", Random.Range (0, textSpeed) + textSpeed, textSpeed);
						}
				}

		}
	
		// Update is called once per frame
		void Update ()
		{
				switch (state) {
				case State.WANDERING:
				//CHOOSE a random direction to walk in every x seconds
						if (Time.time > nextDirectionTime) {
								ChooseDirection ();
								nextDirectionTime += wanderingPeriod;
						}
				//transform.Translate(Vector3.forward * Time.deltaTime * wanderSpeed);
						control.SimpleMove (Vector3.forward * Time.deltaTime * wanderSpeed);
						break;
				case State.CHASING:

						((SmartTextMesh)text.GetComponent ("SmartTextMesh")).UnwrappedText = "Drop that you Skamp!";
				
						target = GameObject.Find ("FPC").transform.position;
						target.y = transform.position.y;	

						transform.LookAt (target);

				//transform.Translate(Vector3.forward * Time.deltaTime * chasingSpeed);
						control.SimpleMove (Vector3.forward * Time.deltaTime * chasingSpeed);
				//CAUGHT and Punished
						if (Vector3.Distance (transform.position, target) < 2f) {
								updateText ();
								Debug.Log ("TAG");
								PlayerVars.hunger -= hungerPenalty;
								state = State.WANDERING;
								hungerPenalty = 0;
								break;
						}
			
						break;
				}
		
		}

		void ChooseDirection ()
		{
				//direction = new Quaternion(Random.Range(0, 360), 1 , Random.Range(0, 360), 0);
				//transform.rotation = direction;
				transform.Rotate (0, Random.Range (0, 360), 0);
		}
	
		void OnTriggerEnter (Collider other)
		{

				switch (state) {
				case State.WANDERING:
				//IF FUZZY RUNS INTO SOMETHING, Turn around
						transform.Rotate (0, 180, 0);
						break;
				}
		}

		void OnTriggerStay (Collider other)
		{

				if (other.gameObject.name != "FPC") {
						//return;
				}

				switch (state) {
				case State.CHASING:
			//IF FUZZY Finds a vice
						if (vices.Count != 0) {
								foreach (GameObject vice in vices) {
										if (other.gameObject.name == vice.name) {
												Debug.Log ("My Favourite!");
												Destroy (other.gameObject);
						
												//LOSE INTEREST in Player
												state = State.WANDERING;
										}
								}
						}
						break;
				}
		}

		void updateText ()
		{
				textindex++;
				((SmartTextMesh)text.GetComponent ("SmartTextMesh")).UnwrappedText = textList.texts [textindex % textList.texts.Count];
		}
}
