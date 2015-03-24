using UnityEngine;
using System.Collections;

/**
 * Charles Logan
 *
 * A rigibody player controller. wasd input and mouse turn affect the force of torque of the object.
 * Collsions on the body can affect both the posision and rotation (y-axis).
 * The camera holder is a smooth alignment to the head point, this is open for 3rd person cam logic.
 * Player states are handled by tracking the animator. This allows the animation to sync changes.
 *
 * Currently does forward/back, left/right, mouse turning, jumping, sprint.
 *
 * 
 */
using System;

public class Player_physics_controller : MonoBehaviour
{
		// Robert Smiley
		// buff values
		// values extracted from the current player buff
		private float buffSpeed;

		// physics values.
		// Naming convention between velocity, accel, or force maybe wrong.
		public float mouse_sensitivity = 1;
		public  float MOVE_ACCEL = 50;			// this control force of movement. MODE.ACCEL
		public float TURN_ACCEL = 25f;				// force body turns to were it looks.
		private const  float BREAK_CONTROL = .5f;			// break force were zx input is zero.	
		private float SLOPE_SLIP_ANGLE = Mathf.Cos (50 * Mathf.Deg2Rad);
		private const float IMPUSLE = 100f;
		private const float RUN_MAX_SPEED = 5;			// velocity cap running.
		private const float MOVE_MAX_SPEED = 3.5f;		// walking cap.
		private const float MOVE_FORCE_RATIO = RUN_MAX_SPEED / MOVE_MAX_SPEED;	// sorta used to control animation sync.
		private const float MOVE_BACK_LIMITER = .7f;	// walk back slower.
		private const float MOVE_SPRINT_A = .1f;		// sprint rampup speed.
		private const float MOVE_SPRINT_DA = .2f;		// down speed.
		private const float MOVE_RESIST_LIMITER = .99f;	// drag when stopping.
		private const float ROTATION_MAX_SPEED = 270;
		private const float ROTATE_ACCEL = 0f;
		private const float ROTATE_RESIST = ROTATION_MAX_SPEED * 2.9f;
		private const float JUMP_UPLIFT = 30;			// jump force.
		private const float JUMP_CONTROL = .09f;		// reduces jump accel as to air-hang longer.
		private const float INVS_RUN_MAX_SPEED = 1f / RUN_MAX_SPEED;
		private const float INVS_ROTATION_MAX_SPEED = 1f / ROTATION_MAX_SPEED;
		private const float TINY_NUM = 0.001f;


		// physics stuff - some can be edit as buffs. Will be simplifyed.
		public float forward_a;					// the result forward forces to apply on body.
		public float sprint_p;					// ratio of extra power when running. Controlled by sprint key.
		public float xz_a_limiter;			// drop walking power.  [0,1], auto sets to 1
		public float base_forward_a;			// base accel from forward keys.
		public float base_strafe_a;
		public float jump_a;					// inital jump accel upwards. drops quickly
		public float jump_a_lift;				// a % value for reducing jump_a each frame. Low is unnatural.
		public float input_z;					// forward input.
		public float input_x;					// strafe input.
		public float input_y;					// jump input.
		public float targetLook_x;				// for smooth look. player will look here.
		public float targetLook_y;
		public bool is_running;					// sprint logic.
		public float drag_dynamic;				// body drag. acts as foot breaks.
		private Vector3 ave_slope;				// downwards slope raycasts are averged to be smoothed. changed in small rads.
		public float look_rotation;					// forward facing rotate. for mouse basis.
		private float rotation_y;						// mouse look delta.
		private float rotation_x;
		public float rot_a_y;					// look rotation
		public float rot_a_x;
		Quaternion look;
		//animation states and hashed vars for speed.
		private AnimatorStateInfo currentAnimState;
		//body states
		private int st_idle = Animator.StringToHash ("Body Base Layer.Idle");
		private int st_jump = Animator.StringToHash ("Body Base Layer.Jump States Blend Tree");
		private int st_zx_move = Animator.StringToHash ("Jump States Blend Tree.Horizonal Move Blend Tree");
		private int st_hit = Animator.StringToHash ("Jump States Blend Tree.Hit");
		private int st_stunned = Animator.StringToHash ("Jump States Blend Tree.Stunned");
		private int st_dead = Animator.StringToHash ("Jump States Blend Tree.Dead");

		// arms and head states
		private int st_upper_idle = Animator.StringToHash ("Arms And head Layer.Idle");
		private int st_grabbing = Animator.StringToHash ("Arms And head Layer.Grabbing");
		private int st_holding = Animator.StringToHash ("Arms And head Layer.Holding");
		private int st_eating = Animator.StringToHash ("Arms And head Layer.Eating");

		// state parameters hashed. 
		private int isJumping = Animator.StringToHash ("isJump_bool");
		private int isGrounded = Animator.StringToHash ("isGrounded_bool");
		private int mov_axis_z = Animator.StringToHash ("move_z");	// normalized for a blend animator.
		private int mov_axis_x = Animator.StringToHash ("move_x");  // normalized for a blend animator.
		private int mov_axis_y = Animator.StringToHash ("move_y");  // normalized for a blend animator.
		private int jump_st_y = Animator.StringToHash ("jumpingState_float"); // likely to be move_y later.
		private int turn_y = Animator.StringToHash ("turn_y");
		private int isMov_zx = Animator.StringToHash ("isMoving_zx_bool");
		private int isHit = Animator.StringToHash ("hit_bool");
		private int isStunned = Animator.StringToHash ("stunned_bool");
		private int isDead = Animator.StringToHash ("dead_bool");

		//
		public bool onGround;

		// components on the player model.
		Ray feetRay;					// feet collision
		RaycastHit hit;					// feet collision
		public Camera headCamera;
		public Animator anim;
		public Rigidbody control;
		public CapsuleCollider cap_collider;
		public TrailRenderer speedTrail;
		public Transform headPoint;		// rig camera here. Transform point for smoothing.
		public Transform grabPoint;		// put item here. Also grab detect zone.
		public Transform footTracePoint;

		// Use this for initialization
		void Start ()
		{
				anim = GetComponent<Animator> ();
				control = GetComponent<Rigidbody> ();
				cap_collider = GetComponent<CapsuleCollider> ();
				speedTrail = GetComponent<TrailRenderer> ();
				headPoint = transform.Find ("Head Cam");
				grabPoint = transform.Find ("Head Cam/Grab Point");
				footTracePoint = transform.Find ("Foot Trace Point");

		
				feetRay = new Ray (footTracePoint.position, -transform.up);
				look_rotation = control.rotation.eulerAngles.y;
				Quaternion look = Quaternion.AngleAxis (look_rotation, control.transform.up);
				xz_a_limiter = 1;
				onGround = false;
				ave_slope = new Vector3 ();
				is_running = false;
				drag_dynamic = control.drag; // get default phys material on body.
		}

		// calculate movement.
		void FixedUpdate ()
		{
				input_z = Input.GetAxis ("Forward");
				input_x = Input.GetAxis ("Straft");
				input_y = Input.GetAxis ("Jump");
				currentAnimState = anim.GetCurrentAnimatorStateInfo (0);

				// detect the ground and slope.
				// TODO make ray checks on timer.
				feetRay.origin = footTracePoint.position;
				feetRay.direction = -transform.up;
				onGround = (Physics.Raycast (feetRay, out hit, 0.45f));
				
				// slope samples are averaged for complex changes.
				ave_slope = Vector3.RotateTowards (ave_slope, hit.normal, 1f, 1);  // might need last value 0f.

				// moving state check and stopping logic.
				anim.SetBool (isMov_zx, control.velocity.sqrMagnitude > TINY_NUM * TINY_NUM);

				// change control when on the ground.
				// movement checks for state.
				if (onGround) {
						anim.SetBool (isGrounded, true);
						if (currentAnimState.nameHash == st_jump) {
								anim.SetBool (isJumping, false);
						}
				} else {
						anim.SetBool (isGrounded, false);

				}

				// Moving backwards reduces sprint.
				float v_boost = Mathf.Lerp (1f, MOVE_FORCE_RATIO, sprint_p);
				float speed_v = control.velocity.magnitude;				// last frame v.
				base_forward_a = 0;

				// move heading.
				// xz move normalized.
				base_forward_a = (input_z);
				base_strafe_a = (input_x);
				Vector3 xz_a_norm = new Vector3 (base_strafe_a, 0, base_forward_a).normalized;
				base_forward_a = xz_a_norm.z * MOVE_ACCEL;
				base_strafe_a = xz_a_norm.x * MOVE_ACCEL;

				// get zx and y velocity vectors.
				Vector3 xz_v = Vector3.ProjectOnPlane (control.velocity, transform.up);
				Vector3 y_v = Vector3.Project (control.velocity, transform.up);

				//sprint
				if (Input.GetButton ("Sprint") && input_z > 0) {
						is_running = true;
						sprint_p = Mathf.Min ((sprint_p += MOVE_SPRINT_A), 1f);
				} else {
						sprint_p = Mathf.Max ((sprint_p -= MOVE_SPRINT_DA), 0f);
						is_running = false;
				}

				// jump
				if (Input.GetButton ("Jump")) {
						// holding jump and if on ground and not jumping. begin upforce and jump state.
						if (currentAnimState.nameHash != st_jump) {
								if (onGround) {
										// only do so if animating in a state. (does not inturrupt current animation).
										if (true) {// !anim.IsInTransition (0)) {   
												anim.SetBool (isJumping, true);
												jump_a = JUMP_UPLIFT;
										}
								}
						} else {
								if (jump_a > TINY_NUM) {
										jump_a -= jump_a * JUMP_CONTROL + TINY_NUM;// .01f;
								}
						}
						//else  if holding jump and currently jumping. drop jump force.
				} 
				// if jumping and moving downwards, then not jumping.
				//float upsign = Vector3.Dot(y_v, control.transform.up);
				if (jump_a > 0) {
						jump_a -= jump_a * JUMP_CONTROL + TINY_NUM;// .01f;
				}
				if (currentAnimState.nameHash == st_jump && Vector3.Dot (y_v, control.transform.up) <= 0) {
						anim.SetBool (isJumping, false);
						jump_a = 0;
				}
				

				/*
		// slope force. Now handled later.
		if (dot_slope < SLOPE_SLIP_ANGLE) {
			Vector3 counter_vector = -transform.up;
			if (!onGround) {
				counter_vector = Vector3.ProjectOnPlane (-control.velocity.normalized, ave_slope);
			}
			//control.AddForce (counter_vector * MOVE_ACCEL * 1 , ForceMode.Acceleration);
		} else {
			//control.AddForce (Vector3.ProjectOnPlane (transform.up, ave_slope).normalized * Physics.gravity.magnitude, ForceMode.Impulse);
		} */
		

				// speed restricter. Don't need vertical speed.
				float set_forward_limiter = 1;
				if (is_running) {
						if (xz_v.sqrMagnitude > RUN_MAX_SPEED * RUN_MAX_SPEED) {
								set_forward_limiter = 0.1f;
						}
				} else {
						if (xz_v.sqrMagnitude > MOVE_MAX_SPEED * MOVE_MAX_SPEED) {
								set_forward_limiter = 0.1f;
						}
				}
				xz_a_limiter = Mathf.MoveTowards (xz_a_limiter, set_forward_limiter, .5f);



				// Combine the forward control values and limiter.
				Vector3 a_sum = (transform.forward * base_forward_a + transform.right * base_strafe_a) * v_boost * xz_a_limiter;

				// when in air or high slope, then very little drive power.
				if (!onGround || Vector3.Dot (footTracePoint.up, ave_slope.normalized) < SLOPE_SLIP_ANGLE) {
						a_sum *= .1f;
				}

				// resistance force so that player stops sliding where move force is zero.
				Vector3 xz_break_heading = -(control.velocity.normalized - a_sum.normalized);
				xz_break_heading.y = 0;

				// breaking force
				control.AddForce (xz_break_heading * MOVE_ACCEL * BREAK_CONTROL, ForceMode.Acceleration);
				// move force
				control.AddForce (a_sum, ForceMode.Acceleration);
				// jump force
				control.AddForce (transform.up * jump_a, ForceMode.Acceleration);

				// rotate body. Its breaks mouse input.
				//Quaternion rot_y = Quaternion.AngleAxis (rotation_y, control.transform.up);
				//control.MoveRotation (control.rotation * rot_y);

				// rotate with force towards the hidden mouse compass.
				float y_angle_body = control.rotation.eulerAngles.y;
				float diffAngle = (look_rotation - y_angle_body);
				//float rot_y_look_force = Mathf.Pow (diffAngle * 1f, 3);
				float rot_y_look_force = diffAngle * TURN_ACCEL;
				control.AddTorque (0, rot_y_look_force, 0, ForceMode.Acceleration);
				//Debug.Log (y_angle_body + " " + look_rotation + " " + rot_y_look_force);


				// pitch cam and grab point. done in physics time. The Camera follows this.
				Vector3 headRot = headPoint.rotation.eulerAngles;
				headRot.x = rot_a_x;
				headPoint.rotation = Quaternion.Euler (headRot);


				/*
				// animation stuff for blending.
				// overly complicated forward speed projection so i can get the direction as well.
				float move_z_dot_sign = Vector3.Dot (control.velocity, transform.forward);

				// animator controls.
				// must normalize to fit animator and sync movement.
				float move_z_norm = Vector3.Project (a_sum, transform.forward).magnitude * INVS_RUN_MAX_SPEED * Mathf.Sign (move_z_dot_sign);
				float rot_y_norm = rotation_y * INVS_ROTATION_MAX_SPEED;
				rot_y_norm = Mathf.Clamp01 (rot_y_norm);
				anim.SetFloat (mov_axis_z, move_z_norm);
				anim.SetFloat (turn_y, rot_y_norm);
*/

				// effects
				speedTrail.enabled = (a_sum.sqrMagnitude > 1);
		}


		// used to cross the phyics frames to render frames.
		void Update ()
		{

				// Robert Smiley
				// Extract values from the current buff
				// extract only if buff is not expired
				if(Time.timeSinceLevelLoad - PlayerVars.buff.time < PlayerVars.buff.duration){
					buffSpeed = PlayerVars.buff.speed;
				} else{
				// expired! set everything to zero!
					buffSpeed = 0.0f;
				}
//				Debug.Log(buffSpeed);

				// mouse look.
				rotation_x = Input.GetAxis ("Mouse Y") * mouse_sensitivity;		// this does not affect collision.
				rotation_y = Input.GetAxis ("Mouse X") * mouse_sensitivity;		// this case is special for applying physics.
				look_rotation = control.rotation.eulerAngles.y + rotation_y;
				rot_a_x += -rotation_x;
				rot_a_x = Mathf.Clamp (rot_a_x, -85, 85);

				// camera transform. To Smooth between fixedUpdate() time and update() time.
				Vector3 useless_v = new Vector3 (); // Could be used for movement effects...
				headCamera.transform.position = Vector3.SmoothDamp (headCamera.transform.position, 
		                                                    headPoint.position, 
		                                                    ref useless_v, .1f * Time.deltaTime);

				// smooth rotation of cam.
				Vector3 camForwards = headPoint.transform.forward;
				headCamera.transform.rotation = Quaternion.LookRotation (Vector3.RotateTowards (headCamera.transform.forward,
		                                                       camForwards,
		                                                       ROTATION_MAX_SPEED * Time.deltaTime, .1f * Time.deltaTime), control.transform.up);


		}
}
