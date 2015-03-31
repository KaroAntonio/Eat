using UnityEngine;
using System.Collections;

public class CamEffects : MonoBehaviour {

	// Use this for initialization
	void Start () {
		RenderSettings.fog = true;
		RenderSettings.fogStartDistance = 0.0f;
		RenderSettings.fogDensity = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
		RenderSettings.fogDensity = PlayerVars.buffFogIntensity;
		RenderSettings.fogColor = PlayerVars.buffFogColor;
	}
}
