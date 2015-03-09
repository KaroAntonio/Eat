/*
	PlayerUI.cs
*/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerUI : MonoBehaviour {
	private const float HUNGER_BAR_WIDTH = 400.0f;
	private const float HUNGER_BAR_HEIGHT = 10.0f;
	private const float HUNGER_BAR_X = 2.8f;
	private const float HUNGER_BAR_Y = 139.9f;

	private Image hungerBar;
	private Image hungerFill;

	// Use this for initialization
	void Start () {
		hungerBar = GameObject.Find("HungerBar").GetComponent<Image>();
		hungerFill = GameObject.Find("HungerFill").GetComponent<Image>();
		hungerBar.rectTransform.sizeDelta = new Vector2(HUNGER_BAR_WIDTH, HUNGER_BAR_HEIGHT);
		hungerFill.rectTransform.sizeDelta = new Vector2(HUNGER_BAR_WIDTH, HUNGER_BAR_HEIGHT);
	}
		
	// Update is called once per frame
	void Update () {
		float hungerRatio = (float)PlayerVars.hunger / (float)PlayerVars.MAX_HUNGER;
		float displayedHunger = hungerRatio * HUNGER_BAR_WIDTH;
		Debug.Log ("HUNGER:" + PlayerVars.hunger);
		hungerFill.rectTransform.sizeDelta = new Vector2(displayedHunger, HUNGER_BAR_HEIGHT);
		//hungerFill.rectTransform.anchoredPosition = new Vector2(HUNGER_BAR_X + ((HUNGER_BAR_WIDTH - displayedEnergy) / 2)
		//														, HUNGER_BAR_Y);
	}
}
