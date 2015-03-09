/*
	PlayerUI.cs
*/

using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerUI : MonoBehaviour {
	private const float ENERGY_BAR_WIDTH = 400.0f;
	private const float ENERGY_BAR_HEIGHT = 10.0f;
	private const float ENERGY_BAR_X = 2.8f;
	private const float ENERGY_BAR_Y = 139.9f;

	private Image energyBar;
	private Image energyFill;

	// Use this for initialization
	void Start () {
		energyBar = GameObject.Find("EnergyBar").GetComponent<Image>();
		energyFill = GameObject.Find("EnergyFill").GetComponent<Image>();
		energyBar.rectTransform.sizeDelta = new Vector2(ENERGY_BAR_WIDTH, ENERGY_BAR_HEIGHT);
		energyFill.rectTransform.sizeDelta = new Vector2(ENERGY_BAR_WIDTH, ENERGY_BAR_HEIGHT);
	}
		
	// Update is called once per frame
	void Update () {
		float relativeEnergy = (float)PlayerVars.energy / (float)PlayerVars.MAX_ENERGY;
		float displayedEnergy = relativeEnergy * ENERGY_BAR_WIDTH;
		energyFill.rectTransform.sizeDelta = new Vector2(displayedEnergy, ENERGY_BAR_HEIGHT);
		energyFill.rectTransform.anchoredPosition = new Vector2(ENERGY_BAR_X + ((ENERGY_BAR_WIDTH - displayedEnergy) / 2)
																, ENERGY_BAR_Y);
	}
}
