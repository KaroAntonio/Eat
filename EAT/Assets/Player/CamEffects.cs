using UnityEngine;
using System.Collections;

public class CamEffects : MonoBehaviour {
	private GameObject speedIcon;
	private GameObject jumpIcon;
	private GameObject blindIcon;
	private GameObject sizeIcon;

	// Use this for initialization
	void Start () {
		speedIcon = GameObject.Find("SpeedIcon");
		jumpIcon = GameObject.Find("JumpIcon");
		blindIcon = GameObject.Find("BlindIcon");
		sizeIcon = GameObject.Find("SizeIcon");

		RenderSettings.fog = true;
		RenderSettings.fogStartDistance = 0.0f;
		RenderSettings.fogDensity = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
		RenderSettings.fogDensity = PlayerVars.buffFogIntensity;
		RenderSettings.fogColor = PlayerVars.buffFogColor;

		if(PlayerVars.buffSpeed == 1.0f){
			speedIcon.SetActive(false);
		} else{
			speedIcon.SetActive(true);
		}

		if(PlayerVars.buffJump == 1.0f){
			jumpIcon.SetActive(false);
		} else{
			jumpIcon.SetActive(true);
		}

		if(PlayerVars.buffFogIntensity == 0.0f){
			blindIcon.SetActive(false);
		} else{
			blindIcon.SetActive(true);
		}

		if(PlayerVars.buffScale == 1.0f){
			sizeIcon.SetActive(false);
		} else{
			sizeIcon.SetActive(true);
		}
		
	}
}
