/*
	PlayerUI.cs
*/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerUI : MonoBehaviour {
	private const float HUNGER_BAR_WIDTH = 400.0f;
	private const float HUNGER_BAR_HEIGHT = 6.0f;
	private const float HUNGER_FILL_HEIGHT = 14.0f;
	private const float HUNGER_BAR_X = 2.8f;
	private const float HUNGER_BAR_Y = 139.9f;

	private Image hungerBar;
	private Image hungerFill;
	private RawImage stomach;
	private Text timeText;

	// Use this for initialization
	void Start () {
		Screen.lockCursor = true;
		GUI.color = new Color32(255, 255, 255, 100);
		hungerBar = GameObject.Find("HungerBar").GetComponent<Image>();
		hungerFill = GameObject.Find("HungerFill").GetComponent<Image>();
		timeText = GameObject.Find("TimeText").GetComponent<Text>();
		stomach = GameObject.Find("Stomach").GetComponent<RawImage>();
		hungerBar.rectTransform.sizeDelta = new Vector2(HUNGER_BAR_WIDTH, HUNGER_BAR_HEIGHT);
		hungerFill.rectTransform.sizeDelta = new Vector2(HUNGER_BAR_WIDTH, HUNGER_FILL_HEIGHT);
		hungerBar.rectTransform.anchoredPosition = new Vector2(0, (0 - Screen.height / 2) + 20);
		timeText.rectTransform.anchoredPosition = new Vector2(40, (0 - Screen.height / 2) + 35);
		stomach.rectTransform.anchoredPosition = new Vector2(220, (0 - Screen.height / 2) + 20);
	}
		
	// Update is called once per frame
	void Update () {
		float hungerRatio = (float)PlayerVars.hunger / (float)PlayerVars.MAX_HUNGER;
		float displayedHunger = hungerRatio * HUNGER_BAR_WIDTH;
		hungerFill.rectTransform.sizeDelta = new Vector2(displayedHunger, HUNGER_FILL_HEIGHT);
		hungerFill.rectTransform.anchoredPosition = new Vector2(HUNGER_BAR_X + ((HUNGER_BAR_WIDTH - displayedHunger) / 2)
																, hungerBar.transform.position.y);

		// set time text to time in minutes and seconds since level start
		timeText.text = ((int)(Time.timeSinceLevelLoad / 60)).ToString("000") + ":" +
			(Time.timeSinceLevelLoad % 60).ToString("00");
	}
}
