using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GenericTexts : MonoBehaviour {

	public static List <string> stolenTexts = new List<string>();

	void Start () {
		stolenTexts.Add ("My Food Babay!");
		stolenTexts.Add ("That was my last Food!");
		stolenTexts.Add ("Help! He took my favourite food!");
		stolenTexts.Add ("So Rude");
	}
}
