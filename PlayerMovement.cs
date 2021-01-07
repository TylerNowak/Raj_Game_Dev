using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Threading;

public class PlayerMovement : MonoBehaviour
{
	/*************************************************************************************************************************************************************
     * This Script is to apply movement and animation to Character in Unity																					                                           *
     * Created by Tyler Nowak                                                                                                                                  *
     ***********************************************************************************************************************************************************/

	//Vector for rotation of Character
	Vector3 rot = Vector3.zero;
	//Vector for movement of Character
	Vector3 moveDirection = Vector3.zero;

	//Rotation Speed & "Multiplier" (more of an addition to adjust speed)
	float rotSpeed = 40f;
	public float rotMultiplier = .025f;

	//Movement speed, Jump speed of application of forces, and the amount of Gravity applied
	public float moveSpeed = 15f;
	public float jumpSpeed = 200f;
	public float gravity = 80;

	//Whether Character is Grounded and/or Jumping
	public bool grounded = true;
	public bool jumping = false;
	
	//Whether Character is open/closed or in it's Rolling state
	public bool closed = false;
	public bool rolling = false;
	
	//Global variables to apply physics and animations to.
	CharacterController controller;
	Animator anim;

	/// <summary>
	/// Method is used for initialization.
	/// </summary>
	void Awake()
	{
		//Initializing the Character Controller
		controller = GetComponent<CharacterController>();
		//Initializing the Animator
		anim = gameObject.GetComponent<Animator>();
		//Initial Rotation Position
		gameObject.transform.eulerAngles = rot;
	}

	/// <summary>
	/// Update is called once per frame.
	/// </summary>
	void Update()
	{
		//Check what key is pressed
		CheckKey();
		//Apply movement determined by the key press
		setMovement();
		//Apply rotation
		gameObject.transform.eulerAngles = rot;
		//Check if Character is on the ground
		GroundCheck();
	}


	/// <summary>
	/// This method applies the movement corresponding to the key that is pressed.
	/// </summary>
	void setMovement()
	{

		//Check if Character is on the ground before applying motions that are only applied while touching the ground
		GroundCheck();

		//Character should not be able to move if in closed position
		if (!closed)
        {
			//Character can only jump if the are in an open state, grounded, and not rolling
			if (grounded && Input.GetKeyDown(KeyCode.Space) && !rolling)
			{
				moveDirection.y = jumpSpeed;
				moveDirection.y -= gravity * Time.deltaTime;
				var flags = controller.Move(moveDirection * Time.deltaTime);
				grounded = ((controller.collisionFlags & CollisionFlags.Below) != 0);
			}

			//Apply forward movement in direction Character is facing and apply gravity
			if (Input.GetKey(KeyCode.W))
			{
				moveDirection = transform.TransformDirection(Vector3.forward * moveSpeed);
				// Apply gravity
				moveDirection.y -= gravity * Time.deltaTime;
				// Move the controller
				var flags = controller.Move(moveDirection * Time.deltaTime);
				grounded = ((controller.collisionFlags & CollisionFlags.Below) != 0);
			}
		}
	}

	/// <summary>
    /// This method checks if the character is grounded.
    /// While checking and updating the grounded flag also applies 
    /// the appropriate animation.
    /// </summary>
	void GroundCheck()
    {
		//Check if grounded and not jumping
		if (!grounded && !jumping)
		{
			moveDirection.y -= gravity * Time.deltaTime;
			grounded = ((controller.collisionFlags & CollisionFlags.Below) != 0);
			anim.SetBool("Falling_Anim", true);
			anim.SetBool("Jump_Anim", false);
		}//Check if jumping
		else if (jumping)
        {
			anim.SetBool("Jump_Anim", true);
		}//Not falling and not jumping
        else
        {
			anim.SetBool("Falling_Anim", false);
			anim.SetBool("Jump_Anim", false);
		}

		//This check is to stop the Jump_Anim once grounded and if the key is still pressed
		if(anim.GetBool("Jump_Anim") && grounded)
        {
			anim.SetBool("Jump_Anim", false);
			jumping = false;
		}
	}

	/// <summary>
    /// This method is used to determine which key is being 
    /// pressed as well to apply the appropriate
    /// animations to said key press.
    /// </summary>
	void CheckKey()
	{
		//Playing walking Animation
		if (Input.GetKey(KeyCode.W))
		{
			anim.SetBool("Walk_Anim", true);
		}
		else if (Input.GetKeyUp(KeyCode.W))
		{
			anim.SetBool("Walk_Anim", false);
		}

		//Playing Rotate Left Animation
		if (Input.GetKey(KeyCode.A))
		{
			rot[1] -= rotSpeed * (Time.deltaTime + rotMultiplier);
			anim.SetBool("Rot_Left_Anim", true);
		}
		else if (Input.GetKeyUp(KeyCode.A))
		{
			anim.SetBool("Rot_Left_Anim", false);
		}

		//Playing Rotate Right Animation
		if (Input.GetKey(KeyCode.D))
		{
			rot[1] += rotSpeed * (Time.deltaTime + rotMultiplier);
			anim.SetBool("Rot_Right_Anim", true);
		}
		else if (Input.GetKeyUp(KeyCode.D))
		{
			anim.SetBool("Rot_Right_Anim", false);
		}

		//Playing Jump Animation
		if (Input.GetKey(KeyCode.Space))
		{
			jumping = true;
			GroundCheck();
		}
		else if (Input.GetKeyUp(KeyCode.Space))
		{
			jumping = false;
		}

		//Playing Roll Animation
		if (Input.GetKeyDown(KeyCode.R) && !closed)
		{
			if (anim.GetBool("Roll_Anim"))
			{
				anim.SetBool("Roll_Anim", false);
				moveSpeed = 15f;
				rolling = false;
			}
			else
			{
				anim.SetBool("Roll_Anim", true);
				moveSpeed = 25f;
				rolling = true;
			}
		}

		//Playing Close Animation
		if (Input.GetKeyDown(KeyCode.LeftControl) && !rolling)
		{
			if (!anim.GetBool("Open_Anim"))
			{
				anim.SetBool("Open_Anim", true);
				closed = false;
			}
			else
			{
				anim.SetBool("Open_Anim", false);
				closed = true;
			}
		}
	}
}
