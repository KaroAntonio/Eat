using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {



	//Constnts
	public float velocity = 5.0F;
	private Vector3 direction = Vector3.zero;
	public float jumpVelocity = 15.0F; 
	public float gravity = 23.0F;
	float rotationY = 0;
	public Animator animator;
	public static GameObject player;
	CharacterController controller;

	void Start(){
		controller = GetComponent<CharacterController>();
		animator = GetComponent<Animator>();
	}

	void Update() {
		player = gameObject;

		//Only move/ jump from ground
		if (controller.isGrounded) {
				direction = new Vector3 (Input.GetAxis ("Straft"), 0, Input.GetAxis ("Forward"));

				direction = transform.TransformDirection(direction);

				direction *= velocity;

				animator.SetBool("Jumping", false);
				
				if (direction != Vector3.zero) {
					animator.SetBool("Running", true);
				}
				
				
				
				//Jump
				if (Input.GetButton ("Jump")) {
						animator.SetBool("Running", false);
						animator.SetBool("Jumping", true);
						
						direction.y = jumpVelocity;
				}
	
		} else {
			animator.SetBool("Jumping", true);
			animator.SetBool("Running", false);

		}

		//Mouse Yaw
		transform.Rotate(0, Input.GetAxis("Mouse X") * 1, 0);

		//Mouse Pitch
		rotationY += Input.GetAxis("Mouse Y") * 1;
		rotationY = Mathf.Clamp (rotationY, -60, 60);
		transform.localEulerAngles = new Vector3(-rotationY, transform.localEulerAngles.y, 0);

		//Fall
		direction.y -= gravity * Time.deltaTime;


		//Making the character move
		controller.Move(direction * Time.deltaTime);
	}
}