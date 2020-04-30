using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

[RequireComponent(typeof(PlayerMovement))]
public class PlayerMovement : MonoBehaviour
{
	public float movementSpeed;
	public float slowedMovementSpeedMultiplier;
	public float speedUpMovementSpeedMultiplier;
	public float timeSlowedFor = 3;
	private FixedJoystick joystick;
	private PhotonView photonView;
	private Rigidbody rigidbody;
	private Vector3 movementDirection;

	bool isSlowed;
	float timer;
	bool hasSpeedUpPassive;

	void Start()
	{

		photonView = GetComponent<PhotonView>();
		rigidbody = GetComponent<Rigidbody>();
		rigidbody.useGravity = false;
#if UNITY_IPHONE || UNITY_ANDROID
        joystick = GameObject.Find("Left Joystick").GetComponent<FixedJoystick>();
#endif
	}

	void FixedUpdate()
	{
		if (photonView.IsMine)
		{
			BasicMovement();
			SlowdownTimer();
		}

	}

	void BasicMovement()
	{
#if UNITY_IPHONE || UNITY_ANDROID || UNITY_WEBGL
		movementDirection.x = joystick.Horizontal;
		movementDirection.y = 0;
		movementDirection.z = joystick.Vertical;
#elif UNITY_EDITOR || UNITY_STANDALONE
		movementDirection.x = Input.GetAxis("Horizontal");
		movementDirection.y = 0;
		movementDirection.z = Input.GetAxisRaw("Vertical");
#endif

		movementDirection = movementDirection.normalized;

		Vector3 movementVelocity = movementDirection * movementSpeed * ((hasSpeedUpPassive)? speedUpMovementSpeedMultiplier: 1.0f ) * Time.fixedDeltaTime * ((isSlowed)? slowedMovementSpeedMultiplier : 1.0f);
		rigidbody.velocity = movementVelocity;
	}

	void SlowdownTimer ()
	{
		if (isSlowed)
		{
			if (timer <= 0.0f)
			{
				isSlowed = false;
			}
			else
			{
				timer -= Time.fixedDeltaTime;
			}
		}
	}

	public void Slowed ()
	{
		timer = timeSlowedFor;
		isSlowed = true;
	}

	public void AssignSpeedUp ()
	{
		hasSpeedUpPassive = true;
	}
}
