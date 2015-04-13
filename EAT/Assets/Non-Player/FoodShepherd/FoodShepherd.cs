using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FoodShepherd : MonoBehaviour {

	enum State {Watching, Walking, Giving, Shouting};


	public GameObject food;
	public List <GameObject> vices = new List<GameObject>();
	public List <GameObject> rewards = new List<GameObject>();
	public List <GameObject> specialRewards = new List<GameObject>();
	public List <GameObject> phobias = new List<GameObject>();
	public GameObject distraction;

	private int specialRewardCountdown;
	public int specialRewardInterval = 3;

	public TextList textList;
	public GameObject text;
	float textSpeed = 10f;

	private int textindex = 0;

	public Transform head;

	void Start () {

		specialRewardCountdown = specialRewardInterval;

		//TEXT Display Initiation
		if ((text != null) && (textList != null)) {
			textList.addText();
			//Debug.Log(textList.texts.Count);
			if (textList.texts.Count > 0) {
				((SmartTextMesh)text.GetComponent ("SmartTextMesh")).UnwrappedText = textList.texts [textindex % textList.texts.Count];
				InvokeRepeating ("updateText", Random.Range (0, textSpeed) + textSpeed, textSpeed);
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (distraction != null) {
			Vector3 distraction2d = new Vector3 (
				distraction.transform.position.x, 	
				transform.position.y, 
				distraction.transform.position.z);
			transform.LookAt (distraction2d);

		}

	}

	void OnTriggerStay(Collider other) {

		if (vices.Count != 0) {
			foreach (GameObject vice in vices) {
				if (other.gameObject.name == vice.name) {
					Debug.Log ("My Favourite!");
					Destroy(other.gameObject);

					--specialRewardCountdown;

					//Spawn Reward
					Vector3 spawnPos = transform.position;
					spawnPos.y += 2f;

					//Regular Reward or special Reward
					int i;
					GameObject reward;
					if ((specialRewardCountdown == 0) && (specialRewards.Count > 0)){
						i = Random.Range (0, specialRewards.Count);
						reward = specialRewards[i];
						specialRewardCountdown = specialRewardInterval;
					} else {
						i = Random.Range (0, rewards.Count);
						reward = rewards[i];
					}

					GameObject newReward = (GameObject) Instantiate (
						reward, 
						spawnPos, 
						Quaternion.identity);

					newReward.name = reward.name;
					newReward.rigidbody.AddForce(transform.up * 200f + 
					                           transform.right * Random.Range (-500,500) + 
					                           transform.forward * Random.Range(-500,500));
				}
			}
		}
	}

	public void displayStolenFoodText() {
		((SmartTextMesh)text.GetComponent ("SmartTextMesh")).UnwrappedText = GenericTexts.stolenTexts[Random.Range (0,GenericTexts.stolenTexts.Count)];
	}

	void updateText() {
		textindex++;
		((SmartTextMesh)text.GetComponent ("SmartTextMesh")).UnwrappedText = textList.texts [textindex % textList.texts.Count];
	}
}
