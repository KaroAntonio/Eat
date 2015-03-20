using UnityEngine;
using System.Collections;

/** 
 * Charles Logan
 * 
 * Simple cam pan controls. MMO style. 
 * Right-click and mouse pain. zoom with wheel. Middle button resets. Left Change Vew.
 * 
 * To use: 
 * You will need to know the camera number ordaer. Here cam 0 is 1st person and cam 1 has the audio Listener.
 *	Set main cam as a child and then place this script on the cam.
 */
public class Cam_controller : MonoBehaviour
{

	const float moveSpeed = 1f;
	private Vector3 origin_pos;
	private Quaternion origin_rot;
	private Camera[] cam;
	private int cam_i;
	AudioListener sound;
	private float look_x;
	private bool cam_lock;

	// Use this for initialization
	void Start ()
	{
		origin_pos = transform.localPosition;
		origin_rot = transform.localRotation;
		cam = Camera.allCameras;
		cam_i = 1;
		sound = cam [1].GetComponent<AudioListener> ();
	
	}

	// Update is called once per frame

		
	void Update ()
	{	

		if (cam_i == 0) {
			look_x += -Input.GetAxis ("Mouse Y") * moveSpeed * Time.deltaTime;
			look_x = Mathf.Clamp (look_x, -90, 90);
			cam [0].transform.localEulerAngles = new Vector3 (look_x, 0, 0);
		} 
		else {
			if (Input.GetButton ("viewReset")) {
				transform.localPosition = origin_pos;
				transform.localRotation = origin_rot;
			}

			if (Input.GetButton ("unlockCam")) {
				transform.RotateAround (transform.parent.position, Vector3.up, Input.GetAxis ("Mouse X") * moveSpeed * Time.deltaTime);
				transform.RotateAround (transform.parent.position, transform.right, -Input.GetAxis ("Mouse Y") * moveSpeed * Time.deltaTime);
			}

			float scroll = Input.GetAxis ("ScrollWheel");
			if (scroll != 0) {
				transform.Translate (Vector3.forward * scroll * moveSpeed * Time.deltaTime);
			}
		}
		// activate next cam. Move sound AudioListener to this.
		if (Input.GetButtonDown ("camChange")) {

			cam [cam_i].enabled = false;
			cam_i++;
			if (!(cam_i < cam.Length)) {
				cam_i = 0;
			}
			cam [cam_i].enabled = true;
			sound.transform.position = transform.position;
		}
	}
}

